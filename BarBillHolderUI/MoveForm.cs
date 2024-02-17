using BarBillHolderLibrary;
using BarBillHolderLibrary.Models;

namespace BarBillHolderUI
{
    public partial class MoveForm : Form
    {
        Bill bill;
        ParentTableAndCustomer instance;
        public MoveForm(ParentTableAndCustomer instance, Bill bill)
        {
            if (bill.total == 0)
            {
                foreach (Item item in instance.GetBill().items)
                {
                    bill.items.Add(item);
                }
            }
            this.bill = bill;
            this.instance = instance;
            InitializeComponent();
            customersPanel.AutoScroll = true;
            tablesPanel.AutoScroll = true;
            ShowCustomers();
            ShowTables();
        }
        private void ShowCustomers()
        {
            Point point = new(15, 30);
            Size size = new(150, 40);
            Color color = Color.Silver;
            foreach (Customer customer in Bar.customers)
            {
                Button button = new()
                {
                    Text = customer.name,
                    BackColor = color,
                    Size = size,
                    Location = point
                };
                button.Click += (s, e) =>
                {
                    foreach (Item item in this.bill.items)
                    {
                        customer.bill.AddItem(item);
                        this.instance.GetBill().RemoveItem(item);
                    }
                    this.Close();
                };
                customersPanel.Controls.Add(button);
                point.Y += 50;
            }
        }
        private void ShowTables()
        {
            Point point = new(15, 30);
            Size size = new(150, 40);
            Color color = Color.Silver;
            foreach (Table table in Bar.tables)
            {
                Button button = new()
                {
                    Text = table.name,
                    BackColor = color,
                    Size = size,
                    Location = point
                };
                button.Click += (s, e) =>
                {
                    table.open = true;
                    foreach (Item item in this.bill.items)
                    {
                        table.bill.AddItem(item);
                        this.instance.GetBill().RemoveItem(item);
                    }
                    this.Close();
                };
                tablesPanel.Controls.Add(button);
                point.Y += 50;
            }
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
