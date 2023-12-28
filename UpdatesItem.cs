using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Cafe_Management_System
{
    public partial class UpdatesItem : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CafeManagement"].ConnectionString;

        public UpdatesItem()
        {
            InitializeComponent();
            LoadDataToDataGridView();
            dataGridView.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void LoadDataToDataGridView()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM items";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textSearchItemName_TextChanged(object sender, EventArgs e)
        {
            SearchItemByName(textSearchItemName.Text);
        }
        private void SearchItemByName(string itemName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM items WHERE name LIKE @ItemName";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ItemName", "%" + itemName + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView.SelectedRows[0];

                // Assuming "name," "price," and "category" are the column names
                string itemName = selectedRow.Cells["name"].Value.ToString();
                string itemPrice = selectedRow.Cells["price"].Value.ToString();
                string itemCategory = selectedRow.Cells["category"].Value.ToString();

                textItemName.Text = itemName;
                textPrice.Text = itemPrice;
                textCategory.Text = itemCategory;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Update the item in the database based on the current values in the text boxes
            string itemName = textItemName.Text;
            string itemCategory = textCategory.Text;
            string itemPrice = textPrice.Text;

            UpdateItemInDatabase(itemName, itemCategory, itemPrice);

            // Refresh the DataGridView to reflect the changes
            LoadDataToDataGridView();
        }


        private void UpdateItemInDatabase(string itemName, string itemCategory, string itemPrice)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE items SET category = @Category, price = @Price WHERE name = @ItemName";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Category", itemCategory);
                    cmd.Parameters.AddWithValue("@Price", itemPrice);
                    cmd.Parameters.AddWithValue("@ItemName", itemName);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.ShowDialog();
        }

        private void back_Click(object sender, EventArgs e)
        {
            DashboardOwner dashboard = new DashboardOwner();
            dashboard.ShowDialog();
        }
    }
}
