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
    public partial class Administrator : Form
    {
        String user = "";
        public Administrator()
        {
            InitializeComponent();
        }
        public string ID
        {
            get { return user.ToString(); }
        }
        public Administrator(String username)
        {
            InitializeComponent();
            userNameLabel.Text = username;
            user = username;
            uC_ViewUser1.ID = ID;
            uC_Profile1.ID = ID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uC_Dashbord1.Visible = true;
            uC_AddUser1.Visible = false;
            uC_ViewUser1.Visible = false;
            uC_Profile1.Visible = false;
            uC_Dashbord1.BringToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            uC_AddUser1.Visible = true;
            uC_Dashbord1.Visible = false;
            uC_ViewUser1.Visible = false;
            uC_Profile1.Visible = false;
            uC_Dashbord1.BringToFront();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            uC_ViewUser1.Visible = true;
            uC_AddUser1.Visible = false;
            uC_Dashbord1.Visible = false;
            uC_Profile1.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            uC_Profile1.Visible = true;
            uC_AddUser1.Visible = false;
            uC_Dashbord1.Visible = false;
            uC_ViewUser1.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();
        }

        private void Administrator_Load(object sender, EventArgs e)
        {
            uC_Dashbord1.Visible = false;
            uC_AddUser1.Visible = false;
            uC_ViewUser1.Visible = false;
            uC_Profile1.Visible = false;
        }
    }
}
