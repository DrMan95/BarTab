namespace BarBillHolderUI
{
    partial class HistoryForm
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
            this.historyTabControl = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // historyTabControl
            // 
            this.historyTabControl.Location = new System.Drawing.Point(12, 12);
            this.historyTabControl.Name = "historyTabControl";
            this.historyTabControl.SelectedIndex = 0;
            this.historyTabControl.Size = new System.Drawing.Size(977, 533);
            this.historyTabControl.TabIndex = 0;
            // 
            // HistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 561);
            this.Controls.Add(this.historyTabControl);
            this.Name = "HistoryForm";
            this.Text = "History";
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl historyTabControl;
    }
}