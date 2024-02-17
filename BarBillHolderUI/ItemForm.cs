using BarBillHolderLibrary;
using BarBillHolderLibrary.Models;

namespace BarBillHolderUI
{
    public partial class ItemForm : Form
    {
        Item item;
        bool isNewItem;
        public ItemForm(Item item)
        {
            InitializeComponent();
            this.item = item;
            if (this.item.name == "#@#new#@#")
            {
                title.Text = "Add new item";
                Text = "Add new item";
                this.isNewItem= true;
                deleteButton.Visible = false;
            }
            else
            {
                this.isNewItem= false;
                itemNameTextBox.Text = item.name;
                itemCategoryTextBox.Text = item.category;
                itemPriceTextBox.Text = item.price.ToString();
            }
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            decimal price;
            if (!decimal.TryParse(itemPriceTextBox.Text, out price))
            {
                MessageBox.Show("Please enter a valid price for the item.");
                return;
            }
            else if (price <= 0 || false )  //<--  fix validation <----------------------------------------------------
            {
                MessageBox.Show("Please enter a valid price for the item.");
                return;
            }
            if (this.isNewItem)
            {
                this.item.name = itemNameTextBox.Text;
                this.item.category = itemCategoryTextBox.Text;
                this.item.price = decimal.Parse(itemPriceTextBox.Text);
            }
            bool ifDoesntExits = true;
            foreach (Tuple<string, List<Tuple<string, decimal>>> category in Bar.menu)
            {
                if (category.Item1 == this.item.category)
                {
                    foreach (Tuple<string, decimal> item in category.Item2)
                    {
                        if (item.Item1 == this.item.name)
                        {
                            if(isNewItem)
                            {
                                MessageBox.Show("An item with this name already exits.");
                                ifDoesntExits = false;
                            }
                            else
                            {
                                category.Item2.Remove(item);
                                this.AddItem(new Item(itemNameTextBox.Text, itemCategoryTextBox.Text, decimal.Parse(itemPriceTextBox.Text)));
                            }
                            return;
                        }
                    }
                }
            }
            if (ifDoesntExits)
            {
                this.AddItem(this.item);
            }
        }
        private void AddItem(Item item)
        {
            foreach (Tuple<string, List<Tuple<string, decimal>>> category in Bar.menu)
            {
                if (category.Item1 == item.category)
                {
                    category.Item2.Add( Tuple.Create(item.name, item.price) );
                    this.Close();
                    return;
                }
            }
            Tuple<string, List<Tuple<string, decimal>>> newCategory = Tuple.Create(item.category, new List<Tuple<string, decimal>> { Tuple.Create(item.name, item.price) } ) ;
            Bar.menu.Add(newCategory);
            this.Close();
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            foreach (Tuple<string, List<Tuple<string, decimal>>> category in Bar.menu)
            {
                if (category.Item1 == this.item.category)
                {
                    foreach (Tuple<string, decimal> item in category.Item2)
                    {
                        if (item.Item1 == this.item.name)
                        {
                            category.Item2.Remove(item);
                            if (category.Item2.Count == 0)
                            {
                                Bar.menu.Remove(category);
                            }
                            this.Close();
                            return;
                        }
                    }
                }
            }
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
