using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.CustomerUC
{
    public partial class UC_Medicine1 : UserControl
    {
        function fn = new function();
        String query;
        public UC_Medicine1()
        {
            InitializeComponent();
        }

        private void UC_Medicine1_Load(object sender, EventArgs e)
        {
            LoadUserMedicines();

        }
        private void LoadUserMedicines()
        {
      
            query = $@"
        SELECT 
            m.MedName AS [Medicine Name],
            m.Price AS [Price],
            m.[Description] AS [Description],
            cm.TimesPerDay AS [Times/Day],
            cm.UnitsPerDose AS [Units/Dose],
            cm.StartDate AS [Start Date],
            cm.DurationDays AS [Duration (Days)],
            cm.EndDate AS [End Date]
        FROM CustomerMedicines cm
        JOIN Medicines m ON cm.MedicineID = m.MedicineID
        WHERE cm.CustomerID = {Session.UserId}
        ORDER BY cm.StartDate DESC";

            DataSet ds = fn.getData(query);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
