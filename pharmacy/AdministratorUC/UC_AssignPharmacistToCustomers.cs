using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Pharmacy.AdministratorUC
{
    public partial class UC_AssignPharmacistToCustomers : UserControl
    {
        private readonly function fn = new function();

        public UC_AssignPharmacistToCustomers()
        {
            InitializeComponent();

            
            this.Load += UC_AssignPharmacistToCustomers_Load;
        }

        private void UC_AssignPharmacistToCustomers_Load(object sender, EventArgs e)
        {
           
            if (this.DesignMode) return;

            
            lbPharmacistsAll.SelectionMode = SelectionMode.One;
            lbCustomersAll.SelectionMode = SelectionMode.MultiExtended;
            lbCustomersOfPharmacist.SelectionMode = SelectionMode.MultiExtended;

            
            LoadAll();
        }

       

        private void LoadAll()
        {
            LoadPharmacistsAll();
            LoadCustomersAll();
            LoadCustomersOfSelectedPharmacist();
        }

        private void LoadPharmacistsAll()
        {
            string q = @"
                SELECT 
                    u.id,
                    u.username,
                    COUNT(c.CustomerID) AS PatientsCount,
                    u.username + ' — ' +
                    CASE WHEN COUNT(c.CustomerID) > 0 THEN 'has customers' ELSE 'no customers' END +
                    ' (' + CAST(COUNT(c.CustomerID) AS varchar(10)) + ')' AS DisplayText
                FROM dbo.[users] u
                LEFT JOIN dbo.Customers c ON c.PharmacistID = u.id
                WHERE u.userRole = 'Pharmacist'
                GROUP BY u.id, u.username
                ORDER BY u.username;";

            DataSet ds = fn.getData(q);
            lbPharmacistsAll.DataSource = ds.Tables[0];
            lbPharmacistsAll.DisplayMember = "DisplayText";
            lbPharmacistsAll.ValueMember = "id";
        }

        private void LoadCustomersAll()
        {
            string q = @"
                SELECT 
                    c.CustomerID AS Id,
                    c.Username,
                    c.PharmacistID,
                    ISNULL(u.username,'') AS PharmacistUsername,
                    c.Username + 
                      CASE WHEN c.PharmacistID IS NULL 
                           THEN ' — unassigned' 
                           ELSE ' — assigned to ' + u.username END AS DisplayText
                FROM dbo.Customers c
                LEFT JOIN dbo.[users] u ON u.id = c.PharmacistID
                ORDER BY c.Username;";

            DataSet ds = fn.getData(q);
            lbCustomersAll.DataSource = ds.Tables[0];
            lbCustomersAll.DisplayMember = "DisplayText";
            lbCustomersAll.ValueMember = "Id";
        }

        private void LoadCustomersOfSelectedPharmacist()
        {
            lbCustomersOfPharmacist.DataSource = null;

            int? pid = GetSelectedPharmacistId();
            if (pid == null) return;

            string q = $@"
                SELECT 
                    c.CustomerID AS Id,
                    c.Username,
                    c.PharmacistID,
                    c.Username + ' — assigned to ' + 
                    (SELECT TOP 1 u.username FROM dbo.[users] u WHERE u.id = {pid.Value}) AS DisplayText
                FROM dbo.Customers c
                WHERE c.PharmacistID = {pid.Value}
                ORDER BY c.Username;";

            DataSet ds = fn.getData(q);
            lbCustomersOfPharmacist.DataSource = ds.Tables[0];
            lbCustomersOfPharmacist.DisplayMember = "DisplayText";
            lbCustomersOfPharmacist.ValueMember = "Id";
        }


        private int? GetSelectedPharmacistId()
        {
            if (lbPharmacistsAll.SelectedItem is DataRowView drv)
                return Convert.ToInt32(drv["id"]);

            if (lbPharmacistsAll.SelectedValue != null &&
                int.TryParse(lbPharmacistsAll.SelectedValue.ToString(), out int id))
                return id;

            return null;
        }

        private List<int> GetSelectedCustomerIds(ListBox lb)
        {
            var ids = new List<int>();

            foreach (var item in lb.SelectedItems)
            {
                if (item is DataRowView drv)
                {
                    ids.Add(Convert.ToInt32(drv["Id"]));
                }
                else
                {
                    if (int.TryParse(lb.SelectedValue?.ToString(), out int id))
                        ids.Add(id);
                }
            }

            return ids.Distinct().ToList();
        }


        private void btnAssignToSelectedPharmacist_Click(object sender, EventArgs e)
        {
            AssignSelectedCustomersToSelectedPharmacist();
        }

        private void btnUnassignFromSelectedPharmacist_Click(object sender, EventArgs e)
        {
            UnassignSelectedCustomersFromSelectedPharmacist();
        }

        private void btnMoveSelectedToPharmacist_Click(object sender, EventArgs e)
        {
            MoveSelectedCustomersToSelectedPharmacist();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAll();
        }

        private void AssignSelectedCustomersToSelectedPharmacist()
        {
            int? pid = GetSelectedPharmacistId();
            if (pid == null)
            {
                MessageBox.Show("Select a pharmacist first.");
                return;
            }

            var selected = GetSelectedCustomerIds(lbCustomersAll);
            if (selected.Count == 0)
            {
                MessageBox.Show("Select one or more customers from the All Customers list.");
                return;
            }

            foreach (var cid in selected)
            {
                fn.setData(
                    $"UPDATE dbo.Customers SET PharmacistID = {pid.Value} WHERE CustomerID = {cid};",
                    "Customer assigned successfully."
                );
            }

            LoadAll();
        }

        private void UnassignSelectedCustomersFromSelectedPharmacist()
        {
            var selected = GetSelectedCustomerIds(lbCustomersOfPharmacist);
            if (selected.Count == 0)
            {
                MessageBox.Show("Select one or more customers from the pharmacist’s customers list.");
                return;
            }

            foreach (var cid in selected)
            {
                fn.setData(
                    $"UPDATE dbo.Customers SET PharmacistID = NULL WHERE CustomerID = {cid};",
                    "Customer unassigned successfully."
                );
            }

            LoadAll();
        }

        private void MoveSelectedCustomersToSelectedPharmacist()
        {
            int? pid = GetSelectedPharmacistId();
            if (pid == null)
            {
                MessageBox.Show("Select a pharmacist first.");
                return;
            }

            var selected = GetSelectedCustomerIds(lbCustomersAll);
            selected.AddRange(GetSelectedCustomerIds(lbCustomersOfPharmacist));
            selected = selected.Distinct().ToList();

            if (selected.Count == 0)
            {
                MessageBox.Show("Select one or more customers to move.");
                return;
            }

            foreach (var cid in selected)
            {
                fn.setData(
                    $"UPDATE dbo.Customers SET PharmacistID = {pid.Value} WHERE CustomerID = {cid};",
                    "Customer moved successfully."
                );
            }

            LoadAll();
        }


        private void lbPharmacistsAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            LoadCustomersOfSelectedPharmacist();
        }

        private void lbCustomersAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional
        }

        private void lbCustomersOfPharmacist_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Optional
        }
    }
}