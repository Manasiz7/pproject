using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Cafe_Management_System
{
    public partial class RemoveItem : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CafeManagement"].ConnectionString;
        public RemoveItem()
        {
            InitializeComponent();
            LoadDataToDataGridView();
            dataGridView.CellClick += dataGridView_CellContentClick;


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textSearchItem_TextChanged(object sender, EventArgs e)
        {
            SearchItemByName(textSearchItem.Text);
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

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView.Rows[e.RowIndex].Selected = true;
            }

        }


        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                // Assuming the primary key column is named "id"
                int selectedItemId = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["id"].Value);

                // Remove the row from the DataGridView
                dataGridView.Rows.RemoveAt(dataGridView.SelectedRows[0].Index);

                // Remove the row from the database
                RemoveItemFromDatabase(selectedItemId);
            }
        }

        private void RemoveItemFromDatabase(int itemId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM items WHERE id = @ItemId";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ItemId", itemId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Item removed successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Item removal failed.");
                    }
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
            loginPage.Show();
        }

        private void back_Click(object sender, EventArgs e)
        {
            DashboardOwner dashboard = new DashboardOwner();
            dashboard.Show();
        }
    }
}
