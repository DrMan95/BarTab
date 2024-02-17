using BarBillHolderLibrary;
using BarBillHolderLibrary.Database;
using BarBillHolderLibrary.Models;

namespace BarBillHolderUI
{
    public partial class EditMenuPage : Form
    {
        public EditMenuPage()
        {
            InitializeComponent();
            DisplayMenu();
        }

        private void DisplayMenu()
        {
            menuTabControl.Controls.Clear();
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
                menuTabControl.Controls.Add(tab);

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
                        Location = point
                    };
                    button.Click += (s, e) =>
                    {
                        ItemForm frm = new(new Item(item.Item1, category.Item1, item.Item2));
                        frm.ShowDialog();
                        DisplayMenu();
                    };
                    tab.Controls.Add(button);
                    point.Y += size.Height + 10;
                    if (point.Y == (x + 5 * size.Height + 50))
                    {
                        point.X = nextCol.X;
                        point.Y = nextCol.Y;
                        nextCol.X += size.Width + 10;
                        nextCol.Y = nextCol.Y;
                    }
                }
            }
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            FileProcessor.SaveMenuToCSV();
            this.Close();
        }
        private void NewItemButton_Click(object sender, EventArgs e)
        {
            ItemForm frm = new(new Item("#@#new#@#", "#@#new#@#", 0));
            frm.ShowDialog();
            DisplayMenu();
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            FileProcessor.ReadMenuFromCSV();
            this.Close();
        }
    }
}
