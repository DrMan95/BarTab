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


        public static void InitializeFilePath(string basePath)
        {
            var dataDir = Path.Combine(basePath, "Data");
            Directory.CreateDirectory(dataDir);  // make sure it exists

            barDataFile = Path.Combine(dataDir, FileNames.BAR_DATA);
            if (!File.Exists(barDataFile))
            {
                using (File.Create(barDataFile)) { }
            }

            menuCSV = Path.Combine(dataDir, FileNames.MENU_CSV);

            historyCSV = Path.Combine(basePath, "History");
            Directory.CreateDirectory(historyCSV);
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
                                        decimal.Parse(root[0].GetProperty("register").GetProperty("card").ToString()),
                                        decimal.Parse(root[0].GetProperty("register").GetProperty("tips").ToString())
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

            // Use the full length of the JSON array (includes bar tables created at runtime)
            for (int i = 0; i < tablesJSON.GetArrayLength(); i++)
            {
                var tJson = tablesJSON[i];

                // Prefer reading the ID from JSON if it exists; fallback to i+1 if not.
                int id;
                if (tJson.TryGetProperty("ID", out var idProp))
                {
                    id = idProp.GetInt32();
                }
                else
                {
                    id = i + 1;
                }

                bool open = bool.Parse(tJson.GetProperty("open").ToString());

                if (open)
                {
                    tables.Add(new Table(
                        id,
                        true,
                        ParseBillFromJSON(tJson.GetProperty("bill"))
                    ));
                }
                else
                {
                    tables.Add(new Table(id));
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
            if (!File.Exists(FileProcessor.menuCSV))
                return;

            var lines = File.ReadAllLines(FileProcessor.menuCSV)
                            .Where(l => !string.IsNullOrWhiteSpace(l))
                            .ToList();

            if (lines.Count == 0)
                return;

            // Detect & skip header if present
            int startIndex = 0;
            var first = lines[0];
            if (first.Contains("Name", StringComparison.OrdinalIgnoreCase) &&
                first.Contains("Category", StringComparison.OrdinalIgnoreCase))
            {
                startIndex = 1; // skip header row
            }

            for (int i = startIndex; i < lines.Count; i++)
            {
                var line = lines[i];

                // Support both old (comma) and new (semicolon) formats
                char separator = line.Contains(';') ? ';' : ',';
                var cols = line.Split(separator);

                // Old format: Name,Category,Price
                // New format: Name;Category;Price;Active
                if (cols.Length < 3)
                    continue;

                var name = cols[0].Trim();
                var category = cols[1].Trim();

                if (!decimal.TryParse(
                        cols[2].Trim(),
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out var price))
                {
                    continue; // skip invalid rows
                }

                bool active = true;
                if (cols.Length >= 4)
                {
                    var activeText = cols[3].Trim();
                    if (!string.IsNullOrEmpty(activeText))
                    {
                        active = activeText.Equals("true", StringComparison.OrdinalIgnoreCase)
                              || activeText == "1"
                              || activeText.Equals("yes", StringComparison.OrdinalIgnoreCase);
                    }
                }

                // Ignore inactive rows (for PDA)
                if (!active)
                    continue;

                // Build Bar.menu structure: List<(category, List<(name, price)>)>
                if (Bar.menu.Count == 0)
                {
                    Bar.menu.Add(
                        Tuple.Create(
                            category,
                            new List<Tuple<string, decimal>>
                            {
                                Tuple.Create(name, price)
                            }
                        )
                    );
                }
                else
                {
                    bool added = false;
                    foreach (var cat in Bar.menu)
                    {
                        if (cat.Item1 == category)
                        {
                            cat.Item2.Add(Tuple.Create(name, price));
                            added = true;
                            break;
                        }
                    }

                    if (!added)
                    {
                        Bar.menu.Add(
                            Tuple.Create(
                                category,
                                new List<Tuple<string, decimal>>
                                {
                                    Tuple.Create(name, price)
                                }
                            )
                        );
                    }
                }
            }
        }
        public static void SaveMenuToCSV()
        {
            var lines = new List<string>();

            // New header, to match MenuController expectation
            lines.Add("Name;Category;Price;Active");

            foreach (var category in Bar.menu)
            {
                foreach (var item in category.Item2)
                {
                    var name = item.Item1;
                    var price = item.Item2.ToString(CultureInfo.InvariantCulture);

                    // For now we persist everything as Active=true
                    lines.Add($"{name};{category.Item1};{price};true");
                }
            }

            File.WriteAllLines(FileProcessor.menuCSV, lines);
        }
        public static void SaveToPaymentHistory(string name, Bill bill, decimal tips)
        {
            DateTime now = DateTime.Now;
            string folder = FileProcessor.historyCSV + $"\\{now.Day}-{now.Month}-{now.Year}";
            string file = folder + $"\\{name}.csv";
            List<string> lines = new();
            foreach (Item item in bill.items)
            {
                lines.Add($"{now.Hour}:{now.Minute},{item.name},{item.price}");
            }
            lines.Add($"{now.Hour}:{now.Minute},{"tips"},{tips}");
            //lines.Add("----------------------------------------");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            File.AppendAllLines(file, lines);
        }
    }
}
