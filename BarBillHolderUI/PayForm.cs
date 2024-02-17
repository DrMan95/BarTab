using BarBillHolderLibrary.Models;

namespace BarBillHolderUI
{
    public partial class PayForm : Form
    {
        Bill bill;
        decimal comition = 1.03M;
        public PayForm(Bill bill)
        {
            this.bill = bill;
            InitializeComponent();
            cashButton.Text = this.bill.total.ToString();
            cardButton.Text = (this.bill.total * this.comition).ToString();
        }

        private void CashButton_Click(object sender, EventArgs e)
        {
            Bar.register.cash += this.bill.total;
            this.bill.Remove();
            this.Close();
        }
        private void CardButton_Click(object sender, EventArgs e)
        {
            Bar.register.card += this.bill.total * this.comition;
            this.bill.Remove();
            this.Close();
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
