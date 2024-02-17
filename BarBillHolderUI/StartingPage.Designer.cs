namespace BarBillHolderUI
{
    partial class StartingPage
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
            this.welcomeTxt = new System.Windows.Forms.Label();
            this.enterButton = new System.Windows.Forms.Button();
            this.editMenuButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.historyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // welcomeTxt
            // 
            this.welcomeTxt.AutoSize = true;
            this.welcomeTxt.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.welcomeTxt.ForeColor = System.Drawing.Color.DimGray;
            this.welcomeTxt.Location = new System.Drawing.Point(31, 26);
            this.welcomeTxt.Name = "welcomeTxt";
            this.welcomeTxt.Size = new System.Drawing.Size(279, 65);
            this.welcomeTxt.TabIndex = 0;
            this.welcomeTxt.Text = "WELCOME!";
            // 
            // enterButton
            // 
            this.enterButton.BackColor = System.Drawing.Color.DimGray;
            this.enterButton.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.enterButton.ForeColor = System.Drawing.Color.Black;
            this.enterButton.Location = new System.Drawing.Point(77, 129);
            this.enterButton.Name = "enterButton";
            this.enterButton.Size = new System.Drawing.Size(186, 68);
            this.enterButton.TabIndex = 1;
            this.enterButton.Text = "Enter";
            this.enterButton.UseVisualStyleBackColor = false;
            this.enterButton.Click += new System.EventHandler(this.EnterButton_Click);
            // 
            // editMenuButton
            // 
            this.editMenuButton.BackColor = System.Drawing.Color.DimGray;
            this.editMenuButton.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.editMenuButton.ForeColor = System.Drawing.Color.Black;
            this.editMenuButton.Location = new System.Drawing.Point(77, 203);
            this.editMenuButton.Name = "editMenuButton";
            this.editMenuButton.Size = new System.Drawing.Size(186, 68);
            this.editMenuButton.TabIndex = 2;
            this.editMenuButton.Text = "Edit Menu";
            this.editMenuButton.UseVisualStyleBackColor = false;
            this.editMenuButton.Click += new System.EventHandler(this.EditMenuButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.BackColor = System.Drawing.Color.DimGray;
            this.exitButton.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.exitButton.ForeColor = System.Drawing.Color.Black;
            this.exitButton.Location = new System.Drawing.Point(77, 352);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(186, 68);
            this.exitButton.TabIndex = 3;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // historyButton
            // 
            this.historyButton.BackColor = System.Drawing.Color.DimGray;
            this.historyButton.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.historyButton.ForeColor = System.Drawing.Color.Black;
            this.historyButton.Location = new System.Drawing.Point(77, 277);
            this.historyButton.Name = "historyButton";
            this.historyButton.Size = new System.Drawing.Size(186, 68);
            this.historyButton.TabIndex = 4;
            this.historyButton.Text = "History";
            this.historyButton.UseVisualStyleBackColor = false;
            this.historyButton.Click += new System.EventHandler(this.historyButton_Click);
            // 
            // StartingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(350, 447);
            this.Controls.Add(this.historyButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.editMenuButton);
            this.Controls.Add(this.enterButton);
            this.Controls.Add(this.welcomeTxt);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "StartingPage";
            this.Text = "StartingPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label welcomeTxt;
        private Button enterButton;
        private Button editMenuButton;
        private Button exitButton;
        private Button historyButton;
    }
}