using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            {textBox6.Text = "0"; }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            clearAll();}
        Int64 totalQuantity;
       
private void button2_Click(object sender, EventArgs e)
    {
        string mname = textBox2.Text;
        string mnumber = textBox3.Text;
        string mdate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
        string edate = dateTimePicker2.Value.ToString("yyyy-MM-dd");

        long quantity;
        if (!long.TryParse(textBox4.Text, out quantity))
        {
            MessageBox.Show("Quantity must be numeric only.");
            return;
        }

        long addQuantity;
        if (!long.TryParse(textBox6.Text, out addQuantity))
        {
            MessageBox.Show("Added quantity must be numeric only.");
            return;
        }

        // هنا التعديل المهم
        decimal unitprice;
        if (!decimal.TryParse(textBox5.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out unitprice))
        {
            MessageBox.Show("Unit price must be numeric only (example: 2.10).");
            return;
        }

        totalQuantity = quantity + addQuantity;

        query = "update medic set " +
                "mname='" + mname + "'," +
                "mnumber='" + mnumber + "'," +
                "mDate='" + mdate + "'," +
                "eDate='" + edate + "'," +
                "quantity=" + totalQuantity + "," +
                "perUnit=" + unitprice.ToString(CultureInfo.InvariantCulture) +
                " where mid='" + textBox1.Text + "'";

        fn.setData(query, "Medicine Details Updated.");
    }
}
}
