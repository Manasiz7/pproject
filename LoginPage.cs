using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Cafe_Management_System
{
    
    public partial class LoginPage : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CafeManagement"].ConnectionString;
        public LoginPage()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            string username = textUsername.Text;
            string password = textPassword.Text;
            string role = comboBoxRole.Text;

            if (ValidateLogin(username, password, role))
            {
                if (role == "staff")
                {
                    // Open the staff dashboard
                    Dashboard staffDashboard = new Dashboard();
                    staffDashboard.Show();
                }
                else if (role == "owner")
                {
                    // Open the owner dashboard
                    DashboardOwner ownerDashboard = new DashboardOwner();
                    ownerDashboard.Show();
                }

                this.Hide(); // Hide the login form
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }



        bool ValidateLogin(string username, string password, string role)
        {
            try
            {
                /* using (SqlConnection connection = new SqlConnection(connectionString))
                 {
                     connection.Open();

                     string query = "SELECT COUNT(*) FROM users WHERE username = @Username AND password = @Password AND role = @Role";
                     using (SqlCommand command = new SqlCommand(query, connection))
                     {
                         command.Parameters.AddWithValue("@Username", username);
                         command.Parameters.AddWithValue("@Password", password);
                         command.Parameters.AddWithValue("@Role", role);

                         int count = (int)command.ExecuteScalar();

                         return count > 0;
                     }
                 }*/

                using (CafeManagementSystemEntities dbContext = new CafeManagementSystemEntities())
                {
                    int count = dbContext.users
                        .Where(u => u.Username == username && u.Password == password && u.Role == role)
                        .Count();

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
        }

        private void textPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
