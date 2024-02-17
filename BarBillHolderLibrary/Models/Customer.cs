using BarBillHolderLibrary.Models;

namespace BarBillHolderLibrary
{
    public class Customer : ParentTableAndCustomer
    {
        /// <summary>
        /// Represents the name of this customer
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Represents the bill that this customer curently has
        /// </summary>
        public Bill bill { get; set; }

        public Customer(string name)
        {
            this.name = name;
            this.bill = new Bill();
            base.isCustomer = true;
        }
        public Customer(string name, Bill bill)
        {
            this.name = name;
            this.bill = bill;
            base.isCustomer = true;
        }
        public override string GetName()
        {
            return this.name;
        }
        public override Bill GetBill()
        {
            return this.bill;
        }
        public override void Remove()
        {
            Bar.customers.Remove(this);
            this.name = "";
            this.bill.Remove();
        }
    }
}
