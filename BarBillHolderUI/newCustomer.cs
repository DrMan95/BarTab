using BarBillHolderLibrary;
using BarBillHolderLibrary.Models;
using BarBillHolderUI.Interfaces;

namespace BarBillHolderUI
{
    public partial class newCustomer : Form
    {
        ICustomerCaller callingForm;

        public newCustomer(ICustomerCaller caller)
        {
            InitializeComponent();
            this.callingForm = caller;
        }

        private void ConfirmNewCustomer_Click(object sender, EventArgs e)
        {
            string name = newCustomerNameTextBox.Text;
            if ( ValidName(name) )
            {
                Customer newCustomer = new(name);
                this.callingForm.CustomerComplete(newCustomer);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid name that doesn't already exits");
            }
        }

        private bool ValidName(string name)
        {
            if (name.Length == 0) { return false; }
            foreach (Customer c in Bar.customers)
            {
                if (c.name == name) { return false; }
            }
            return true;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
