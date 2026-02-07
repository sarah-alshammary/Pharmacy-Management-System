using System;
using System.Data;
using System.Windows.Forms;

namespace Pharmacy.PharmacistUC
{
    public partial class UC_P_ViewMedicine : UserControl
    {
        function fn = new function();
        string query;
        string medicineId; 

        public UC_P_ViewMedicine()
        {
            InitializeComponent();
        }

        private void UC_P_ViewMedicine_Load(object sender, EventArgs e)
        {
            LoadForPharmacist();
        }

        
        private void LoadForPharmacist(string nameLike = null)
        {
            int pid = Session.UserId;
            string nameFilter = string.IsNullOrWhiteSpace(nameLike)
                ? ""
                : " AND (COALESCE(m.MedName, d.mname) LIKE '" + nameLike.Replace("'", "''") + "%')";

           
            query = $@"
WITH src AS (
    SELECT 
        d.id,
        d.mid,
        COALESCE(m.MedName, d.mname) AS mname,
        d.mnumber,
        d.mDate,
        d.eDate,
        d.quantity,
        COALESCE(m.Price, d.perUnit) AS perUnit,   -- ✅ هنا الحساب الصحيح
        COALESCE(m.MedicineID, TRY_CAST(d.mid AS INT)) AS MedIdKey
    FROM dbo.medic AS d
    LEFT JOIN dbo.Medicines AS m
           ON m.MedicineID = TRY_CAST(d.mid AS INT)
    WHERE EXISTS (
            SELECT 1
            FROM dbo.v_MyInventory AS vi
            WHERE vi.PharmacistID = {pid}
              AND vi.MedicineID = COALESCE(m.MedicineID, TRY_CAST(d.mid AS INT))
         )
    {nameFilter}
)


SELECT 
    MIN(id) AS id,
    CAST(MedIdKey AS VARCHAR(20)) AS mid,
    MAX(mname) AS mname,
    MAX(mnumber) AS mnumber,
    MIN(mDate) AS mDate,
    MAX(eDate) AS eDate,
    SUM(quantity) AS quantity,
    MAX(perUnit) AS perUnit     
FROM src
GROUP BY MedIdKey
ORDER BY mname;";

            setDataGridView(query);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadForPharmacist(textBox1.Text);
        }

        private void setDataGridView(string q)
        {
            DataSet ds = fn.getData(q);
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : null;

            // hide the internal auto ID, but keep mid visible
            if (dataGridView1.Columns.Contains("id"))
                dataGridView1.Columns["id"].Visible = false;

            // make sure mid stays visible (Medicine ID)
            if (dataGridView1.Columns.Contains("mid"))
            {
                dataGridView1.Columns["mid"].HeaderText = "Medicine ID";
                dataGridView1.Columns["mid"].Visible = true;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
                medicineId = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(medicineId))
            {
                MessageBox.Show("Please select a medicine first.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this medicine?",
                                "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            string safeMid = medicineId.Trim().Replace("'", "''");

            query = $@"
BEGIN TRY
    BEGIN TRAN;

    DECLARE @PhID INT = {Session.UserId};
    DECLARE @Mid INT = TRY_CAST('{safeMid}' AS INT);

    -- 1) Delete from PharmacistMedicines (the actual inventory for this pharmacist)
    DELETE FROM dbo.PharmacistMedicines
    WHERE PharmacistID = @PhID
      AND MedicineID = @Mid;

    -- 2) Delete from medic (just backup/legacy table)
    DELETE FROM dbo.medic
    WHERE PharmacistID = @PhID
      AND TRY_CAST(mid AS INT) = @Mid;

    COMMIT TRAN;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRAN;
    DECLARE @msg NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(@msg, 16, 1);
END CATCH;";

            fn.setData(query, "Medicine removed from your inventory.");
            LoadForPharmacist(textBox1.Text);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            LoadForPharmacist();
        }

        
        private void chart1_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
    }
}
