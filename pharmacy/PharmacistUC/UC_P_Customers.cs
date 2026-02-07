using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pharmacy.PharmacistUC
{
    public partial class UC_P_Customers : UserControl
    {
        private bool _binding = false;
        private readonly function fn = new function();   // your DB helper

        public UC_P_Customers()
        {
            InitializeComponent();
            // Events are already wired in Designer; no need to wire here again.
        }

        /* ======================= Lifecycle ======================= */

        private void UC_P_Customers_Load(object sender, EventArgs e)
        {
            if (Session.UserId <= 0) return;

            LoadMyCustomers();
            LoadMyInventory();
        }
        //Customers
        private void LoadMyCustomers()
        {
            if (Session.UserId <= 0) return;
            try
            {
                _binding = true;

                string q =
                    $"SELECT CustomerID, Username FROM Customers " +
                    $"WHERE PharmacistID = {Session.UserId} ORDER BY Username;";

                var ds = fn.getData(q);

                if (ds != null && ds.Tables.Count > 0)
                {
                    combCustomers.DisplayMember = "Username";
                    combCustomers.ValueMember = "CustomerID";
                    combCustomers.DataSource = ds.Tables[0];
                    combCustomers.SelectedIndex = -1;
                }
                else
                {
                    combCustomers.DataSource = null;
                    combCustomers.Items.Clear();
                    combCustomers.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message);
            }
            finally
            {
                _binding = false;
            }
        }

        private int? GetSelectedCustomerId()
        {
            // Nothing selected
            if (combCustomers == null || combCustomers.SelectedIndex < 0)
                return null;

            // If DataTable is the DataSource
            if (combCustomers.SelectedItem is DataRowView drv &&
                drv.Row.Table.Columns.Contains("CustomerID"))
            {
                return Convert.ToInt32(drv["CustomerID"]);
            }

            // If using ValueMember/SelectedValue
            var val = combCustomers.SelectedValue;
            if (val == null || val == DBNull.Value) return null;

            if (int.TryParse(val.ToString(), out int id)) return id;

            return null;
        }

        private void combCustomers_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_binding) return;

            var customerId = GetSelectedCustomerId();
            if (customerId == null) return;

            LoadCustomerMedicines(customerId.Value);
        }

        /* ======================= Inventory ======================= */

        private void LoadMyInventory(string search = null)
        {
            if (Session.UserId <= 0) return;
            if (dgvInventory == null) return;
            try
            {
                string filter = string.IsNullOrWhiteSpace(search)
                    ? string.Empty
                    : " AND m.MedName LIKE '%" + search.Replace("'", "''") + "%'";
                string query = $@"
                    SELECT pm.MedicineID,
                           m.MedName       AS [Medicine],
                           m.[Description],
                           pm.QtyAvailable AS [Available]
                    FROM PharmacistMedicines pm
                    JOIN Medicines m ON m.MedicineID = pm.MedicineID
                    WHERE pm.PharmacistID = {Session.UserId}
                          {filter}
                    ORDER BY m.MedName;";

                var ds = fn.getData(query);

                if (ds != null && ds.Tables.Count > 0)
                {
                    dgvInventory.DataSource = ds.Tables[0];
                    if (dgvInventory.Columns.Contains("MedicineID"))
                        dgvInventory.Columns["MedicineID"].Visible = false;
                }
                else
                {
                    dgvInventory.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading inventory: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadMyInventory(txtSearch?.Text?.Trim());
        }

        /* ======================= Customer Prescriptions ======================= */

        private void LoadCustomerMedicines(int customerId)
        {
            try
            {
                if (dgvCustomerPrescriptions == null) return;

                string query = $@"
                    SELECT m.MedName       AS [Medicine],
                           cm.TimesPerDay,
                           cm.UnitsPerDose,
                           cm.StartDate,
                           cm.DurationDays,
                           cm.EndDate,
                           u.username       AS [Prescribed By]
                    FROM CustomerMedicines cm
                    JOIN Medicines m ON m.MedicineID = cm.MedicineID
                    JOIN users u ON u.id = cm.PrescribedByPharmacistID
                    WHERE cm.CustomerID = {customerId}
                    ORDER BY cm.StartDate DESC;";

                var ds = fn.getData(query);
                dgvCustomerPrescriptions.DataSource =
                    (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading prescriptions: " + ex.Message);
            }
        }

        //SAFETY: allergies & interactions
        // Calls your stored procedure sp_CheckPrescriptionSafety and returns its first table.
        private DataTable RunSafetyCheck(int customerId, int medicineId)
        {
            string q =
                $"EXEC dbo.sp_CheckPrescriptionSafety @CustomerID={customerId}, @MedicineID={medicineId};";
            var ds = fn.getData(q);
            return (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : null;
        }

        private enum SafetyDecision { Safe, Caution, Block }

        // Turns the result of the safety SP into user-friendly text and a decision.
        private SafetyDecision EvaluateSafety(DataTable issues, out string message)
        {
            var sb = new StringBuilder();

            if (issues == null || issues.Rows.Count == 0)
            {
                message = "No allergies or interactions found.";
                return SafetyDecision.Safe;
            }
            // Pretty print grouped by IssueType
            foreach (var grp in issues.AsEnumerable().GroupBy(r => r.Field<string>("IssueType")))
            {
                sb.AppendLine(grp.Key + ":");
                foreach (var r in grp)
                {
                    string item1 = r.Field<string>("Item1");
                    string item2 = r.Field<string>("Item2");
                    string sev = r.Field<string>("Severity");
                    string note = r.Field<string>("Note");

                    if (grp.Key == "Allergy")
                        sb.AppendLine($"  • {item1}   [Severity: {sev}]   {note}");
                    else
                        sb.AppendLine($"  • {item1} ↔ {item2}   [Severity: {sev}]   {note}");}
                sb.AppendLine();
            }
            bool hasAllergy = issues.AsEnumerable().Any(r => r.Field<string>("IssueType") == "Allergy");
            bool hasMajor = issues.AsEnumerable().Any(r => string.Equals(r.Field<string>("Severity"), "Major", StringComparison.OrdinalIgnoreCase));
            bool hasModerate = issues.AsEnumerable().Any(r => string.Equals(r.Field<string>("Severity"), "Moderate", StringComparison.OrdinalIgnoreCase));
            if (hasAllergy || hasMajor)
            {
                sb.AppendLine("Major risk detected (allergy or major interaction).");
                message = sb.ToString();
                return SafetyDecision.Block;
            }
            if (hasModerate)
            {
                sb.AppendLine(" Moderate interactions detected.");
                message = sb.ToString();
                return SafetyDecision.Caution;
            }
            sb.AppendLine("Only minor or no interactions.");
            message = sb.ToString();
            return SafetyDecision.Safe;
        }
        // Button: check safety for selected patient + selected medicine from inventory
        private void btnCheckSafety_Click(object sender, EventArgs e)
        {
            var cid = GetSelectedCustomerId();
            if (cid == null)
            {
                MessageBox.Show("Please select a patient first.");
                return;
            }
            if (!dgvInventory.Columns.Contains("MedicineID") ||
                dgvInventory.CurrentRow == null ||
                dgvInventory.CurrentRow.Cells["MedicineID"]?.Value == null)
            {
                MessageBox.Show("Select a medicine from My Inventory first.");
                return;
            }
            int medicineId = Convert.ToInt32(dgvInventory.CurrentRow.Cells["MedicineID"].Value);
            var dt = RunSafetyCheck(cid.Value, medicineId);
            var decision = EvaluateSafety(dt, out string msg);
            var icon = decision == SafetyDecision.Block ? MessageBoxIcon.Stop : decision == SafetyDecision.Caution ? MessageBoxIcon.Exclamation :MessageBoxIcon.Information;
            MessageBox.Show(msg, "Safety Check", MessageBoxButtons.OK, icon);
        }

        /* ======================= Prescribe ======================= */

        private void btnPrescribe_Click(object sender, EventArgs e)
        {
            if (Session.UserId <= 0)
            {
                MessageBox.Show("Invalid session.");
                return;
            }

            if (combCustomers == null || combCustomers.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a customer first.");
                return;
            }

            if (dgvInventory == null)
            {
                MessageBox.Show("Inventory grid not found or not initialized.");
                return;
            }

            if (dgvInventory.CurrentRow == null)
            {
                MessageBox.Show("Please select a medicine from the inventory.");
                return;
            }

            if (!dgvInventory.Columns.Contains("MedicineID") ||
                dgvInventory.CurrentRow.Cells["MedicineID"]?.Value == null)
            {
                MessageBox.Show("MedicineID column not found or not selected.");
                return;
            }

            if (!dgvInventory.Columns.Contains("Available") ||
                dgvInventory.CurrentRow.Cells["Available"]?.Value == null)
            {
                MessageBox.Show("Available quantity column missing.");
                return;
            }

            int customerId = GetSelectedCustomerId() ?? 0;
            int medicineId = Convert.ToInt32(dgvInventory.CurrentRow.Cells["MedicineID"].Value);
            decimal qtyAvail = Convert.ToDecimal(dgvInventory.CurrentRow.Cells["Available"].Value);

            int timesPerDay = (int)numTimesPerDay.Value;
            decimal dose = numUnitsPerDose.Value;
            int days = (int)numDurationDays.Value;

            decimal totalRequired = timesPerDay * dose * days;

            if (qtyAvail < totalRequired)
            {
                MessageBox.Show("Not enough quantity available in stock.");
                return;
            }

            // Safety check before saving
            var safetyDt = RunSafetyCheck(customerId, medicineId);
            var decision = EvaluateSafety(safetyDt, out string safetyMsg);

            if (decision == SafetyDecision.Block)
            {
                MessageBox.Show(safetyMsg, "Unsafe", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (decision == SafetyDecision.Caution)
            {
                var r = MessageBox.Show(safetyMsg + "\n\nProceed anyway?",
                                        "Caution", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (r != DialogResult.Yes) return;
            }

            try
            {
                string query = $@"
BEGIN TRY
    BEGIN TRAN;

    UPDATE PharmacistMedicines
    SET QtyAvailable = QtyAvailable - {totalRequired}
    WHERE PharmacistID = {Session.UserId}
      AND MedicineID   = {medicineId}
      AND QtyAvailable >= {totalRequired};

    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR(N'Medicine not available or insufficient quantity.', 16, 1);
        ROLLBACK TRAN;
        RETURN;
    END

    INSERT INTO CustomerMedicines
        (CustomerID, MedicineID, TimesPerDay, UnitsPerDose, StartDate, DurationDays, PrescribedByPharmacistID)
    VALUES
        ({customerId}, {medicineId}, {timesPerDay}, {dose}, CAST(GETDATE() AS DATE), {days}, {Session.UserId});

    COMMIT TRAN;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRAN;
    DECLARE @msg NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(@msg, 16, 1);
END CATCH;";

                fn.setData(query, "Prescription added successfully.");

                LoadMyInventory();
                LoadCustomerMedicines(customerId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        /* ======================= Reset / Delete ======================= */

        private void reset_Click(object sender, EventArgs e)
        {
            try
            {
                // refresh inventory keeping current search text
                LoadMyInventory(txtSearch?.Text?.Trim());

                // refresh patient prescriptions if a patient is selected
                var customerId = GetSelectedCustomerId();
                if (customerId != null)
                    LoadCustomerMedicines(customerId.Value);
                else
                    dgvCustomerPrescriptions.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reset failed: " + ex.Message);
            }
        }

        private void dgvCustomerPrescriptions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // optional cell actions here
        }

        private void delete_Click(object sender, EventArgs e)
        {
            var customerId = GetSelectedCustomerId();
            if (customerId == null)
            {
                MessageBox.Show("Please select a customer first.");
                return;
            }

            if (dgvCustomerPrescriptions?.CurrentRow == null)
            {
                MessageBox.Show("Please select a prescription row to delete.");
                return;
            }

            var row = dgvCustomerPrescriptions.CurrentRow;

            string medName = row.Cells["Medicine"]?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(medName))
            {
                MessageBox.Show("Cannot read medicine name from the selected row.");
                return;
            }

            int timesPerDay = Convert.ToInt32(row.Cells["TimesPerDay"].Value);
            decimal unitsPerDose = Convert.ToDecimal(row.Cells["UnitsPerDose"].Value,
                                         System.Globalization.CultureInfo.InvariantCulture);
            int durationDays = Convert.ToInt32(row.Cells["DurationDays"].Value);
            DateTime startDt = Convert.ToDateTime(row.Cells["StartDate"].Value);

            decimal totalUnits = timesPerDay * unitsPerDose * durationDays;

            if (MessageBox.Show("Delete this prescription and restore stock?",
                                "Confirm delete",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            string q = $@"
BEGIN TRY
  BEGIN TRAN;

  DECLARE @MedID INT = (SELECT TOP(1) MedicineID
                        FROM dbo.Medicines
                        WHERE MedName = '{medName.Replace("'", "''")}');

  IF @MedID IS NULL
  BEGIN
      RAISERROR('Medicine not found by name.',16,1);
      ROLLBACK TRAN;
      RETURN;
  END

  ;WITH cte AS (
      SELECT TOP (1) *
      FROM dbo.CustomerMedicines
      WHERE CustomerID = {customerId}
        AND MedicineID  = @MedID
        AND StartDate   = '{startDt:yyyy-MM-dd}'
        AND TimesPerDay = {timesPerDay}
        AND UnitsPerDose= {unitsPerDose.ToString(System.Globalization.CultureInfo.InvariantCulture)}
        AND DurationDays= {durationDays}
        AND PrescribedByPharmacistID = {Session.UserId}
      ORDER BY StartDate DESC
  )
  DELETE FROM cte;

  IF @@ROWCOUNT = 0
  BEGIN
      RAISERROR('No matching prescription was found to delete.',16,1);
      ROLLBACK TRAN;
      RETURN;
  END

  UPDATE dbo.PharmacistMedicines
  SET QtyAvailable = QtyAvailable + {totalUnits.ToString(System.Globalization.CultureInfo.InvariantCulture)}
  WHERE PharmacistID = {Session.UserId}
    AND MedicineID   = @MedID;

  COMMIT TRAN;
END TRY
BEGIN CATCH
  IF XACT_STATE() <> 0 ROLLBACK TRAN;
  DECLARE @msg NVARCHAR(4000)=ERROR_MESSAGE();
  RAISERROR(@msg,16,1);
END CATCH;";

            try
            {
                fn.setData(q, "Prescription deleted and stock restored.");
                LoadMyInventory(txtSearch?.Text?.Trim());
                LoadCustomerMedicines(customerId.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed: " + ex.Message);
            }
        }
    }
}
