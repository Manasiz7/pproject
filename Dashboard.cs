using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafe_Management_System
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {

            PlaceOrder1 placeOrder = new PlaceOrder1();
            placeOrder.Show();
            this.Hide();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            AddItems addItems = new AddItems();
            addItems.Show();
        }

        private void btnUpdateItem_Click(object sender, EventArgs e)
        {
            UpdatesItem updates = new UpdatesItem();
            updates.Show();

        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {

            RemoveItem removeItems = new RemoveItem();
            removeItems.Show();

        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
        }
    }
}
