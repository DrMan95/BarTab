namespace BarBillHolderLibrary.Models
{
    public class Bill : IRemove
    {
        /// <summary>
        /// Represents the items that had been ordered
        /// </summary>
        public List<Item> items { get; set; }
        /// <summary>
        /// Represents the total costs of those items
        /// </summary>
        public decimal total { get; set; }

        public Bill()
        {
            this.items = new List<Item>();
            this.total = 0;
        }
        public Bill(Bill bill)
        {
            this.items = new();
            foreach (Item item in bill.items)
            {
                this.items.Add(item);
            }
            this.total = bill.total;
        }

        public void AddItem(Item item)
        {
            this.items.Add(item);
            this.total += item.price;
        }
        public void RemoveItem(Item item)
        {
            this.items.Remove(item);
            this.total -= item.price;
        }
        public void Remove()
        {
            this.items.Clear();
            this.total = 0;
        }
    }
}
