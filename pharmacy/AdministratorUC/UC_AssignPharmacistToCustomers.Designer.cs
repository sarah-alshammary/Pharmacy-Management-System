
namespace Pharmacy.AdministratorUC
{
    partial class UC_AssignPharmacistToCustomers
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbPharmacistsAll = new System.Windows.Forms.ListBox();
            this.lbCustomersAll = new System.Windows.Forms.ListBox();
            this.lbCustomersOfPharmacist = new System.Windows.Forms.ListBox();
            this.btnAssignToSelectedPharmacist = new System.Windows.Forms.Button();
            this.btnUnassignFromSelectedPharmacist = new System.Windows.Forms.Button();
            this.btnMoveSelectedToPharmacist = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbPharmacistsAll
            // 
            this.lbPharmacistsAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPharmacistsAll.FormattingEnabled = true;
            this.lbPharmacistsAll.ItemHeight = 46;
            this.lbPharmacistsAll.Location = new System.Drawing.Point(793, 302);
            this.lbPharmacistsAll.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.lbPharmacistsAll.Name = "lbPharmacistsAll";
            this.lbPharmacistsAll.Size = new System.Drawing.Size(840, 280);
            this.lbPharmacistsAll.TabIndex = 0;
            this.lbPharmacistsAll.SelectedIndexChanged += new System.EventHandler(this.lbPharmacistsAll_SelectedIndexChanged);
            // 
            // lbCustomersAll
            // 
            this.lbCustomersAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCustomersAll.FormattingEnabled = true;
            this.lbCustomersAll.ItemHeight = 46;
            this.lbCustomersAll.Location = new System.Drawing.Point(793, 646);
            this.lbCustomersAll.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.lbCustomersAll.Name = "lbCustomersAll";
            this.lbCustomersAll.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbCustomersAll.Size = new System.Drawing.Size(840, 280);
            this.lbCustomersAll.TabIndex = 1;
            this.lbCustomersAll.SelectedIndexChanged += new System.EventHandler(this.lbCustomersAll_SelectedIndexChanged);
            // 
            // lbCustomersOfPharmacist
            // 
            this.lbCustomersOfPharmacist.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCustomersOfPharmacist.FormattingEnabled = true;
            this.lbCustomersOfPharmacist.ItemHeight = 46;
            this.lbCustomersOfPharmacist.Location = new System.Drawing.Point(793, 996);
            this.lbCustomersOfPharmacist.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.lbCustomersOfPharmacist.Name = "lbCustomersOfPharmacist";
            this.lbCustomersOfPharmacist.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbCustomersOfPharmacist.Size = new System.Drawing.Size(840, 280);
            this.lbCustomersOfPharmacist.TabIndex = 2;
            this.lbCustomersOfPharmacist.SelectedIndexChanged += new System.EventHandler(this.lbCustomersOfPharmacist_SelectedIndexChanged);
            // 
            // btnAssignToSelectedPharmacist
            // 
            this.btnAssignToSelectedPharmacist.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAssignToSelectedPharmacist.Location = new System.Drawing.Point(1986, 495);
            this.btnAssignToSelectedPharmacist.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnAssignToSelectedPharmacist.Name = "btnAssignToSelectedPharmacist";
            this.btnAssignToSelectedPharmacist.Size = new System.Drawing.Size(210, 96);
            this.btnAssignToSelectedPharmacist.TabIndex = 3;
            this.btnAssignToSelectedPharmacist.Text = "Assign";
            this.btnAssignToSelectedPharmacist.UseVisualStyleBackColor = true;
            this.btnAssignToSelectedPharmacist.Click += new System.EventHandler(this.btnAssignToSelectedPharmacist_Click);
            // 
            // btnUnassignFromSelectedPharmacist
            // 
            this.btnUnassignFromSelectedPharmacist.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUnassignFromSelectedPharmacist.Location = new System.Drawing.Point(1986, 630);
            this.btnUnassignFromSelectedPharmacist.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnUnassignFromSelectedPharmacist.Name = "btnUnassignFromSelectedPharmacist";
            this.btnUnassignFromSelectedPharmacist.Size = new System.Drawing.Size(210, 96);
            this.btnUnassignFromSelectedPharmacist.TabIndex = 4;
            this.btnUnassignFromSelectedPharmacist.Text = "Unassign";
            this.btnUnassignFromSelectedPharmacist.UseVisualStyleBackColor = true;
            this.btnUnassignFromSelectedPharmacist.Click += new System.EventHandler(this.btnUnassignFromSelectedPharmacist_Click);
            // 
            // btnMoveSelectedToPharmacist
            // 
            this.btnMoveSelectedToPharmacist.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveSelectedToPharmacist.Location = new System.Drawing.Point(1986, 782);
            this.btnMoveSelectedToPharmacist.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnMoveSelectedToPharmacist.Name = "btnMoveSelectedToPharmacist";
            this.btnMoveSelectedToPharmacist.Size = new System.Drawing.Size(210, 96);
            this.btnMoveSelectedToPharmacist.TabIndex = 5;
            this.btnMoveSelectedToPharmacist.Text = "Move";
            this.btnMoveSelectedToPharmacist.UseVisualStyleBackColor = true;
            this.btnMoveSelectedToPharmacist.Click += new System.EventHandler(this.btnMoveSelectedToPharmacist_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(1986, 945);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(210, 96);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(41, 66);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(748, 54);
            this.label1.TabIndex = 7;
            this.label1.Text = "Assign Pharmacist To Customers";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // UC_AssignPharmacistToCustomers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnMoveSelectedToPharmacist);
            this.Controls.Add(this.btnUnassignFromSelectedPharmacist);
            this.Controls.Add(this.btnAssignToSelectedPharmacist);
            this.Controls.Add(this.lbCustomersOfPharmacist);
            this.Controls.Add(this.lbCustomersAll);
            this.Controls.Add(this.lbPharmacistsAll);
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "UC_AssignPharmacistToCustomers";
            this.Size = new System.Drawing.Size(3460, 1714);
            this.Load += new System.EventHandler(this.UC_AssignPharmacistToCustomers_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbPharmacistsAll;
        private System.Windows.Forms.ListBox lbCustomersAll;
        private System.Windows.Forms.ListBox lbCustomersOfPharmacist;
        private System.Windows.Forms.Button btnAssignToSelectedPharmacist;
        private System.Windows.Forms.Button btnUnassignFromSelectedPharmacist;
        private System.Windows.Forms.Button btnMoveSelectedToPharmacist;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label label1;
    }
}
