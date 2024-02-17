namespace BarBillHolderUI
{
    partial class BarOverview
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.barOverviewSplitContainer = new System.Windows.Forms.SplitContainer();
            this.customersGroupBox = new System.Windows.Forms.GroupBox();
            this.customersPanel = new System.Windows.Forms.Panel();
            this.newCustomerButton = new System.Windows.Forms.Button();
            this.registerButton = new System.Windows.Forms.Button();
            this.tableStand = new System.Windows.Forms.Button();
            this.tableWindow = new System.Windows.Forms.Button();
            this.table11 = new System.Windows.Forms.Button();
            this.table12 = new System.Windows.Forms.Button();
            this.table10 = new System.Windows.Forms.Button();
            this.table9 = new System.Windows.Forms.Button();
            this.table8 = new System.Windows.Forms.Button();
            this.table5 = new System.Windows.Forms.Button();
            this.table6 = new System.Windows.Forms.Button();
            this.table7 = new System.Windows.Forms.Button();
            this.table3 = new System.Windows.Forms.Button();
            this.table2 = new System.Windows.Forms.Button();
            this.table1 = new System.Windows.Forms.Button();
            this.table4 = new System.Windows.Forms.Button();
            this.customerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.customerBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barOverviewSplitContainer)).BeginInit();
            this.barOverviewSplitContainer.Panel1.SuspendLayout();
            this.barOverviewSplitContainer.Panel2.SuspendLayout();
            this.barOverviewSplitContainer.SuspendLayout();
            this.customersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // barOverviewSplitContainer
            // 
            this.barOverviewSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barOverviewSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.barOverviewSplitContainer.Name = "barOverviewSplitContainer";
            // 
            // barOverviewSplitContainer.Panel1
            // 
            this.barOverviewSplitContainer.Panel1.Controls.Add(this.customersGroupBox);
            this.barOverviewSplitContainer.Panel1.Controls.Add(this.newCustomerButton);
            // 
            // barOverviewSplitContainer.Panel2
            // 
            this.barOverviewSplitContainer.Panel2.Controls.Add(this.registerButton);
            this.barOverviewSplitContainer.Panel2.Controls.Add(this.tableStand);
            this.barOverviewSplitContainer.Panel2.Controls.Add(this.tableWindow);
            this.barOverviewSplitContainer.Panel2.Controls.Add(this.table11);
            this.barOverviewSplitContainer.Panel2.Controls.Add(this.table12);
            this.barOverviewSplitContainer.Panel2.Controls.Add(this.table10);
            this.barOverviewSplitContainer.Panel2.Controls.Add(this.table9);
            this.barOverviewSplitContainer.Panel2.Controls.Add(this.table8);
            this.barOverviewSplitContainer.Panel2.Controls.Add(this.table5);
            this.barOverviewSplitContainer.Panel2.Controls.Add(this.table6);
            this.barOverviewSplitContainer.Panel2.Controls.Add(this.table7);
            this.barOverviewSplitContainer.Panel2.Controls.Add(this.table3);
            this.barOverviewSplitContainer.Panel2.Controls.Add(this.table2);
            this.barOverviewSplitContainer.Panel2.Controls.Add(this.table1);
            this.barOverviewSplitContainer.Panel2.Controls.Add(this.table4);
            this.barOverviewSplitContainer.Size = new System.Drawing.Size(800, 510);
            this.barOverviewSplitContainer.SplitterDistance = 266;
            this.barOverviewSplitContainer.TabIndex = 0;
            // 
            // customersGroupBox
            // 
            this.customersGroupBox.Controls.Add(this.customersPanel);
            this.customersGroupBox.Location = new System.Drawing.Point(12, 95);
            this.customersGroupBox.Name = "customersGroupBox";
            this.customersGroupBox.Size = new System.Drawing.Size(242, 403);
            this.customersGroupBox.TabIndex = 1;
            this.customersGroupBox.TabStop = false;
            this.customersGroupBox.Text = "Customers";
            // 
            // customersPanel
            // 
            this.customersPanel.Location = new System.Drawing.Point(6, 24);
            this.customersPanel.Name = "customersPanel";
            this.customersPanel.Size = new System.Drawing.Size(230, 373);
            this.customersPanel.TabIndex = 0;
            // 
            // newCustomerButton
            // 
            this.newCustomerButton.BackColor = System.Drawing.Color.Silver;
            this.newCustomerButton.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.newCustomerButton.Location = new System.Drawing.Point(25, 26);
            this.newCustomerButton.Name = "newCustomerButton";
            this.newCustomerButton.Size = new System.Drawing.Size(217, 45);
            this.newCustomerButton.TabIndex = 0;
            this.newCustomerButton.Text = "New Customer";
            this.newCustomerButton.UseVisualStyleBackColor = false;
            this.newCustomerButton.Click += new System.EventHandler(this.NewCustomerButton_Click);
            // 
            // registerButton
            // 
            this.registerButton.BackColor = System.Drawing.Color.Silver;
            this.registerButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.registerButton.Location = new System.Drawing.Point(3, 469);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(86, 36);
            this.registerButton.TabIndex = 14;
            this.registerButton.Text = "Register";
            this.registerButton.UseVisualStyleBackColor = false;
            this.registerButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // tableStand
            // 
            this.tableStand.BackColor = System.Drawing.Color.Green;
            this.tableStand.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tableStand.Location = new System.Drawing.Point(213, 286);
            this.tableStand.Name = "tableStand";
            this.tableStand.Size = new System.Drawing.Size(70, 73);
            this.tableStand.TabIndex = 13;
            this.tableStand.Text = "stand";
            this.tableStand.UseVisualStyleBackColor = false;
            // 
            // tableWindow
            // 
            this.tableWindow.BackColor = System.Drawing.Color.Green;
            this.tableWindow.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tableWindow.Location = new System.Drawing.Point(213, 184);
            this.tableWindow.Name = "tableWindow";
            this.tableWindow.Size = new System.Drawing.Size(70, 75);
            this.tableWindow.TabIndex = 12;
            this.tableWindow.Text = "window";
            this.tableWindow.UseVisualStyleBackColor = false;
            // 
            // table11
            // 
            this.table11.BackColor = System.Drawing.Color.Green;
            this.table11.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.table11.Location = new System.Drawing.Point(427, 456);
            this.table11.Name = "table11";
            this.table11.Size = new System.Drawing.Size(79, 42);
            this.table11.TabIndex = 11;
            this.table11.Text = "11";
            this.table11.UseVisualStyleBackColor = false;
            // 
            // table12
            // 
            this.table12.BackColor = System.Drawing.Color.Green;
            this.table12.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.table12.Location = new System.Drawing.Point(304, 456);
            this.table12.Name = "table12";
            this.table12.Size = new System.Drawing.Size(79, 42);
            this.table12.TabIndex = 10;
            this.table12.Text = "12";
            this.table12.UseVisualStyleBackColor = false;
            // 
            // table10
            // 
            this.table10.BackColor = System.Drawing.Color.Green;
            this.table10.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.table10.Location = new System.Drawing.Point(304, 384);
            this.table10.Name = "table10";
            this.table10.Size = new System.Drawing.Size(79, 42);
            this.table10.TabIndex = 9;
            this.table10.Text = "10";
            this.table10.UseVisualStyleBackColor = false;
            // 
            // table9
            // 
            this.table9.BackColor = System.Drawing.Color.Green;
            this.table9.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.table9.Location = new System.Drawing.Point(427, 384);
            this.table9.Name = "table9";
            this.table9.Size = new System.Drawing.Size(79, 42);
            this.table9.TabIndex = 8;
            this.table9.Text = "9";
            this.table9.UseVisualStyleBackColor = false;
            // 
            // table8
            // 
            this.table8.BackColor = System.Drawing.Color.Green;
            this.table8.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.table8.Location = new System.Drawing.Point(304, 286);
            this.table8.Name = "table8";
            this.table8.Size = new System.Drawing.Size(79, 42);
            this.table8.TabIndex = 7;
            this.table8.Text = "8";
            this.table8.UseVisualStyleBackColor = false;
            // 
            // table5
            // 
            this.table5.BackColor = System.Drawing.Color.Green;
            this.table5.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.table5.Location = new System.Drawing.Point(427, 143);
            this.table5.Name = "table5";
            this.table5.Size = new System.Drawing.Size(79, 42);
            this.table5.TabIndex = 6;
            this.table5.Text = "5";
            this.table5.UseVisualStyleBackColor = false;
            // 
            // table6
            // 
            this.table6.BackColor = System.Drawing.Color.Green;
            this.table6.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.table6.Location = new System.Drawing.Point(427, 236);
            this.table6.Name = "table6";
            this.table6.Size = new System.Drawing.Size(79, 42);
            this.table6.TabIndex = 5;
            this.table6.Text = "6";
            this.table6.UseVisualStyleBackColor = false;
            // 
            // table7
            // 
            this.table7.BackColor = System.Drawing.Color.Green;
            this.table7.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.table7.Location = new System.Drawing.Point(304, 198);
            this.table7.Name = "table7";
            this.table7.Size = new System.Drawing.Size(79, 42);
            this.table7.TabIndex = 4;
            this.table7.Text = "7";
            this.table7.UseVisualStyleBackColor = false;
            // 
            // table3
            // 
            this.table3.BackColor = System.Drawing.Color.Green;
            this.table3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.table3.Location = new System.Drawing.Point(213, 111);
            this.table3.Name = "table3";
            this.table3.Size = new System.Drawing.Size(79, 42);
            this.table3.TabIndex = 3;
            this.table3.Text = "3";
            this.table3.UseVisualStyleBackColor = false;
            // 
            // table2
            // 
            this.table2.BackColor = System.Drawing.Color.Green;
            this.table2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.table2.Location = new System.Drawing.Point(213, 44);
            this.table2.Name = "table2";
            this.table2.Size = new System.Drawing.Size(79, 42);
            this.table2.TabIndex = 2;
            this.table2.Text = "2";
            this.table2.UseVisualStyleBackColor = false;
            // 
            // table1
            // 
            this.table1.BackColor = System.Drawing.Color.Green;
            this.table1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.table1.Location = new System.Drawing.Point(91, 44);
            this.table1.Name = "table1";
            this.table1.Size = new System.Drawing.Size(79, 42);
            this.table1.TabIndex = 1;
            this.table1.Text = "1";
            this.table1.UseVisualStyleBackColor = false;
            // 
            // table4
            // 
            this.table4.BackColor = System.Drawing.Color.Green;
            this.table4.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.table4.Location = new System.Drawing.Point(91, 111);
            this.table4.Name = "table4";
            this.table4.Size = new System.Drawing.Size(79, 42);
            this.table4.TabIndex = 0;
            this.table4.Text = "4";
            this.table4.UseVisualStyleBackColor = false;
            // 
            // customerBindingSource
            // 
            this.customerBindingSource.DataSource = typeof(BarBillHolderLibrary.Customer);
            // 
            // customerBindingSource1
            // 
            this.customerBindingSource1.DataSource = typeof(BarBillHolderLibrary.Customer);
            // 
            // BarOverview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 510);
            this.Controls.Add(this.barOverviewSplitContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "BarOverview";
            this.Text = "Bar Overview";
            this.barOverviewSplitContainer.Panel1.ResumeLayout(false);
            this.barOverviewSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barOverviewSplitContainer)).EndInit();
            this.barOverviewSplitContainer.ResumeLayout(false);
            this.customersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer barOverviewSplitContainer;
        private Button table1;
        private Button table2;
        private Button table3;
        private Button table4;
        private Button table5;
        private Button table6;
        private Button table7;
        private Button table8;
        private Button table9;
        private Button table10;
        private Button table11;
        private Button table12;
        private Button tableStand;
        private Button tableWindow;
        private Button newCustomerButton;
        private GroupBox customersGroupBox;
        private BindingSource customerBindingSource;
        private BindingSource customerBindingSource1;
        private Button registerButton;
        private Panel customersPanel;
    }
}