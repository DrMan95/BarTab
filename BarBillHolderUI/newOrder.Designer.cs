namespace BarBillHolderUI
{
    partial class newOrder
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
            this.categoriesLabel = new System.Windows.Forms.Label();
            this.categoriesTabControl = new System.Windows.Forms.TabControl();
            this.newOrderGroupBox = new System.Windows.Forms.GroupBox();
            this.confirmOrderButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // categoriesLabel
            // 
            this.categoriesLabel.AutoSize = true;
            this.categoriesLabel.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.categoriesLabel.Location = new System.Drawing.Point(323, 30);
            this.categoriesLabel.Name = "categoriesLabel";
            this.categoriesLabel.Size = new System.Drawing.Size(155, 37);
            this.categoriesLabel.TabIndex = 0;
            this.categoriesLabel.Text = "Categories";
            this.categoriesLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // categoriesTabControl
            // 
            this.categoriesTabControl.Location = new System.Drawing.Point(156, 70);
            this.categoriesTabControl.Name = "categoriesTabControl";
            this.categoriesTabControl.SelectedIndex = 0;
            this.categoriesTabControl.Size = new System.Drawing.Size(632, 368);
            this.categoriesTabControl.TabIndex = 1;
            // 
            // newOrderGroupBox
            // 
            this.newOrderGroupBox.Location = new System.Drawing.Point(12, 70);
            this.newOrderGroupBox.Name = "newOrderGroupBox";
            this.newOrderGroupBox.Size = new System.Drawing.Size(138, 364);
            this.newOrderGroupBox.TabIndex = 2;
            this.newOrderGroupBox.TabStop = false;
            this.newOrderGroupBox.Text = "New Order";
            // 
            // confirmOrderButton
            // 
            this.confirmOrderButton.BackColor = System.Drawing.Color.Silver;
            this.confirmOrderButton.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.confirmOrderButton.Location = new System.Drawing.Point(671, 445);
            this.confirmOrderButton.Name = "confirmOrderButton";
            this.confirmOrderButton.Size = new System.Drawing.Size(117, 32);
            this.confirmOrderButton.TabIndex = 3;
            this.confirmOrderButton.Text = "Confirm";
            this.confirmOrderButton.UseVisualStyleBackColor = false;
            this.confirmOrderButton.Click += new System.EventHandler(this.ConfirmOrderButton_Click);
            // 
            // backButton
            // 
            this.backButton.BackColor = System.Drawing.Color.DimGray;
            this.backButton.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.backButton.Location = new System.Drawing.Point(548, 445);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(117, 32);
            this.backButton.TabIndex = 4;
            this.backButton.Text = "Back";
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // newOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 489);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.confirmOrderButton);
            this.Controls.Add(this.newOrderGroupBox);
            this.Controls.Add(this.categoriesTabControl);
            this.Controls.Add(this.categoriesLabel);
            this.Name = "newOrder";
            this.Text = "newOrder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label categoriesLabel;
        private TabControl categoriesTabControl;
        private GroupBox newOrderGroupBox;
        private Button confirmOrderButton;
        private Button backButton;
    }
}