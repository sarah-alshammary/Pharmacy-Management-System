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
    public partial class UC_P_AddMedicine : UserControl
    {
        function fn = new function();
        String query;
        public UC_P_AddMedicine()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!=""&& textBox2.Text!=""&&textBox3.Text!=""&& textBox4.Text!="" && textBox5.Text!="")
            {
                String mid = textBox1.Text;
                String mname = textBox2.Text;
                String mnumber = textBox3.Text;
                String mdate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                String edate = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                Int64 quantity = Int64.Parse(textBox4.Text);
                Int64 perunit = Int64.Parse(textBox5.Text);

                query = "insert into medic(mid,mname,mnumber,mDate,eDate,quantity,perUnit) values('"+mid+"','"+mname+"','"+ mnumber + "','"+mdate+"','"+edate+"',"+quantity+","+perunit+")";
                fn.setData(query, "Medicine Added to Database.");
            }
            else
            {
                MessageBox.Show("Enter all Data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            dateTimePicker1.ResetText();
            dateTimePicker2.ResetText();
        }
    }
}
