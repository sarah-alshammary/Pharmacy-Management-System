using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy
{
    public partial class CustomerInfo : Form
    {
        

        public CustomerInfo()
        {
            InitializeComponent();
            
        }
       

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uC_Medicine11.Visible = true;
            uC_Profile11.Visible = false;
           
        }

        private void CustomerInfo_Load(object sender, EventArgs e)
        {
            uC_Medicine11.Visible = false;
            uC_Profile11.Visible = false;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            uC_Medicine11.Visible = false;
            uC_Profile11.Visible = true;
           
            
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                
                var pythonPath = "python"; 

                
                var scriptPath = @"C:\Users\sarah\Desktop\work\main.py";


                var workingDir = @"C:\Users\sarah\Desktop\work";

                
                var psi = new ProcessStartInfo
                {
                    FileName = pythonPath,
                    Arguments = $"\"{scriptPath}\"",   
                    UseShellExecute = false,
                    RedirectStandardOutput = false,    
                    RedirectStandardError = false,
                    CreateNoWindow = true,             
                    WorkingDirectory = workingDir
                };
                using (var process = new Process { StartInfo = psi })
                {
                    process.Start();
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Python Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnAlerts_Cick(object sender, EventArgs e)
        {
            try
            {
                // Python must be in PATH
                var pythonPath = "python";

                // Path to your alerts GUI script
                var scriptPath = @"C:\Users\sarah\Desktop\reminde\Sarah.pyw"
;

                // Working directory of the script
                var workingDir = @"C:\Users\sarah\Desktop\reminde";

                var psi = new ProcessStartInfo
                {
                    FileName = pythonPath,
                    Arguments = $"\"{scriptPath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = false,   // false so that Python/Tkinter window is visible
                    WorkingDirectory = workingDir
                };
                using (var process = new Process { StartInfo = psi })
                {
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting alerts script: " + ex.Message,
                                "Python Launch Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
    }
}
