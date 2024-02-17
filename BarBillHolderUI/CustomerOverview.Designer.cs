namespace BarBillHolderUI
{
    partial class CustomerOverview
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
            this.customerOverviewGroupBox = new System.Windows.Forms.GroupBox();
            this.moveButton = new System.Windows.Forms.Button();
            this.totalBill = new System.Windows.Forms.TextBox();
            this.backButton = new System.Windows.Forms.Button();
            this.payButton = new System.Windows.Forms.Button();
            this.newOrder = new System.Windows.Forms.Button();
            this.itemsPanel = new System.Windows.Forms.Panel();
            this.customerOverviewGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // customerOverviewGroupBox
            // 
            this.customerOverviewGroupBox.Controls.Add(this.moveButton);
            this.customerOverviewGroupBox.Controls.Add(this.totalBill);
            this.customerOverviewGroupBox.Controls.Add(this.backButton);
            this.customerOverviewGroupBox.Controls.Add(this.payButton);
            this.customerOverviewGroupBox.Controls.Add(this.newOrder);
            this.customerOverviewGroupBox.Location = new System.Drawing.Point(12, 12);
            this.customerOverviewGroupBox.Name = "customerOverviewGroupBox";
            this.customerOverviewGroupBox.Size = new System.Drawing.Size(150, 296);
            this.customerOverviewGroupBox.TabIndex = 0;
            this.customerOverviewGroupBox.TabStop = false;
            this.customerOverviewGroupBox.Text = "<ID>";
            // 
            // moveButton
            // 
            this.moveButton.BackColor = System.Drawing.Color.Silver;
            this.moveButton.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.moveButton.Location = new System.Drawing.Point(15, 91);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(119, 33);
            this.moveButton.TabIndex = 5;
            this.moveButton.Text = "Move";
            this.moveButton.UseVisualStyleBackColor = false;
            this.moveButton.Click += new System.EventHandler(this.MoveButton_Click);
            // 
            // totalBill
            // 
            this.totalBill.Location = new System.Drawing.Point(15, 148);
            this.totalBill.Name = "totalBill";
            this.totalBill.Size = new System.Drawing.Size(119, 23);
            this.totalBill.TabIndex = 4;
            this.totalBill.Text = "0.0€";
            this.totalBill.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // backButton
            // 
            this.backButton.BackColor = System.Drawing.Color.Silver;
            this.backButton.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.backButton.Location = new System.Drawing.Point(15, 242);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(119, 33);
            this.backButton.TabIndex = 3;
            this.backButton.Text = "Back";
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // payButton
            // 
            this.payButton.BackColor = System.Drawing.Color.Silver;
            this.payButton.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.payButton.Location = new System.Drawing.Point(15, 194);
            this.payButton.Name = "payButton";
            this.payButton.Size = new System.Drawing.Size(119, 33);
            this.payButton.TabIndex = 1;
            this.payButton.Text = "Pay";
            this.payButton.UseVisualStyleBackColor = false;
            this.payButton.Click += new System.EventHandler(this.PayButton_Click);
            // 
            // newOrder
            // 
            this.newOrder.BackColor = System.Drawing.Color.Silver;
            this.newOrder.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.newOrder.Location = new System.Drawing.Point(15, 35);
            this.newOrder.Name = "newOrder";
            this.newOrder.Size = new System.Drawing.Size(119, 33);
            this.newOrder.TabIndex = 0;
            this.newOrder.Text = "New Order";
            this.newOrder.UseVisualStyleBackColor = false;
            this.newOrder.Click += new System.EventHandler(this.NewOrder_Click);
            // 
            // itemsPanel
            // 
            this.itemsPanel.Location = new System.Drawing.Point(168, 12);
            this.itemsPanel.Name = "itemsPanel";
            this.itemsPanel.Size = new System.Drawing.Size(184, 296);
            this.itemsPanel.TabIndex = 1;
            // 
            // CustomerOverview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(373, 327);
            this.Controls.Add(this.itemsPanel);
            this.Controls.Add(this.customerOverviewGroupBox);
            this.Name = "CustomerOverview";
            this.Text = "Customer Overview";
            this.customerOverviewGroupBox.ResumeLayout(false);
            this.customerOverviewGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox customerOverviewGroupBox;
        private Button newOrder;
        private Button backButton;
        private Button payButton;
        private TextBox totalBill;
        private Button moveButton;
        private Panel itemsPanel;
    }
}