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
    public partial class UC_P_UpdateMedicine : UserControl
    {
        function fn = new function();
        String query;
        public UC_P_UpdateMedicine()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="")
            {
                query = "select * from medic where mid='"+textBox1.Text+"'";
                DataSet ds = fn.getData(query);
                if(ds.Tables[0].Rows.Count!=0)
                {
                    textBox2.Text = ds.Tables[0].Rows[0][2].ToString();
                    textBox3.Text=ds.Tables[0].Rows[0][3].ToString();
                    dateTimePicker1.Text = ds.Tables[0].Rows[0][4].ToString();
                    dateTimePicker2.Text = ds.Tables[0].Rows[0][5].ToString();
                    textBox4.Text = ds.Tables[0].Rows[0][6].ToString();
                    textBox5.Text = ds.Tables[0].Rows[0][7].ToString();
                }
                else
                {
                    MessageBox.Show("No Medicine with ID:"+textBox1.Text+"exist.","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
            else
            {
                clearAll();
            }
        }

        private void clearAll()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            dateTimePicker1.ResetText();
            dateTimePicker2.ResetText();
            textBox4.Clear();
            textBox5.Clear();
            if(textBox6.Text!="0")
            {
                textBox6.Text = "0";
            }
            else
            {
                textBox6.Text = "0";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clearAll();
        }
        Int64 totalQuantity;
        private void button2_Click(object sender, EventArgs e)
        {
            String mname = textBox2.Text;
            String mnumber = textBox3.Text;
            String mdate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            String edate = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            Int64 quantity = Int64.Parse(textBox4.Text);
            Int64 addQuantity = Int64.Parse(textBox6.Text);
            Int64 unitprice = Int64.Parse(textBox5.Text);


            totalQuantity = quantity + addQuantity;

            query = "update medic set mname='"+mname+"',mnumber='"+mnumber+"',mDate='"+mdate+"',eDate='"+edate+"',quantity="+totalQuantity+",perUnit="+unitprice+"  where mid='"+textBox1.Text+"'";
            fn.setData(query, "Medicine Details Updated.");

        }
    }
}
