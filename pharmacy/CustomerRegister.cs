using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy
{
    public partial class CustomerRegister : Form
    {
        function fn = new function();
        String query;
        public CustomerRegister()
        {
            InitializeComponent();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            customer c = new customer();
            c.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string u = textBox1.Text.Trim();
            string e1 = textBox2.Text.Trim();
            string m = textBox5.Text.Trim();
            string p = textBox3.Text.Trim();
            string c = textBox4.Text.Trim();

          
            if (string.IsNullOrWhiteSpace(u) ||
                string.IsNullOrWhiteSpace(e1) ||
                string.IsNullOrWhiteSpace(m) ||
                string.IsNullOrWhiteSpace(p) ||
                string.IsNullOrWhiteSpace(c))
            {
                MessageBox.Show("Please fill in all fields.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!e1.Contains("@") || !e1.Contains("."))
            {
                MessageBox.Show("Invalid email format.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!m.All(char.IsDigit))
            {
                MessageBox.Show("Mobile phone must contain digits only.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (p != c)
            {
                MessageBox.Show("Passwords do not match.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string emailEsc = e1.Replace("'", "''");
            query = $"SELECT TOP 1 CustomerID FROM Customers WHERE Email = N'{emailEsc}'";
            DataSet ds = fn.getData(query);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("This email is already registered.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            
            string uEsc = u.Replace("'", "''");
            string mEsc = m.Replace("'", "''");
            string pEsc = p.Replace("'", "''"); 

            
            query = "INSERT INTO Customers (Username, Email, Mobile, Password) " +$"VALUES (N'{uEsc}', N'{emailEsc}', N'{mEsc}', N'{pEsc}')";
            fn.setData(query, "Account created successfully.");
            query = $"SELECT TOP 1 CustomerID, Username FROM Customers WHERE Email = N'{emailEsc}'";
            DataSet ds2 = fn.getData(query);
            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                var row = ds2.Tables[0].Rows[0];
                Session.UserId = Convert.ToInt32(row["CustomerID"]);
                Session.Username = row["Username"].ToString();

                
                var main = new CustomerInfo(); 
                main.Show();
                this.Hide();
            }
            else
            {
               
                textBox1.Clear();
                textBox2.Clear();
                textBox5.Clear();
                textBox3.Clear();
                textBox4.Clear();
            }
        }
    }
}
