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

    public partial class Form1 : Form
    {
        function fn = new function();
        

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string uname = textBox1.Text.Trim();
                string pass = textBox2.Text.Trim();
                if (string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(pass))
                {
                    MessageBox.Show("insert user name and password.");
                    return;
                }
                if (uname == "root" && pass == "root")
                {
                    new Administrator().Show();
                    this.Hide();
                    return;
                } 
                uname = uname.Replace("'", "''");
                pass = pass.Replace("'", "''");
                function fn = new function();
                DataSet ds;
                string query = $"SELECT TOP 1 id, userRole, username FROM dbo.users WHERE username = '{uname}' AND pass = '{pass}'";
                ds = fn.getData(query);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Wrong Username OR Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                } 
                var row = ds.Tables[0].Rows[0];
                int userId = Convert.ToInt32(row["id"]);
                string role = row["userRole"].ToString();
                string uName = row["username"].ToString();
                if (role.Equals("Administrator", StringComparison.OrdinalIgnoreCase))
                {
                    var admin = new Administrator(uName);
                    admin.Show();
                    this.Hide();
                }
                else if (role.Equals("Pharmacist", StringComparison.OrdinalIgnoreCase))
                { 
                    Session.UserId = userId;   
                    Session.Username = uName;    

                    var pharm = new Pharmacist();
                    pharm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("user type not supported");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Login Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            customer c = new customer();
            c.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
