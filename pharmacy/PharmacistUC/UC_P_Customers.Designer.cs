
namespace Pharmacy.PharmacistUC
{
    partial class UC_P_Customers
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
            this.combCustomers = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvInventory = new System.Windows.Forms.DataGridView();
            this.numTimesPerDay = new System.Windows.Forms.NumericUpDown();
            this.btnPrescribe = new System.Windows.Forms.Button();
            this.dgvCustomerPrescriptions = new System.Windows.Forms.DataGridView();
            this.numUnitsPerDose = new System.Windows.Forms.NumericUpDown();
            this.numDurationDays = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.reset = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.btnCheckSafety = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimesPerDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerPrescriptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnitsPerDose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDurationDays)).BeginInit();
            this.SuspendLayout();
            // 
            // combCustomers
            // 
            this.combCustomers.FormattingEnabled = true;
            this.combCustomers.Location = new System.Drawing.Point(134, 156);
            this.combCustomers.Name = "combCustomers";
            this.combCustomers.Size = new System.Drawing.Size(173, 43);
            this.combCustomers.TabIndex = 0;
            this.combCustomers.SelectionChangeCommitted += new System.EventHandler(this.combCustomers_SelectionChangeCommitted);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(468, 149);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(243, 54);
            this.txtSearch.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(726, 149);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(160, 30);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvInventory
            // 
            this.dgvInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInventory.Location = new System.Drawing.Point(39, 266);
            this.dgvInventory.Name = "dgvInventory";
            this.dgvInventory.RowHeadersWidth = 62;
            this.dgvInventory.RowTemplate.Height = 28;
            this.dgvInventory.Size = new System.Drawing.Size(403, 373);
            this.dgvInventory.TabIndex = 3;
            // 
            // numTimesPerDay
            // 
            this.numTimesPerDay.Location = new System.Drawing.Point(147, 698);
            this.numTimesPerDay.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numTimesPerDay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTimesPerDay.Name = "numTimesPerDay";
            this.numTimesPerDay.Size = new System.Drawing.Size(84, 41);
            this.numTimesPerDay.TabIndex = 4;
            this.numTimesPerDay.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // btnPrescribe
            // 
            this.btnPrescribe.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrescribe.Location = new System.Drawing.Point(746, 692);
            this.btnPrescribe.Name = "btnPrescribe";
            this.btnPrescribe.Size = new System.Drawing.Size(152, 27);
            this.btnPrescribe.TabIndex = 5;
            this.btnPrescribe.Text = "Prescribe";
            this.btnPrescribe.UseVisualStyleBackColor = true;
            this.btnPrescribe.Click += new System.EventHandler(this.btnPrescribe_Click);
            // 
            // dgvCustomerPrescriptions
            // 
            this.dgvCustomerPrescriptions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomerPrescriptions.Location = new System.Drawing.Point(521, 266);
            this.dgvCustomerPrescriptions.Name = "dgvCustomerPrescriptions";
            this.dgvCustomerPrescriptions.RowHeadersWidth = 62;
            this.dgvCustomerPrescriptions.RowTemplate.Height = 28;
            this.dgvCustomerPrescriptions.Size = new System.Drawing.Size(403, 373);
            this.dgvCustomerPrescriptions.TabIndex = 6;
            this.dgvCustomerPrescriptions.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCustomerPrescriptions_CellContentClick);
            // 
            // numUnitsPerDose
            // 
            this.numUnitsPerDose.DecimalPlaces = 2;
            this.numUnitsPerDose.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.numUnitsPerDose.Location = new System.Drawing.Point(389, 695);
            this.numUnitsPerDose.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.numUnitsPerDose.Name = "numUnitsPerDose";
            this.numUnitsPerDose.Size = new System.Drawing.Size(84, 41);
            this.numUnitsPerDose.TabIndex = 7;
            this.numUnitsPerDose.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numDurationDays
            // 
            this.numDurationDays.Location = new System.Drawing.Point(622, 695);
            this.numDurationDays.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numDurationDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDurationDays.Name = "numDurationDays";
            this.numDurationDays.Size = new System.Drawing.Size(84, 41);
            this.numDurationDays.TabIndex = 8;
            this.numDurationDays.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(750, 68);
            this.label1.TabIndex = 9;
            this.label1.Text = "Customers and Prescriptions";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(37, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 39);
            this.label2.TabIndex = 10;
            this.label2.Text = "Patient";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(383, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 39);
            this.label3.TabIndex = 11;
            this.label3.Text = "Search";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(41, 224);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(223, 39);
            this.label4.TabIndex = 12;
            this.label4.Text = "My Inventory";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(514, 224);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(392, 39);
            this.label5.TabIndex = 13;
            this.label5.Text = "Customer Prescriptions";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(32, 701);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(183, 39);
            this.label6.TabIndex = 14;
            this.label6.Text = "Times/day";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(285, 697);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(188, 39);
            this.label7.TabIndex = 15;
            this.label7.Text = "Units/dose";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(546, 695);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 39);
            this.label8.TabIndex = 16;
            this.label8.Text = "Days";
            // 
            // reset
            // 
            this.reset.BackColor = System.Drawing.Color.White;
            this.reset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reset.Location = new System.Drawing.Point(935, 306);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(121, 47);
            this.reset.TabIndex = 22;
            this.reset.Text = "Reset";
            this.reset.UseVisualStyleBackColor = false;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // delete
            // 
            this.delete.BackColor = System.Drawing.Color.White;
            this.delete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delete.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.delete.Location = new System.Drawing.Point(935, 372);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(121, 52);
            this.delete.TabIndex = 21;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = false;
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // btnCheckSafety
            // 
            this.btnCheckSafety.BackColor = System.Drawing.Color.White;
            this.btnCheckSafety.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckSafety.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnCheckSafety.Location = new System.Drawing.Point(904, 692);
            this.btnCheckSafety.Name = "btnCheckSafety";
            this.btnCheckSafety.Size = new System.Drawing.Size(152, 27);
            this.btnCheckSafety.TabIndex = 23;
            this.btnCheckSafety.Text = "Check Safety";
            this.btnCheckSafety.UseVisualStyleBackColor = false;
            this.btnCheckSafety.Click += new System.EventHandler(this.btnCheckSafety_Click);
            // 
            // UC_P_Customers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 35F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnCheckSafety);
            this.Controls.Add(this.reset);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numDurationDays);
            this.Controls.Add(this.numUnitsPerDose);
            this.Controls.Add(this.dgvCustomerPrescriptions);
            this.Controls.Add(this.btnPrescribe);
            this.Controls.Add(this.numTimesPerDay);
            this.Controls.Add(this.dgvInventory);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.combCustomers);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UC_P_Customers";
            this.Size = new System.Drawing.Size(2182, 1462);
            this.Load += new System.EventHandler(this.UC_P_Customers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimesPerDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerPrescriptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnitsPerDose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDurationDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox combCustomers;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvInventory;
        private System.Windows.Forms.NumericUpDown numTimesPerDay;
        private System.Windows.Forms.Button btnPrescribe;
        private System.Windows.Forms.DataGridView dgvCustomerPrescriptions;
        private System.Windows.Forms.NumericUpDown numUnitsPerDose;
        private System.Windows.Forms.NumericUpDown numDurationDays;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button reset;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Button btnCheckSafety;
    }
}
