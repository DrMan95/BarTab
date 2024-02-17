namespace BarBillHolderUI
{
    partial class ItemForm
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
            this.title = new System.Windows.Forms.Label();
            this.itemNameTextBox = new System.Windows.Forms.TextBox();
            this.itemNameLabel = new System.Windows.Forms.Label();
            this.itemCategoryLabel = new System.Windows.Forms.Label();
            this.itemCategoryTextBox = new System.Windows.Forms.TextBox();
            this.itemPriceLabel = new System.Windows.Forms.Label();
            this.itemPriceTextBox = new System.Windows.Forms.TextBox();
            this.confirmButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.title.Location = new System.Drawing.Point(146, 15);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(58, 30);
            this.title.TabIndex = 0;
            this.title.Text = "Item";
            // 
            // itemNameTextBox
            // 
            this.itemNameTextBox.Location = new System.Drawing.Point(114, 75);
            this.itemNameTextBox.Name = "itemNameTextBox";
            this.itemNameTextBox.Size = new System.Drawing.Size(207, 23);
            this.itemNameTextBox.TabIndex = 1;
            // 
            // itemNameLabel
            // 
            this.itemNameLabel.AutoSize = true;
            this.itemNameLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.itemNameLabel.Location = new System.Drawing.Point(14, 73);
            this.itemNameLabel.Name = "itemNameLabel";
            this.itemNameLabel.Size = new System.Drawing.Size(64, 25);
            this.itemNameLabel.TabIndex = 2;
            this.itemNameLabel.Text = "Name";
            // 
            // itemCategoryLabel
            // 
            this.itemCategoryLabel.AutoSize = true;
            this.itemCategoryLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.itemCategoryLabel.Location = new System.Drawing.Point(14, 116);
            this.itemCategoryLabel.Name = "itemCategoryLabel";
            this.itemCategoryLabel.Size = new System.Drawing.Size(91, 25);
            this.itemCategoryLabel.TabIndex = 4;
            this.itemCategoryLabel.Text = "Category";
            // 
            // itemCategoryTextBox
            // 
            this.itemCategoryTextBox.Location = new System.Drawing.Point(114, 118);
            this.itemCategoryTextBox.Name = "itemCategoryTextBox";
            this.itemCategoryTextBox.Size = new System.Drawing.Size(207, 23);
            this.itemCategoryTextBox.TabIndex = 3;
            // 
            // itemPriceLabel
            // 
            this.itemPriceLabel.AutoSize = true;
            this.itemPriceLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.itemPriceLabel.Location = new System.Drawing.Point(14, 160);
            this.itemPriceLabel.Name = "itemPriceLabel";
            this.itemPriceLabel.Size = new System.Drawing.Size(54, 25);
            this.itemPriceLabel.TabIndex = 6;
            this.itemPriceLabel.Text = "Price";
            // 
            // itemPriceTextBox
            // 
            this.itemPriceTextBox.Location = new System.Drawing.Point(114, 162);
            this.itemPriceTextBox.Name = "itemPriceTextBox";
            this.itemPriceTextBox.Size = new System.Drawing.Size(207, 23);
            this.itemPriceTextBox.TabIndex = 5;
            // 
            // confirmButton
            // 
            this.confirmButton.BackColor = System.Drawing.Color.Silver;
            this.confirmButton.Location = new System.Drawing.Point(244, 214);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(77, 33);
            this.confirmButton.TabIndex = 7;
            this.confirmButton.Text = "Confirm";
            this.confirmButton.UseVisualStyleBackColor = false;
            this.confirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // backButton
            // 
            this.backButton.BackColor = System.Drawing.Color.Gray;
            this.backButton.Location = new System.Drawing.Point(14, 214);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(77, 33);
            this.backButton.TabIndex = 8;
            this.backButton.Text = "Back";
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.BackColor = System.Drawing.Color.Silver;
            this.deleteButton.Location = new System.Drawing.Point(127, 214);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(77, 33);
            this.deleteButton.TabIndex = 9;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = false;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // ItemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(335, 263);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.itemPriceLabel);
            this.Controls.Add(this.itemPriceTextBox);
            this.Controls.Add(this.itemCategoryLabel);
            this.Controls.Add(this.itemCategoryTextBox);
            this.Controls.Add(this.itemNameLabel);
            this.Controls.Add(this.itemNameTextBox);
            this.Controls.Add(this.title);
            this.Name = "ItemForm";
            this.Text = "Item";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label title;
        private TextBox itemNameTextBox;
        private Label itemNameLabel;
        private Label itemCategoryLabel;
        private TextBox itemCategoryTextBox;
        private Label itemPriceLabel;
        private TextBox itemPriceTextBox;
        private Button confirmButton;
        private Button backButton;
        private Button deleteButton;
    }
}