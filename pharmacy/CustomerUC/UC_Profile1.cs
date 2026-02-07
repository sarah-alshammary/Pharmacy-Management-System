using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.CustomerUC
{
    public partial class UC_Profile1 : UserControl
    {
        function fn = new function();
        String query;
        public UC_Profile1()
        {
            InitializeComponent();
            label7.Text = Session.Username ?? "";
            this.Click += pnlWhite_Click;
        }
        private void WireClickToPanel(Control parent,EventHandler handler)
        {
            parent.Click += handler;
            foreach (Control c in parent.Controls)
            {
                c.Click += handler;
                if (c.HasChildren) WireClickToPanel(c, handler);
            }
        }
        private void LoadCustomer()
        {
            if (Session.UserId <= 0)
            {
                MessageBox.Show("No logged-in user.");
                return;
            }
            query = $"SELECT TOP 1 Username, Email, Mobile, Password FROM Customers WHERE CustomerID = {Session.UserId}";
            DataSet ds = fn.getData(query);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("User not found.");
                return;
            }
            var row = ds.Tables[0].Rows[0];
            textBox1.Text = row["Username"].ToString();
            textBox2.Text = row["Email"].ToString();
            textBox5.Text = row["Mobile"].ToString();
            textBox3.Text = row["Password"].ToString();
        }
        private void UC_Profile1_Enter(object sender, EventArgs e){}
        private void button1_Click(object sender, EventArgs e)
        {
            if(Session.UserId<=0)
            {
                MessageBox.Show("No logged-in user.");
                return;
            }
            if(string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text)|| string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("All fields are required.");
                return;
            }
            string u = textBox1.Text.Trim().Replace("'", "''");
            string e1 = textBox2.Text.Trim().Replace("'", "''");
            string m = textBox5.Text.Trim().Replace("'", "''");
            string p = textBox3.Text.Trim().Replace("'", "''");
            query =
                "UPDATE Customers SET " +
                $"Username = N'{u}', " +
                $"Email    = N'{e1}', " +
                $"Mobile   = N'{m}', " +
                $"Password = N'{p}' " +
                $"WHERE CustomerID = {Session.UserId}";

            fn.setData(query, "Profile updated successfully.");
            Session.Username = textBox1.Text.Trim();
            label7.Text = Session.Username;
            LoadCustomer();
    }
        private void button2_Click(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        private void pnlWhite_Click(object sender, EventArgs e)
        {
            LoadCustomer();
        }
    }
}
