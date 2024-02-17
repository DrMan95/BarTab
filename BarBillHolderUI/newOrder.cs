using BarBillHolderLibrary;
using BarBillHolderLibrary.Models;

namespace BarBillHolderUI
{
    public partial class newOrder : Form
    {
        ParentTableAndCustomer instance;
        List<Item> newOrderList = new();
        public newOrder(ParentTableAndCustomer instance)
        {
            InitializeComponent();
            this.instance = instance;
            newOrderGroupBox.Text = "New order for " + this.instance.GetName();
            
            foreach (Tuple<string, List<Tuple<string, decimal>>> category in Bar.menu)
            {
                TabPage tab = new()
                {
                    Location = new Point(4, 24),
                    Name = category.Item1,
                    Padding = new Padding(3),
                    Size = new Size(768, 340),
                    TabIndex = 0,
                    Text = category.Item1,
                    UseVisualStyleBackColor = true,
                    AutoScroll = true
                };
                categoriesTabControl.Controls.Add(tab);

                int x = 20, y = x;
                Size size = new(150, 50);
                Point point = new(x, y);
                Point nextCol = new(x + size.Width + 10, y);
                foreach (Tuple<string, decimal> item in category.Item2)
                {
                    Button button = new()
                    {
                        Text = item.Item1,
                        Size = size,
                        Location = point,
                        Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point)
                    };
                    button.Click += (s, e) =>
                    {
                        this.newOrderList.Add(new Item(item.Item1, category.Item1, item.Item2, Item.Status.UNDONE));
                        this.ShowNewOrderList();
                    };
                    tab.Controls.Add(button);
                    point.Y += size.Height + 10;
                    if (point.Y == (x + 5 * size.Height + 50))
                    {
                        point.X = nextCol.X;
                        point.Y = nextCol.Y;
                        nextCol.X +=  size.Width + 10;
                        nextCol.Y = nextCol.Y;
                    }
                }
            }
        }

        private void ShowNewOrderList()
        {
            Size size = new(100, 40);
            Point point = new(20, 40);
            foreach (Item item in this.newOrderList)
            {
                Button button = new()
                {
                    Text = item.name,
                    Size = size,
                    Location = point,
                    Font = new Font("Segoe UI Semibold", 10.25F, FontStyle.Bold, GraphicsUnit.Point)
                };
                button.Click += (s, e) =>
                {
                    this.newOrderList.Remove(item);
                    newOrderGroupBox.Controls.Clear();
                    this.ShowNewOrderList();
                };
                newOrderGroupBox.Controls.Add(button);
                point.Y += size.Height + 10;
            }
        }

        private void ConfirmOrderButton_Click(object sender, EventArgs e)
        {
            Bill bill = this.instance.GetBill();
            foreach (Item item in this.newOrderList)
            {
                bill.AddItem(item);
            }
            this.newOrderList.Clear();
            this.Close();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
