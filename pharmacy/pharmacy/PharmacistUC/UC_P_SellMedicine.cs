using DGVPrinterHelper;
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
    public partial class UC_P_SellMedicine : UserControl
    {
        function fn = new function();
        String query;
        DataSet ds;
        public UC_P_SellMedicine()
        {
            InitializeComponent();
        }

        private void UC_P_SellMedicine_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            query = "select mname from medic where eDate >=getDate() and quantity >'0'";
            ds=fn.getData(query);

            for(int i=0;i<ds.Tables[0].Rows.Count;i++)
            {
                listBox1.Items.Add(ds.Tables[0].Rows[i][0].ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UC_P_SellMedicine_Load(this, null);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            query = "select mname from medic where mname like '"+textBox1.Text+"%'and eDate >=getdate() and quantity >'0'";
            ds = fn.getData(query);

            for(int i=0;i<ds.Tables[0].Rows.Count;i++)
            {
                listBox1.Items.Add(ds.Tables[0].Rows[i][0].ToString());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox4.Clear();
            String name = listBox1.GetItemText(listBox1.SelectedItem);

            textBox3.Text = name;
            query = "select mid,eDate,perUnit from medic where mname='"+name+"'";
            ds = fn.getData(query);
            textBox2.Text = ds.Tables[0].Rows[0][0].ToString();
            dateTimePicker1.Text = ds.Tables[0].Rows[0][1].ToString();
            textBox5.Text= ds.Tables[0].Rows[0][2].ToString();

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if(textBox4.Text !="")
            {
                Int64 unitPrice = Int64.Parse(textBox5.Text);
                Int64 noOfUnit = Int64.Parse(textBox4.Text);
                Int64 totalAmount = unitPrice * noOfUnit;
                textBox6.Text = totalAmount.ToString();
            }

            else
            {
                textBox6.Clear();
            }
        }

        protected int n, totalAmount = 0;
        protected Int64 quantity, newQuantity;

        

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox2.Text!="")
            {
                query = " select quantity from medic where mid= '"+textBox2.Text+"'";
                ds = fn.getData(query);

                quantity = Int64.Parse(ds.Tables[0].Rows[0][0].ToString());
                newQuantity = quantity - Int64.Parse(textBox4.Text);

                if(newQuantity>=0)
                {
                    n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[0].Value = textBox2.Text;
                    dataGridView1.Rows[n].Cells[1].Value = textBox3.Text;
                    dataGridView1.Rows[n].Cells[2].Value = dateTimePicker1.Text;
                    dataGridView1.Rows[n].Cells[3].Value = textBox5.Text;
                    dataGridView1.Rows[n].Cells[4].Value = textBox4.Text;
                    dataGridView1.Rows[n].Cells[5].Value = textBox6.Text;

                    totalAmount = totalAmount + int.Parse(textBox6.Text);
                    label9.Text = "JD. " + totalAmount.ToString();

                    query = "update medic set quantity='" + newQuantity + "' where mid ='" +textBox2.Text+ "'";
                    fn.setData(query, "Medicine Added.");


                }
                else
                {
                    MessageBox.Show("Medicine is Out of Stock.\n Only " + quantity + "Left", "Warning !!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                clearAll();
                UC_P_SellMedicine_Load(this, null);
            }
            else
            {
                MessageBox.Show("Select Medicine First.", "Information !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

           
            
        }
        int valueAmount;
        String valueId;
        protected Int64 noOfunit;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                valueAmount = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                valueId = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                noOfunit = Int64.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
            }catch(Exception)
            { }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DGVPrinter print = new DGVPrinter();
            print.Title = "Medicine Bill";
            print.SubTitle = String.Format("Date:- {0}", DateTime.Now.Date);
            print.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            print.PageNumbers = true;
            print.PageNumberInHeader = false;
            print.PorportionalColumns = true;
            print.HeaderCellAlignment = StringAlignment.Near;
            print.Footer = "Total Payable Amount :" + label9.Text;
            print.FooterSpacing = 15;
            print.PrintDataGridView(dataGridView1);

            totalAmount = 0;
            label9.Text = "JD.00";
            dataGridView1.DataSource = 0;
        }
        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    valueId = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                    int index = dataGridView1.SelectedRows[0].Index;
                    dataGridView1.Rows.RemoveAt(index);

                    string query = "select quantity from medic where mid = '" + valueId + "'";
                    DataSet ds = fn.getData(query);
                    long quantity = Int64.Parse(ds.Tables[0].Rows[0][0].ToString());

                    long newQuantity = quantity + noOfunit;
                    query = "update medic set quantity = " + newQuantity + " where mid = '" + valueId + "'";
                    fn.setData(query, "Medicine Removed From Cart.");

                    totalAmount = totalAmount - valueAmount;
                    if (totalAmount < 0) totalAmount = 0;

                    label9.Text = "JD. " + totalAmount.ToString();

                    UC_P_SellMedicine_Load(this, null);
                }
                else
                {
                    MessageBox.Show("Please select a row to remove.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }























            //if (valueId != null)
            //{
            //    try
            //    {
            //        dataGridView1.Rows.RemoveAt(this.dataGridView1.SelectedRows[0].Index);
            //    }
            //    catch
            //    {

            //    }
            //    finally
            //    {
            //        query = "select quantity from medic where mid ='" + valueId + "'";
            //        ds = fn.getData(query);
            //        quantity = Int64.Parse(ds.Tables[0].Rows[0][0].ToString());
            //        newQuantity = quantity + noOfunit;

            //        query = "update medic set quantity ='" + newQuantity + "'where mid ='" + valueId + "'";
            //        fn.setData(query, "Medicine Remove from Cart.");
            //        totalAmount = totalAmount - valueAmount;
            //        label9.Text = "$." + totalAmount.ToString();
            //    }
            //    UC_P_SellMedicine_Load(this, null);
            //}
        }


        private void clearAll()
        {
            textBox2.Clear();
            textBox3.Clear();
            dateTimePicker1.ResetText();
            textBox5.Clear();
            textBox4.Clear();
            textBox6.Clear();

        }
    }
}
