using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.AdministratorUC
{
    public partial class UC_AddUser : UserControl
    {
        function fn = new function();
        String query;
        public UC_AddUser()
        {
            InitializeComponent();
        }
        private void btnSignUp_Click(object sender, EventArgs e)
        {
            String role = txtUserRole.Text;
            String name = textName.Text;
            String dob = textDob.Text;
            Int64 mobile = Int64.Parse(textMobileNo.Text);
            String email = textEmailAddress.Text;
            String username = textusername.Text;
            String pass = textPassword.Text;
            try
            {
                query = "INSERT INTO dbo.[users] ([userRole],[name],[dob],[mobile],[email],[username],[pass]) " + "VALUES ('" + role + "','" + name + "','" + dob + "','" + mobile + "','" + email + "','" + username + "','" + pass + "');";
                fn.setData(query, "Sign Up Successful.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            clearAll();
        }
        public void clearAll()
        {
            textName.Clear();
            textDob.ResetText();
            textMobileNo.Clear();
            textEmailAddress.Clear();
            textusername.Clear();
            textPassword.Clear();
            txtUserRole.SelectedIndex = -1;
        }

        private void UC_AddUser_Load(object sender, EventArgs e)
        {

        }
    }
}
