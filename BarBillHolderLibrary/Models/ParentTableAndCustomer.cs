namespace BarBillHolderLibrary.Models
{
    public abstract class ParentTableAndCustomer : IRemove
    {
        public bool isCustomer { get; set; }
        public abstract string GetName();
        public abstract Bill GetBill();
        public abstract void Remove();
    }
}
