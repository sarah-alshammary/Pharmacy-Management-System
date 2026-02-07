using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Pharmacy
{
    public partial class customer : Form
    {
        function fn = new function();
        string query;
        // File path for "Remember Me" (AppData\Roaming\Pharmacy\remember.txt)
        private static readonly string RememberDir =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pharmacy");
        private static readonly string RememberFile = Path.Combine(RememberDir, "remember.txt");

        public customer()
        {
            InitializeComponent();
        }
        //  Remember Me 
        private void SaveRememberToFile(string email, string password, bool remember)
        {
            try
            {
                if (!remember)
                {
                    ClearRememberFile();
                    return;
                }
                if (!Directory.Exists(RememberDir))
                    Directory.CreateDirectory(RememberDir);
                File.WriteAllLines(RememberFile, new[] { email, password, remember.ToString() });
            }
            catch {  }
        }

        private void LoadRememberFromFile()
        {
            try
            {
                if (!File.Exists(RememberFile)) return;
                var lines = File.ReadAllLines(RememberFile);
                if (lines.Length >= 3 && bool.TryParse(lines[2], out bool rem))
                {
                    textBox1.Text = lines[0];
                    textBox2.Text = lines[1];
                    checkBox1.Checked = rem;
                }
            }
            catch {  }
        }
        private void ClearRememberFile()
        {
            try
            {
                if (File.Exists(RememberFile))
                    File.Delete(RememberFile);
            }
            catch {  }
        }
        private void customer_Load(object sender, EventArgs e)
        {
            LoadRememberFromFile(); 
        }
        // Login button click
        private void button2_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Simple login check against DB
            query =
                $"SELECT TOP 1 CustomerID, Username, Email, Mobile, Password FROM Customers " +
                $"WHERE Email = N'{email.Replace("'", "''")}' AND Password = N'{password.Replace("'", "''")}'";
            DataSet ds = fn.getData(query);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];
                Session.UserId = Convert.ToInt32(row["CustomerID"]);
                Session.Username = row["Username"].ToString();
                // Remember Me using file
                SaveRememberToFile(email, password, checkBox1.Checked);

                CustomerInfo ci = new CustomerInfo();
                ci.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid email or password",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Sign Up
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CustomerRegister cr = new CustomerRegister();
            cr.Show();
            this.Hide();
        }

        // Forgot Password
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 1) Ask for email
            string email = Prompt.Show("Forgot password", "Enter your email:", textBox1.Text);
            if (string.IsNullOrWhiteSpace(email)) return;

            // Check if email exists
            string checkQuery = $"SELECT TOP 1 CustomerID FROM Customers WHERE Email = N'{email.Replace("'", "''")}'";
            DataSet ds = fn.getData(checkQuery);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("Email not found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // 2) Ask for new password and confirm (masked)
            string newPass = Prompt.Show("Reset password", "Enter new password:", "", mask: true);
            if (string.IsNullOrWhiteSpace(newPass)) return;

            string confirm = Prompt.Show("Reset password", "Confirm new password:", "", mask: true);
            if (newPass != confirm)
            {
                MessageBox.Show("Passwords do not match.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 3) Update password in DB (escape single quotes)
            string update =
                $"UPDATE Customers SET Password = N'{newPass.Replace("'", "''")}' " +
                $"WHERE Email = N'{email.Replace("'", "''")}'";
            try
            {
                // Use your function's update method (adjust name if different)
                fn.setData(update, "Password updated successfully.");    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update failed:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

   
    // Small prompt form (no external libraries, no VisualBasic)
    public static class Prompt
    {
        public static string Show(string title, string label, string defaultValue = "", bool mask = false)
        {
            using (var form = new Form())
            using (var lbl = new Label())
            using (var txt = new TextBox())
            using (var btnOk = new Button())
            using (var btnCancel = new Button())
            {
                form.Width = 380;
                form.Height = 160;
                form.Text = title;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.MaximizeBox = false;
                form.MinimizeBox = false;
                lbl.Left = 15; lbl.Top = 15; lbl.AutoSize = true; lbl.Text = label;
                txt.Left = 15; txt.Top = 45; txt.Width = 340; txt.Text = defaultValue;
                if (mask) txt.UseSystemPasswordChar = true;
                btnOk.Text = "OK"; btnOk.Left = 200; btnOk.Top = 80; btnOk.Width = 75;
                btnCancel.Text = "Cancel"; btnCancel.Left = 280; btnCancel.Top = 80; btnCancel.Width = 75;
                btnOk.DialogResult = DialogResult.OK;
                btnCancel.DialogResult = DialogResult.Cancel;
                form.Controls.Add(lbl);
                form.Controls.Add(txt);
                form.Controls.Add(btnOk);
                form.Controls.Add(btnCancel);
                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;
                var result = form.ShowDialog();
                return result == DialogResult.OK ? txt.Text : null;
            }
        }
    }
}
