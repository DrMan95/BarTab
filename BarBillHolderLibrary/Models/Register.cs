namespace BarBillHolderLibrary.Models
{
    public class Register
    {
        public decimal cash { get; set; }
        public decimal card { get; set; }

        public Register()
        {
            this.cash = 0;
            this.card = 0;
        }
        public Register(decimal cash, decimal card)
        {
            this.cash = cash;
            this.card = card;
        }
        public override string ToString()
        {
            return $"Cash: {this.cash}€\nCard: {this.card}€\nPending: {Bar.PendingBills()}€";
        }
    }
}
