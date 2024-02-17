using BarBillHolderLibrary;
using BarBillHolderLibrary.Database;
using BarBillHolderLibrary.Models;
using BarBillHolderUI.Interfaces;

namespace BarBillHolderUI
{
    public partial class BarOverview : Form , ICustomerCaller
    {
        public BarOverview()
        {
            this.InitializeComponent();
            this.ShowCustomers();
            customersPanel.AutoScroll = true;
            this.FillTables();
        }

        private void FillTables()
        {
            this.FillTable(table1, Bar.tables[0]);
            this.FillTable(table2, Bar.tables[1]);
            this.FillTable(table3, Bar.tables[2]);
            this.FillTable(table4, Bar.tables[3]);
            this.FillTable(table5, Bar.tables[4]);
            this.FillTable(table6, Bar.tables[5]);
            this.FillTable(table7, Bar.tables[6]);
            this.FillTable(table8, Bar.tables[7]);
            this.FillTable(table9, Bar.tables[8]);
            this.FillTable(table10, Bar.tables[9]);
            this.FillTable(table11, Bar.tables[10]);
            this.FillTable(table12, Bar.tables[11]);
            this.FillTable(tableWindow, Bar.tables[12]);
            this.FillTable(tableStand, Bar.tables[13]);
        }

        private void FillTable(Button button, Table table)
        {
            button.BackColor = SetColorForTable(table);
            button.Click += (s, e) =>
            {
                CustomerOverview frm = new(table);
                frm.ShowDialog();
                if (table.bill.total > 0) { table.open = true; }
                else { table.open = false; }
                this.SetColorForTables();
            };
        }
        private void SetColorForTables()
        {
            table1.BackColor = this.SetColorForTable(Bar.tables[0]);
            table2.BackColor = this.SetColorForTable(Bar.tables[1]);
            table3.BackColor = this.SetColorForTable(Bar.tables[2]);
            table4.BackColor = this.SetColorForTable(Bar.tables[3]);
            table5.BackColor = this.SetColorForTable(Bar.tables[4]);
            table6.BackColor = this.SetColorForTable(Bar.tables[5]);
            table7.BackColor = this.SetColorForTable(Bar.tables[6]);
            table8.BackColor = this.SetColorForTable(Bar.tables[7]);
            table9.BackColor = this.SetColorForTable(Bar.tables[8]);
            table10.BackColor = this.SetColorForTable(Bar.tables[9]);
            table11.BackColor = this.SetColorForTable(Bar.tables[10]);
            table12.BackColor = this.SetColorForTable(Bar.tables[11]);
            tableWindow.BackColor = this.SetColorForTable(Bar.tables[12]);
            tableStand.BackColor = this.SetColorForTable(Bar.tables[13]);
        }
        public Color SetColorForTable(Table table)
        {
            Color color = Color.White;
            if (table.bill.total > 0)
            {
                color = Color.Green;
            }
            foreach (Item item in table.bill.items)
            {
                if (item.status == Item.Status.UNDONE)
                {
                    color = Color.Blue;
                }
            }
            return color;
        }
        private void ShowCustomers()
        {
            Point point = new(15, 30);
            Size size = new(180, 40);
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
                    CustomerOverview frm = new(customer);
                    frm.ShowDialog();
                    customersPanel.Controls.Clear();
                    this.ShowCustomers();
                };
                customersPanel.Controls.Add(button);
                point.Y += 50;
            }
        }

        public void CustomerComplete(Customer newCustomer)
        {
            Bar.customers.Add(newCustomer);
            customersPanel.Controls.Clear();
            this.ShowCustomers();
            _ = FileProcessor.SaveBarInstanceAsync();
        }

        private void NewCustomerButton_Click(object sender, EventArgs e)
        {
            newCustomer frm = new(this);
            frm.ShowDialog();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Bar.register.ToString());
        }
    }
}
