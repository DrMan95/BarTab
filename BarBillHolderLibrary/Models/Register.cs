namespace BarBillHolderLibrary.Models
{
    public class Register
    {
        public decimal cash { get; set; }
        public decimal card { get; set; }
        public decimal tips { get; set; }

        public Register()
        {
            this.cash = 0;
            this.card = 0;
            this.tips = 0;
        }
        public Register(decimal cash, decimal card, decimal tips)
        {
            this.cash = cash;
            this.card = card;
            this.tips = tips;
        }
        public override string ToString()
        {
            return $"Cash: {this.cash}€\nCard: {this.card}€\nTips: {this.tips}€\nPending: {Bar.PendingBills()}€";
        }
    }
}
