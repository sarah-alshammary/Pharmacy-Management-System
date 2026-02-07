using DGVPrinterHelper;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Pharmacy.PharmacistUC
{
    public partial class UC_P_SellMedicine : UserControl
    {
        function fn = new function();
        string query;
        DataSet ds;

        public UC_P_SellMedicine()
        {
            InitializeComponent();
        }
        private void UC_P_SellMedicine_Load(object sender, EventArgs e)
        {
            LoadList();   
        }
        private void LoadList(string term = null)
        {
            listBox1.Items.Clear();

            string filter = string.IsNullOrWhiteSpace(term)
                ? ""
                : " AND m.MedName LIKE '" + term.Replace("'", "''") + "%'";
            string q = $@"
SELECT m.MedName
FROM dbo.PharmacistMedicines pm
JOIN dbo.Medicines m ON m.MedicineID = pm.MedicineID
OUTER APPLY (
    SELECT MAX(d.eDate) AS eDate
    FROM dbo.medic d
    WHERE d.PharmacistID = {Session.UserId}
      AND TRY_CAST(d.mid AS INT) = m.MedicineID
) x
WHERE pm.PharmacistID = {Session.UserId}
  AND pm.QtyAvailable > 0
  AND ISNULL(x.eDate, CAST('2099-12-31' AS DATE)) > CAST(GETDATE() AS DATE)
{filter}
ORDER BY m.MedName;";

            ds = fn.getData(q);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                listBox1.Items.Add(ds.Tables[0].Rows[i][0].ToString());
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadList(textBox1.Text);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            LoadList(textBox1.Text);
            clearAll();
            dataGridView1.Rows.Clear();
            totalAmount = 0m;
            label9.Text = "JD. 0";
        } 
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                textBox4.Clear();

                string name = listBox1.GetItemText(listBox1.SelectedItem);
                textBox3.Text = name;

                string q = $@"
SELECT TOP 1
    m.MedicineID,
    m.Price,
    pm.QtyAvailable,
    ISNULL(x.eDate, CAST('2099-12-31' AS DATE)) AS eDate
FROM dbo.Medicines m
JOIN dbo.PharmacistMedicines pm
  ON pm.MedicineID = m.MedicineID AND pm.PharmacistID = {Session.UserId}
OUTER APPLY (
    SELECT MAX(d.eDate) AS eDate
    FROM dbo.medic d
    WHERE d.PharmacistID = {Session.UserId}
      AND TRY_CAST(d.mid AS INT) = m.MedicineID
) x
WHERE m.MedName = '{name.Replace("'", "''")}'
  AND pm.QtyAvailable > 0
  AND ISNULL(x.eDate, CAST('2099-12-31' AS DATE)) > CAST(GETDATE() AS DATE);";

                ds = fn.getData(q);
                if (ds.Tables[0].Rows.Count == 0) return;

                var r = ds.Tables[0].Rows[0];
                textBox2.Text = r["MedicineID"].ToString();                    
                textBox5.Text = Convert.ToDecimal(r["Price"]).ToString("0.##"); 
                dateTimePicker1.Value = Convert.ToDateTime(r["eDate"]);         
            }
            catch { }
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox4.Text))
            {
                if (decimal.TryParse(textBox5.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var unitPrice) &&
                    int.TryParse(textBox4.Text, out var units))
                {
                    var total = unitPrice * units;
                    textBox6.Text = total.ToString("0.##");
                }
                else
                {
                    textBox6.Clear();
                }
            }
            else
            {
                textBox6.Clear();
            }
        } 
        protected int n;
        protected long quantity, newQuantity;
        protected decimal totalAmount = 0m;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Select Medicine first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (dateTimePicker1.Value.Date <= DateTime.Today)
                {
                    MessageBox.Show("This medicine is expired and cannot be sold.",
                                    "Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int mid = int.Parse(textBox2.Text);
                if (!int.TryParse(textBox4.Text, out int sellUnits) || sellUnits <= 0)
                {
                    MessageBox.Show("Enter a valid number of units.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }  
                query = $@"SELECT QtyAvailable FROM dbo.PharmacistMedicines
                           WHERE PharmacistID = {Session.UserId} AND MedicineID = {mid};";
                ds = fn.getData(query);

                quantity = (ds.Tables[0].Rows.Count > 0)
                    ? Convert.ToInt64(ds.Tables[0].Rows[0][0])
                    : 0;

                newQuantity = quantity - sellUnits;

                if (newQuantity >= 0)
                {
                    
                    n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[0].Value = mid.ToString();     
                    dataGridView1.Rows[n].Cells[1].Value = textBox3.Text;     
                    dataGridView1.Rows[n].Cells[2].Value = dateTimePicker1.Text;
                    dataGridView1.Rows[n].Cells[3].Value = textBox5.Text;      
                    dataGridView1.Rows[n].Cells[4].Value = textBox4.Text;      
                    dataGridView1.Rows[n].Cells[5].Value = textBox6.Text;      
                    query = $@"
UPDATE dbo.PharmacistMedicines
SET QtyAvailable = {newQuantity}
WHERE PharmacistID = {Session.UserId} AND MedicineID = {mid};";
                    fn.setData(query, "Medicine added to cart.");

                    
                    if (decimal.TryParse(textBox6.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var line))
                        totalAmount += line;

                    label9.Text = "JD. " + totalAmount.ToString("0.##");

                    clearAll();
                    LoadList(textBox1.Text);
                }
                else
                {
                    MessageBox.Show($"Medicine is out of stock.\nOnly {quantity} left.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        
        int valueAmount;
        string valueId;
        protected long noOfunit;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                valueAmount = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                valueId = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                noOfunit = long.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    valueId = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();  
                    int index = dataGridView1.SelectedRows[0].Index;

                    
                    if (decimal.TryParse(dataGridView1.SelectedRows[0].Cells[5].Value.ToString(),
                                         NumberStyles.Number, CultureInfo.InvariantCulture, out var line))
                    {
                        totalAmount -= line;
                        if (totalAmount < 0) totalAmount = 0;
                        label9.Text = "JD. " + totalAmount.ToString("0.##");
                    }

                   
                    string q1 = $@"UPDATE dbo.PharmacistMedicines
                                   SET QtyAvailable = QtyAvailable + {noOfunit}
                                   WHERE PharmacistID = {Session.UserId}
                                     AND MedicineID = {valueId};";
                    fn.setData(q1, "Medicine removed from cart.");

                    
                    dataGridView1.Rows.RemoveAt(index);

                    LoadList(textBox1.Text);
                }
                else
                {
                    MessageBox.Show("Please select a row to remove.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

       
        private void button3_Click(object sender, EventArgs e)
        {
            DGVPrinter print = new DGVPrinter();
            print.Title = "Medicine Bill";
            print.SubTitle = string.Format("Date: {0:yyyy-MM-dd}", DateTime.Now.Date);
            print.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            print.PageNumbers = true;
            print.PageNumberInHeader = false;
            print.PorportionalColumns = true;
            print.HeaderCellAlignment = StringAlignment.Near;
            print.Footer = "Total Payable Amount: " + label9.Text;
            print.FooterSpacing = 15;
            print.PrintDataGridView(dataGridView1);

            totalAmount = 0m;
            label9.Text = "JD. 0";
            dataGridView1.Rows.Clear();
        }

        private void clearAll()
        {
            textBox2.Clear();              
            textBox3.Clear();              
            dateTimePicker1.Value = DateTime.Today;
            textBox5.Clear();              
            textBox4.Clear();              
            textBox6.Clear();              
        }
    }
}
