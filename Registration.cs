using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafe_Management_System
{
    public partial class Registration : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CafeManagement"].ConnectionString;

        public Registration()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        bool ValidateRegistration(string username, string password, string role, string name, string surname, string gender, string mobNo, DateTime birthDate)
        {
            try
            {
                // Check if username already exists
                if (IsUsernameExists(username))
                {
                    MessageBox.Show("Username is already taken. Please choose a different username.");
                    return false;
                }

                // Check username constraint: Username should contain all letters lower case
                if (!IsUsernameValid(username))
                {
                    MessageBox.Show("Username should contain all letters lower case");
                    return false;
                }

                // Check mobile number constraint: exactly 10 digits
                if (!IsMobileValid(mobNo))
                {
                    return false; // The IsMobileValid method will show appropriate messages
                }

                // Check password constraint: at least one capital letter and digits
                if (!IsPasswordValid(password))
                {
                    MessageBox.Show("Password should contain at least one capital letter and at least one digit.");
                    return false;
                }

                // Check if age is less than 18 for the recent date
                DateTime currentDate = DateTime.Now;
                int age = currentDate.Year - birthDate.Year;

                if ((birthDate.Month > currentDate.Month) || (birthDate.Month == currentDate.Month && birthDate.Day > currentDate.Day))
                {
                    age--;
                }

                if (age < 18)
                {
                    MessageBox.Show("Age must be 18 or older for registration.");
                    return false;
                }

                // Perform the registration (insert into the database)
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO users (username, password, role, name, surname, gender, mob_no, birth_date) " +
                                   "VALUES (@Username, @Password, @Role, @Name, @Surname, @Gender, @MobNo, @BirthDate)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Role", role);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Surname", surname);
                        command.Parameters.AddWithValue("@Gender", gender);
                        command.Parameters.AddWithValue("@MobNo", mobNo);
                        command.Parameters.AddWithValue("@BirthDate", birthDate);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Registration successful!");
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Please try again.");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        bool IsMobileValid(string mobile)
        {
            // Exactly 10 digits
            if (!Regex.IsMatch(mobile, @"^\d{10}$"))
            {
                MessageBox.Show("Mobile number should contain exactly 10 digits.");
                return false;
            }

            // Check if mobile number already exists in the users table
            if (IsMobileExists(mobile))
            {
                MessageBox.Show("Mobile number is already registered. Please use a different mobile number.");
                return false;
            }

            return true;
        }

        bool IsMobileExists(string mobile)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM users WHERE mob_no = @Mobile";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Mobile", mobile);

                        int count = (int)command.ExecuteScalar();

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        bool IsUsernameExists(string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM users WHERE username = @Username";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        int count = (int)command.ExecuteScalar();

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        bool IsUsernameValid(string username)
        {
            // Contains only lowercase letters
            return Regex.IsMatch(username, "^[a-z]+$");
        }

        bool IsPasswordValid(string password)
        {
            // At least one capital letter and at least one digit
            return Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d).+$");
        }

        private void btnSignIn_Click_1(object sender, EventArgs e)
        {
            string username = textUsername.Text;
            string password = textPassword.Text;
            string role = comboBoxRole.Text;
            string name = textName.Text;
            string surname = textSurName.Text;
            string gender = comboBoxGender.Text;
            string mobNo = textBoxMob.Text;
            DateTime birthDate = dateTimePicker1.Value;

            if (ValidateRegistration(username, password, role, name, surname, gender, mobNo, birthDate))
            {
                // Registration successful, open the login page or perform other actions
                LoginPage loginPage = new LoginPage();
                loginPage.Show();

                this.Hide(); // Hide the registration form
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
