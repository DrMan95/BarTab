using BarBillHolderLibrary.Models;

namespace BarBillHolderLibrary
{
    public class Item : IRemove
    {
        public enum Status { EMPTY, UNDONE, DONE };
        /// <summary>
        /// Represents the name of the item
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Represents the category of this item
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// Represents the price of thit item
        /// </summary>
        public decimal price { get; set; }

        public Status status { get; set; }


        public Item(string name, string category, decimal price, Status status)
        {
            this.name = name;
            this.category = category;
            this.price = price;
            this.status = status;
        }
        public Item(string name, string category, decimal price)
        {
            this.name = name;
            this.category = category;
            this.price = price;
            this.status = Status.EMPTY;
        }

        public void Remove()
        {
            foreach (Tuple<string, List<Tuple<string, decimal>>> category in Bar.menu)
            {
                if (category.Item1 == this.category)
                {
                    foreach (Tuple<string, decimal> item in category.Item2)
                    {
                        if (this.name == item.Item1)
                        {
                            category.Item2.Remove(item);
                            this.name = "";
                            this.price = 0;
                            return;
                        }
                    }
                }
            }
        }
    }
}
