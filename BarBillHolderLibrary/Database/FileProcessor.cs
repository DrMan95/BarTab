using BarBillHolderLibrary.Models;
using CsvHelper;
using System.Globalization;
using System.Text.Json;

namespace BarBillHolderLibrary.Database
{
    public static class FileProcessor
    {
        public static string barDataFile { get; set; }
        public static string menuFile { get; set; }
        public static string menuCSV { get; set; }
        public static string historyCSV { get; set; }


        public static void InitializeFilePath(string filePath)
        {
            FileProcessor.barDataFile = $"{filePath}\\Data\\{FileNames.BAR_DATA}";
            if(!File.Exists(FileProcessor.barDataFile))
            {
                File.Create(FileProcessor.barDataFile);
            }
            FileProcessor.menuCSV = $"{filePath}\\Data\\{FileNames.MENU_CSV}";
            FileProcessor.historyCSV = $"{filePath}\\History";
        }

        public static bool FileBarIsEmpty()
        {
            string fileTxt = File.ReadAllText(FileProcessor.barDataFile);
            
            if (fileTxt.Length == 0) return true;
            return false;
        }

        public static void ParseFileBar()
        {
            string fileTxt = File.ReadAllText(FileProcessor.barDataFile);
            using JsonDocument doc = JsonDocument.Parse(fileTxt);
            JsonElement root = doc.RootElement;
            Bar.name = root[0].GetProperty("name").ToString();
            Bar.tables = ParseTablesFromJSON(root[0].GetProperty("tables"));
            Bar.customers = ParseCustomersFromJSON(root[0].GetProperty("customers"));
            Bar.register = new Register(
                                        decimal.Parse(root[0].GetProperty("register").GetProperty("cash").ToString()),
                                        decimal.Parse(root[0].GetProperty("register").GetProperty("card").ToString())
                                        );
        }

        private static List<Customer> ParseCustomersFromJSON(JsonElement customersJSON)
        {
            List<Customer> customers = new();
            for( int i=0; i< customersJSON.GetArrayLength(); i++ )
            {
                customers.Add(new Customer(
                                            customersJSON[i].GetProperty("name").ToString(), 
                                            ParseBillFromJSON(customersJSON[i].GetProperty("bill")) 
                                            ));
            }
            return customers;
        }

        private static Bill ParseBillFromJSON(JsonElement billJSON)
        {
            Bill bill = new()
            {
                items = ParseItemsFromJSON(billJSON.GetProperty("items")),
                total = decimal.Parse(billJSON.GetProperty("total").ToString())
            };
            return bill;
        }

        private static List<Item> ParseItemsFromJSON(JsonElement itemsJSON)
        {
            List<Item> items = new();
            for (int i=0; i< itemsJSON.GetArrayLength(); i++ )
            {
                items.Add(new Item(
                                    itemsJSON[i].GetProperty("name").ToString(),
                                    itemsJSON[i].GetProperty("category").ToString(),
                                    decimal.Parse(itemsJSON[i].GetProperty("price").ToString()),
                                    Item.Status.Parse<Item.Status>(itemsJSON[i].GetProperty("status").ToString())
                                    ));
            }
            return items;
        }

        private static List<Table> ParseTablesFromJSON(JsonElement tablesJSON)
        {
            List<Table> tables = new();
            for(int i=0; i<14; i++)
            {
                if (bool.Parse(tablesJSON[i].GetProperty("open").ToString()))
                {
                    tables.Add(new Table(i + 1,
                                         true,
                                         ParseBillFromJSON(tablesJSON[i].GetProperty("bill"))));
                }
                else
                {
                    tables.Add(new Table(i + 1));
                }
            }
            return tables;
        }

        public static async Task SaveBarInstanceAsync()
        {
            string barData = $"[{Bar.ToJson()}]";
            await File.WriteAllTextAsync(FileProcessor.barDataFile, barData);
        }

        public static void ReadMenuFromCSV()
        {
            Bar.menu = new();
            if (File.Exists(FileProcessor.menuCSV))
            {
                List<string> lines = File.ReadAllLines(FileProcessor.menuCSV).ToList();
                foreach (string line in lines)
                {
                    string[] cols = line.Split(',');
                    if (Bar.menu.Count == 0)
                    {
                        Bar.menu.Add( Tuple.Create(cols[1], new List<Tuple<string, decimal>> { Tuple.Create(cols[0], decimal.Parse(cols[2]) ) } ) );
                    }
                    else
                    {
                        bool added = false;
                        foreach (Tuple<string, List<Tuple<string, decimal>>> category in Bar.menu)
                        {
                            if (category.Item1 == cols[1])
                            {
                                category.Item2.Add(Tuple.Create(cols[0], decimal.Parse(cols[2])));
                                added = true;
                            }
                        }
                        if (!added)
                        {
                            Bar.menu.Add(Tuple.Create(cols[1], new List<Tuple<string, decimal>> { Tuple.Create(cols[0], decimal.Parse(cols[2])) }));
                        }
                    }

                }
            }
        }
        public static void SaveMenuToCSV()
        {
            List<string> lines = new();
            foreach (Tuple<string , List<Tuple<string , decimal>>> category in Bar.menu)
            {
                foreach (Tuple<string , decimal> item in category.Item2)
                {
                    lines.Add($"{ item.Item1},{category.Item1},{ item.Item2 }");
                }
            }
            File.WriteAllLines(FileProcessor.menuCSV, lines);
        }
        public static void SaveToPaymentHistory(string name, Bill bill)
        {
            DateTime now = DateTime.Now;
            string folder = FileProcessor.historyCSV + $"\\{now.Day}-{now.Month}-{now.Year}";
            string file = folder + $"\\{name}.csv";
            List<string> lines = new();
            foreach (Item item in bill.items)
            {
                lines.Add($"{now.Hour}:{now.Minute},{item.name},{item.price}");
            }
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            File.AppendAllLines(file, lines);
        }
    }
}
