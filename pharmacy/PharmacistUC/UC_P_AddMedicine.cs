using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace Pharmacy.PharmacistUC
{
    public partial class UC_P_AddMedicine : UserControl
    {
        private readonly function fn = new function();
        

        public UC_P_AddMedicine()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session.UserId <= 0)
                {
                    MessageBox.Show("Invalid session. Please login again.", "Session",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // mid (required, numeric)
                if (!int.TryParse(textBox1.Text.Trim(), out int mid) || mid <= 0)
                {
                    MessageBox.Show("Medicine ID (mid) must be a positive whole number.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(textBox2.Text) || 
                    string.IsNullOrWhiteSpace(textBox3.Text) || 
                    string.IsNullOrWhiteSpace(textBox4.Text) || 
                    string.IsNullOrWhiteSpace(textBox5.Text))   
                {
                    MessageBox.Show("Enter all required data.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string mname = textBox2.Text.Trim().Replace("'", "''");
                string mnumber = textBox3.Text.Trim().Replace("'", "''"); 
                string desc = (textBox6?.Text ?? "").Trim().Replace("'", "''");
                if (!int.TryParse(textBox4.Text.Trim(), out int quantity) || quantity < 0)
                {
                    MessageBox.Show("Quantity must be a non-negative whole number.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!decimal.TryParse(textBox5.Text.Trim(),
                        System.Globalization.NumberStyles.Number,
                        System.Globalization.CultureInfo.InvariantCulture, out decimal price) || price < 0)
                {
                    MessageBox.Show("Price Per Unit must be a non-negative number.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DateTime mDate = dateTimePicker1.Value.Date;
                DateTime eDate = dateTimePicker2.Value.Date;
                if (eDate < mDate)
                {
                    MessageBox.Show("Expire Date must be after Manufacturing Date.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var exists = fn.getData($"SELECT 1 FROM dbo.Medicines WHERE MedicineID = {mid};");
                if (exists != null && exists.Tables.Count > 0 && exists.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("This Medicine ID already exists. Please use a different ID.",
                        "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string q = $@"
BEGIN TRY
  BEGIN TRAN;

  -- Insert medicine with explicit ID (Description = @desc)
  SET IDENTITY_INSERT dbo.Medicines ON;

  INSERT INTO dbo.Medicines (MedicineID, MedName, [Description], Price)
  VALUES ({mid}, '{mname}', '{desc}', {price.ToString(System.Globalization.CultureInfo.InvariantCulture)});

  SET IDENTITY_INSERT dbo.Medicines OFF;

  -- Add to pharmacist's inventory (Qty)
  IF EXISTS (
      SELECT 1 FROM dbo.PharmacistMedicines
      WHERE PharmacistID = {Session.UserId} AND MedicineID = {mid}
  )
  BEGIN
      UPDATE dbo.PharmacistMedicines
      SET QtyAvailable = QtyAvailable + {quantity}
      WHERE PharmacistID = {Session.UserId} AND MedicineID = {mid};
  END
  ELSE
  BEGIN
      INSERT INTO dbo.PharmacistMedicines (PharmacistID, MedicineID, QtyAvailable)
      VALUES ({Session.UserId}, {mid}, {quantity});
  END;
  IF NOT EXISTS (
      SELECT 1 FROM dbo.medic
      WHERE PharmacistID = {Session.UserId} AND mid = CAST({mid} AS VARCHAR(20))
  )
  BEGIN
      INSERT INTO dbo.medic (mid, mname, mnumber, mDate, eDate, quantity, perUnit, PharmacistID)
      VALUES (CAST({mid} AS VARCHAR(20)), '{mname}', '{mnumber}',
              '{mDate:yyyy-MM-dd}', '{eDate:yyyy-MM-dd}', {quantity},
              {price.ToString(System.Globalization.CultureInfo.InvariantCulture)}, {Session.UserId});
  END
  ELSE
  BEGIN
      UPDATE dbo.medic
      SET quantity = quantity + {quantity},
          perUnit  = {price.ToString(System.Globalization.CultureInfo.InvariantCulture)},
          eDate    = '{eDate:yyyy-MM-dd}',
          mnumber  = '{mnumber}'
      WHERE PharmacistID = {Session.UserId} AND mid = CAST({mid} AS VARCHAR(20));
  END

  COMMIT TRAN;
END TRY
BEGIN CATCH
  IF XACT_STATE() <> 0 ROLLBACK TRAN;
  BEGIN TRY SET IDENTITY_INSERT dbo.Medicines OFF END TRY BEGIN CATCH END CATCH;
  DECLARE @msg NVARCHAR(4000) = ERROR_MESSAGE();
  RAISERROR(@msg, 16, 1);
END CATCH;";

                fn.setData(q, "Medicine added successfully.");
                clearAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            clearAll();
        }

        public void clearAll()
        {
            textBox1.Clear();  
            textBox2.Clear();  
            textBox3.Clear();  
            textBox4.Clear();  
            textBox5.Clear();
            textBox6.Clear();
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;
            textBox1.Focus();
        }
    }
}
