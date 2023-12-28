using System;
using System.Windows.Forms;

namespace Cafe_Management_System
{
    public partial class welcome : Form
    {
        private Timer timer;

        public welcome()
        {
            InitializeComponent();

            // Initialize and configure the Timer
            timer = new Timer();
            timer.Interval = 5000; // 5 seconds
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Stop the timer to prevent it from ticking again
            timer.Stop();

            // Open the LoginPage form
            LoginPage loginPage = new LoginPage();
            loginPage.FormClosed += (s, args) => { this.Close(); }; // Close welcome form when LoginPage is closed
            loginPage.Show();

            // Hide the current welcome form
            this.Hide();
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Your paint code here
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Your click event code here
        }
    }
}
