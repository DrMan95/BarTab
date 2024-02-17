namespace BarBillHolderUI
{
    partial class PayForm
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
            this.cashButton = new System.Windows.Forms.Button();
            this.cardButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.cashLabel = new System.Windows.Forms.Label();
            this.cardLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cashButton
            // 
            this.cashButton.BackColor = System.Drawing.Color.Silver;
            this.cashButton.Font = new System.Drawing.Font("Segoe UI Semibold", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cashButton.Location = new System.Drawing.Point(28, 73);
            this.cashButton.Name = "cashButton";
            this.cashButton.Size = new System.Drawing.Size(174, 81);
            this.cashButton.TabIndex = 0;
            this.cashButton.Text = "cash";
            this.cashButton.UseVisualStyleBackColor = false;
            this.cashButton.Click += new System.EventHandler(this.CashButton_Click);
            // 
            // cardButton
            // 
            this.cardButton.BackColor = System.Drawing.Color.Silver;
            this.cardButton.Font = new System.Drawing.Font("Segoe UI Semibold", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cardButton.Location = new System.Drawing.Point(246, 73);
            this.cardButton.Name = "cardButton";
            this.cardButton.Size = new System.Drawing.Size(174, 81);
            this.cardButton.TabIndex = 1;
            this.cardButton.Text = "card";
            this.cardButton.UseVisualStyleBackColor = false;
            this.cardButton.Click += new System.EventHandler(this.CardButton_Click);
            // 
            // backButton
            // 
            this.backButton.BackColor = System.Drawing.Color.Gray;
            this.backButton.Location = new System.Drawing.Point(165, 178);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(117, 39);
            this.backButton.TabIndex = 2;
            this.backButton.Text = "Back";
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // cashLabel
            // 
            this.cashLabel.AutoSize = true;
            this.cashLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cashLabel.Location = new System.Drawing.Point(74, 29);
            this.cashLabel.Name = "cashLabel";
            this.cashLabel.Size = new System.Drawing.Size(76, 32);
            this.cashLabel.TabIndex = 3;
            this.cashLabel.Text = "CASH";
            // 
            // cardLabel
            // 
            this.cardLabel.AutoSize = true;
            this.cardLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cardLabel.Location = new System.Drawing.Point(301, 29);
            this.cardLabel.Name = "cardLabel";
            this.cardLabel.Size = new System.Drawing.Size(77, 32);
            this.cardLabel.TabIndex = 4;
            this.cardLabel.Text = "CARD";
            // 
            // PayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 243);
            this.Controls.Add(this.cardLabel);
            this.Controls.Add(this.cashLabel);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.cardButton);
            this.Controls.Add(this.cashButton);
            this.Name = "PayForm";
            this.Text = "PayForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button cashButton;
        private Button cardButton;
        private Button backButton;
        private Label cashLabel;
        private Label cardLabel;
    }
}