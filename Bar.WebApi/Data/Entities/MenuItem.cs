namespace Bar.WebApi.Data.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal Price { get; set; }

        // Whether this item is shown in PDA
        public bool Active { get; set; } = true;

        // Simple stock count – null = “unlimited”
        public int? StockQuantity { get; set; }
    }
}