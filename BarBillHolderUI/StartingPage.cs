using BarBillHolderLibrary.Database;
using System;
namespace BarBillHolderUI
{
    public partial class StartingPage : Form
    {
        public StartingPage()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            BarOverview frm = new();
            frm.ShowDialog();
        }
        private void EditMenuButton_Click(object sender, EventArgs e)
        {
            EditMenuPage frm = new();
            frm.ShowDialog();
        }
        private void historyButton_Click(object sender, EventArgs e)
        {
            HistoryForm frm = new();
            frm.ShowDialog();
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            _ = FileProcessor.SaveBarInstanceAsync();
            this.Close();
        }
    }
}
