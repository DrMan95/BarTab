using BarBillHolderLibrary;
using BarBillHolderLibrary.Database;
using BarBillHolderLibrary.Models;

namespace BarBillHolderUI
{
    public partial class CustomerOverview : Form
    {
        ParentTableAndCustomer instance;
        Bill itemsChecked = new();
        public CustomerOverview(ParentTableAndCustomer instance)
        {
            InitializeComponent();
            this.instance = instance;
            this.UpdateInstanceData();
        }

        private void UpdateInstanceData()
        {
            customerOverviewGroupBox.Text = this.instance.GetName();
            totalBill.Clear();
            totalBill.Text = this.instance.GetBill().total.ToString() + "€";
            this.LoadItems();
        }

        private void LoadItems()
        {
            itemsPanel.Controls.Clear();
            itemsPanel.AutoScroll = true;
            Point point = new(30, 10);
            foreach(Item item in this.instance.GetBill().items)
            {
                Label label = new()
                {
                    Text = item.name + "  " + item.price + "€",
                    Location = new Point(point.X , point.Y + 3),
                    AutoSize = true
                };
                label.Click += (s, e) =>
                {
                    item.status = Item.Status.DONE;
                    label.Text = item.name + "  " + item.price + "€";
                };
                itemsPanel.Controls.Add(label);

                CheckBox checkBox = new()
                {
                    Location = new Point(point.X - 20, point.Y)
                };
                point.Y += 30;
                if (item.status == Item.Status.UNDONE)
                {
                    label.Text = item.name + "  " + item.price + "€  Pending...";
                }
                else if (item.status == Item.Status.DONE)
                {
                    label.Text = item.name + "  " + item.price + "€";
                }
                checkBox.Click += (s, e) =>
                {
                    if (checkBox.Checked)
                    {
                        this.itemsChecked.AddItem(item);
                        totalBill.Text = this.itemsChecked.total.ToString() + "€";
                    }
                    else
                    {
                        this.itemsChecked.RemoveItem(item);
                        if(this.itemsChecked.total  > 0)
                        {
                            totalBill.Text = this.itemsChecked.total.ToString() + "€";
                        }
                        else
                        {
                            totalBill.Text = this.instance.GetBill().total.ToString() + "€";
                        }
                    }
                };
                itemsPanel.Controls.Add(checkBox);
            }
        }

        private void PayButton_Click(object sender, EventArgs e)
        {
            if (this.itemsChecked.total == 0)
            {
                Bill bill = new(this.instance.GetBill());
                PayForm frm = new(this.instance.GetBill());
                frm.ShowDialog();
                if (this.instance.GetBill().total == 0)
                {
                    FileProcessor.SaveToPaymentHistory(this.instance.GetName(), bill);
                    this.instance.Remove();
                    this.Close();
                }
            }
            else
            {
                Bill bill = new(this.itemsChecked);
                PayForm frm = new(bill);
                frm.ShowDialog();
                if (bill.total == 0)
                {
                    foreach (Item item in this.itemsChecked.items)
                    {
                        this.instance.GetBill().RemoveItem(item);
                    }
                    if (this.instance.GetBill().total == 0)
                    {
                        this.instance.Remove();
                        this.Close();
                    }
                    this.UpdateInstanceData();
                }
            }
        }
        private void NewOrder_Click(object sender, EventArgs e)
        {
            newOrder frm = new(this.instance);
            frm.ShowDialog();
            this.UpdateInstanceData();
        }
        private void MoveButton_Click(object sender, EventArgs e)
        {
            MoveForm frm = new(this.instance, this.itemsChecked);
            frm.ShowDialog();
            this.itemsChecked.Remove();
            this.UpdateInstanceData();
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
