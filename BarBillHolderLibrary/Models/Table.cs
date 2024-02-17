namespace BarBillHolderLibrary.Models
{
    public class Table : ParentTableAndCustomer
    {
        /// <summary>
        /// Represents the table's number
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Represents the table's name
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Flags if there is unpayed bill
        /// true if there is still unpayed bill
        /// false otherwise
        /// </summary>
        public bool open { get; set; }
        /// <summary>
        /// Represents the bill that this table curently has
        /// </summary>
        public Bill bill { get; set; }


        public Table(int ID)
        {
            this.ID = ID;
            this.name = this.SetName(ID);
            this.open = false;
            this.bill = new Bill();
            base.isCustomer = false;
        }

        public Table(int ID, bool open, Bill bill)
        {
            this.ID = ID;
            this.name = this.SetName(ID);
            this.open = open;
            this.bill = bill;
            base.isCustomer = false;
        }
        private string SetName(int ID)
        {
            if (ID == 13)
            {
                return "Table Window";
            }
            else if (ID == 14)
            {
                return "Table Stand";
            }
            else
            {
                return $"Table {ID}";
            }
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
            this.open = false;
            this.bill.Remove();
        }
    }
}
