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
    public partial class DashboardOwner : Form
    {


        private string userRole;

        public DashboardOwner()
        {
            InitializeComponent();
            
        }

        


        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            AddItems addItems = new AddItems();
            addItems.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlaceOrder1 placeOrder = new PlaceOrder1();
            placeOrder.Show();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdatesItem updates = new UpdatesItem();
            updates.Show();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveItem removeItems = new RemoveItem();
            removeItems .Show();    
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnStaffInfo_Click(object sender, EventArgs e)
        {
            StaffInfo staffInfo = new StaffInfo();
            staffInfo.Show();
        }

        private void DashboardOwner_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
        }
    }
}
