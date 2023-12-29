using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Cafe_Management_System
{
    public partial class AddItems : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CafeManagement"].ConnectionString;

        public AddItems()
        {
            InitializeComponent();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Assuming comboBox represents the category, textItem represents the item name,
                    // and textPrice represents the item price.
                    string category = comboBoxCategory.Text;
                    string itemName = textItem.Text;
                    decimal price = Convert.ToDecimal(textPrice.Text);

                    string query = "INSERT INTO items (name, category, price) VALUES (@Name, @Category, @Price)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", itemName);
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@Price", price);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Item added successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            DashboardOwner dashboard = new DashboardOwner();
            dashboard.Show();
        }
    }
}
