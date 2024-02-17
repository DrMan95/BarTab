using System.Text.Json;

namespace BarBillHolderLibrary.Models
{
    public static class Bar
    {
        /// <summary>
        /// Represents the name of the bar
        /// </summary>
        public static string? name { get; set; }
        /// <summary>
        /// Represents the tables this bar has
        /// </summary>
        public static List<Table>? tables { get; set; }
        /// <summary>
        /// Represents the customers that are in the bar
        /// </summary>
        public static List<Customer>? customers { get; set; }
        /// <summary>
        /// Represents the cash machine of the bar
        /// </summary>
        public static Register? register { get; set; }
        /// <summary>
        /// This is the menu of the shop
        /// </summary>
        public static List<Tuple<string, List<Tuple<string, decimal>>>>? menu;

        public static void InitializeBar(string name)
        {
            //TODO check if there is a bar this this name in the database
            // and if so insert all its data
            //  Bar(string name, List<Table> tables, List<Customer> customers, float register)
            // else
            Bar.name = name;
            Bar.tables = new();
            for (int i = 0; i < 14; i++)
            {
                Bar.tables.Add(new Table(i + 1));
            }
            Bar.customers = new();
            Bar.register = new();
        }

        internal static object ToJson()
        {
            return $"{{\"name\": \"{Bar.name}\",\"tables\":{JsonSerializer.Serialize(Bar.tables)},\"customers\":{JsonSerializer.Serialize(Bar.customers)},\"register\":{JsonSerializer.Serialize(Bar.register)}}}";
        }

        internal static object PendingBills()
        {
            decimal pending = 0;
            foreach (Customer customer in Bar.customers)
            {
                pending += customer.bill.total;
            }
            foreach (Table table in Bar.tables)
            {
                pending += table.bill.total;
            }
            return pending;
        }
    }
}
