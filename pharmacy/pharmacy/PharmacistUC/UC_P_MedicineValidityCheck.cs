using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.PharmacistUC
{
    public partial class UC_P_MedicineValidityCheck : UserControl
    {
        function fn = new function();
        String query;
        public UC_P_MedicineValidityCheck()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex==0)
            {
                query = "select *from medic where eDate >= getdate()";
                
                setDataGridView(query, "Valid Medicines.", Color.Black);
            }
            else if(comboBox1.SelectedIndex==1)
            {
                query = "select *from medic where eDate <= getdate()";

                setDataGridView(query, "Expired Medicines.", Color.Red);
            }
            else if(comboBox1.SelectedIndex == 2)
            {
                query = "select *from medic";
                setDataGridView(query, "", Color.Black);
            }
        }

        private void setDataGridView(String query,String labelName,Color col)
        {
            DataSet ds = fn.getData(query);
            dataGridView1.DataSource = ds.Tables[0];
            label3.Text = labelName;
            label3.ForeColor = col;
        }

        private void UC_P_MedicineValidityCheck_Load(object sender, EventArgs e)
        {
            label3.Text = "";
        }
    }
}
