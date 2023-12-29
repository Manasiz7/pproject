using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Cafe_Management_System
{
    public partial class PlaceOrder1 : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CafeManagement"].ConnectionString;
        private decimal totalCartPrice = 0;
        private DataGridViewRow selectedRow;
        private string customerName;
        public PlaceOrder1()
        {
            InitializeComponent();
            InitializeComboBox(); // Call the function to initialize the combo box
            InitializeDataGridView();
            CultureInfo indianCulture = new CultureInfo("en-IN");
            Thread.CurrentThread.CurrentCulture = indianCulture;
            Thread.CurrentThread.CurrentUICulture = indianCulture;

        }

        private void PlaceOrder_Load(object sender, EventArgs e)
        {
            dataGridView.RowHeadersVisible = false;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Handle the event as needed
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                dataGridView.Rows[e.RowIndex].Selected = true;
            }
        }

        private void InitializeComboBox()
        {
            // Populate the ComboBox with categories from the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT DISTINCT Category FROM Items";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxCategory.Items.Add(reader["Category"].ToString());
                        }
                    }
                }
            }
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxItems.Items.Clear();

            // Get the selected category from the ComboBox
            if (comboBoxCategory.SelectedItem != null)
            {
                string selectedCategory = comboBoxCategory.SelectedItem.ToString();

                // Fetch item names from the database based on the selected category
                PopulateListBoxWithItems(selectedCategory);
            }
        }

        private void listBoxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxItems.SelectedItem != null)
            {
                string selectedItem = listBoxItems.SelectedItem.ToString();
                numericUpDown.Value = 0;
                textTotal.Text = string.Empty;
                DisplayItemDetails(selectedItem);
                DisplayItemDetails(selectedItem);
            }
        }

        private void DisplayItemDetails(string itemName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Query to get item details (name and price) based on the selected item name
                string query = "SELECT Name, Price FROM Items WHERE Name = @Name";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", itemName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Display the item details in text boxes
                            textItemname.Text = reader["Name"].ToString();
                            textPrice.Text = reader["Price"].ToString();
                        }
                    }
                }
            }
        }

        private void PopulateListBoxWithItems(string category)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Query to get item names in the selected category
                string query = "SELECT Name FROM Items WHERE Category = @Category";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Category", category);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listBoxItems.Items.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
        }

        private void textItemName_TextChanged(object sender, EventArgs e)
        {

        }

        private void textTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice();
        }
        private void CalculateTotalPrice()
        {
            // Check if textPrice is not empty and is a valid decimal
            if (!string.IsNullOrEmpty(textPrice.Text) && decimal.TryParse(textPrice.Text, out decimal itemPrice) && numericUpDown.Value != null && numericUpDown.Value > 0)
            {
                // Calculate the total price and display it in textTotal
                decimal total = itemPrice * numericUpDown.Value;

                // Use decimal.TryParse for formatting
                if (decimal.TryParse(total.ToString(), out decimal formattedTotal))
                {
                    textTotal.Text = formattedTotal.ToString("C"); // Format as currency
                }
                else
                {
                    // Handle the case where formatting fails
                    textTotal.Text = "Error";
                }
            }
        }


        private void InitializeDataGridView()
        {
            // Set up the DataGridView columns
            dataGridView.Columns.Add("ItemName", "Item Name");
            dataGridView.Columns.Add("UnitPrice", "Unit Price");
            dataGridView.Columns.Add("Quantity", "Quantity");
            dataGridView.Columns.Add("TotalPrice", "Total Price");

            // Set the default cell style to have a white background
            dataGridView.DefaultCellStyle.BackColor = Color.White;
        }


        // Inside the btnAddToCart_Click method
        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            string itemName = textItemname.Text;
            decimal unitPrice, total;
            int quantity;

            if (decimal.TryParse(textPrice.Text, out unitPrice) && int.TryParse(numericUpDown.Value.ToString(), out quantity))
            {
                if (quantity > 0)
                {
                    // Check if the item is already in the cart
                    DataGridViewRow existingRow = dataGridView.Rows
                        .Cast<DataGridViewRow>()
                        .FirstOrDefault(row => row.Cells["ItemName"].Value?.ToString() == itemName);

                    if (existingRow != null)
                    {
                        // If the item is in the cart, update the quantity and total
                        int existingQuantity = Convert.ToInt32(existingRow.Cells["Quantity"].Value);
                        existingRow.Cells["Quantity"].Value = existingQuantity + quantity;

                        decimal existingTotal;
                        if (decimal.TryParse(existingRow.Cells["TotalPrice"].Value?.ToString(), NumberStyles.Currency, new CultureInfo("en-IN"), out existingTotal))
                        {
                            existingRow.Cells["TotalPrice"].Value = (existingTotal + unitPrice * quantity);
                        }
                        else
                        {
                            // Handle the case where the conversion fails
                            MessageBox.Show("Invalid existing total price format. Please check your data.");
                        }
                    }
                    else
                    {
                        // If the item is not in the cart, add a new row
                        total = unitPrice * quantity;
                        dataGridView.Rows.Add(itemName, unitPrice.ToString("C", new CultureInfo("en-IN")), quantity, total.ToString("C", new CultureInfo("en-IN")));

                        // Update the total cart price
                        totalCartPrice += total;
                    }

                    // Update the total price using the new totalCartPrice
                    UpdateTotalPrice(totalCartPrice);

                    // Update the total price in the textBoxTotalPrice
                    textBoxTotalPrice.Text = totalCartPrice.ToString("C", new CultureInfo("en-IN"));
                }
                else
                {
                    MessageBox.Show("Quantity should be greater than zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid quantity or unit price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void UpdateTotalPrice(decimal change)
        {
            // Validate if the text in textBoxTotalPrice is a valid decimal
            if (decimal.TryParse(textBoxTotalPrice.Text, NumberStyles.Currency, new CultureInfo("en-IN"), out decimal currentTotal))
            {
                decimal newTotal = currentTotal + change;

                // Check if the new total is negative, set to zero in that case
                if (newTotal < 0)
                {
                    textBoxTotalPrice.Text = "₹0.00";
                }
                else
                {
                    textBoxTotalPrice.Text = newTotal.ToString("C", new CultureInfo("en-IN")); // Display the new total as currency
                }
            }
            else
            {
                // Handle the case where the text is not a valid decimal
                MessageBox.Show("Invalid total price format. Please enter a valid number.");
                textBoxTotalPrice.Text = "₹0.00"; // Set a default value or handle it as appropriate
            }
        }


        

        private void textItemname_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            string searchInput = textSearch.Text.Trim();

            if (!string.IsNullOrEmpty(searchInput))
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "SELECT name FROM items WHERE LOWER(name) LIKE @SearchInput";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@SearchInput", "%" + searchInput.ToLower() + "%");

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                listBoxItems.Items.Clear();

                                while (reader.Read())
                                {
                                    string itemName = reader["name"].ToString();
                                    listBoxItems.Items.Add(itemName);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                // Clear the ListBox if the search input is empty
                listBoxItems.Items.Clear();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                // Get the total price of the selected row
                decimal removedItemTotal;
                if (decimal.TryParse(dataGridView.SelectedRows[0].Cells["TotalPrice"].Value.ToString(), NumberStyles.Currency, new CultureInfo("en-IN"), out removedItemTotal))
                {
                    // Remove the row from the DataGridView
                    dataGridView.Rows.RemoveAt(dataGridView.SelectedRows[0].Index);
                    // Update the total price after removing an item from the cart
                    UpdateTotalPrice(-removedItemTotal);
                }
                else
                {
                    MessageBox.Show("Invalid total price format. Please check your data.");
                }
            }
        }









        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            // Update the selectedRow when the selection changes
            if (dataGridView.SelectedRows.Count > 0)
            {
                selectedRow = dataGridView.SelectedRows[0];
            }
            else
            {
                selectedRow = null;
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            DashboardOwner dashboard = new DashboardOwner();
            dashboard.ShowDialog();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.ShowDialog();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Ask the user for their name
            using (var inputDialog = new Form())
            using (var textBox = new TextBox())
            using (var okButton = new Button())
            {
                inputDialog.Text = "Enter Customer Name";
                textBox.Dock = DockStyle.Top;
                okButton.Dock = DockStyle.Bottom;
                okButton.Text = "OK";

                okButton.Click += (s, ev) => inputDialog.DialogResult = DialogResult.OK;
                inputDialog.AcceptButton = okButton;

                inputDialog.Controls.Add(textBox);
                inputDialog.Controls.Add(okButton);

                DialogResult result = inputDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    customerName = textBox.Text;

                    // Your printing logic here...
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler((s, ev) => PrintPage(s, ev, customerName)); // Pass customerName to PrintPage
                    PrintDialog printDialog = new PrintDialog();

                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            pd.Print();
                            MessageBox.Show("Receipt generated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs e, string customerName)
        {
            Graphics g = e.Graphics;

            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;

            Font headerFont = new Font(dataGridView.Font.FontFamily, dataGridView.Font.Size + 2, FontStyle.Bold);
            Brush headerBrush = Brushes.Black;  // Use black brush for column headers
            Brush dateBrush = Brushes.Blue;    // Use blue brush for date
            Brush cellBrush = Brushes.Black;   // Use black brush for cell text
            float columnSpacing = 20;

            string currentDate = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
            g.DrawString($"Date: {currentDate}", new Font(dataGridView.Font, FontStyle.Bold), dateBrush, x, y);
            y += 20;

            g.DrawString($"Customer: {customerName}", new Font(dataGridView.Font, FontStyle.Bold), Brushes.Black, x, y);
            y += 20;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                // Draw column headers with black brush and bold font
                g.DrawString(column.HeaderText, new Font(headerFont.FontFamily, headerFont.Size, FontStyle.Bold), headerBrush, x, y);
                x += column.Width + columnSpacing;
            }

            x = e.MarginBounds.Left;
            y += dataGridView.ColumnHeadersHeight;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    string cellValue = GetFormattedCellValue(cell);
                    Brush currentCellBrush = cell.OwningColumn.HeaderText.Equals("Total Price") ? Brushes.Blue : cellBrush;  // Use blue brush for "Total Price" cell
                    g.DrawString(cellValue, new Font(dataGridView.Font, FontStyle.Bold), currentCellBrush, x, y);
                    x += cell.Size.Width + columnSpacing;
                }

                x = e.MarginBounds.Left;
                y += row.Height;

                if (y + row.Height > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            // Draw the Total Amount with blue brush, bold font, and a larger size
            decimal totalAmount = CalculateTotalAmount();
            string formattedTotalAmount = FormatCurrency(totalAmount);

            try
            {
                Font totalAmountFont = new Font(dataGridView.Font.FontFamily, dataGridView.Font.Size + 4, FontStyle.Bold);  // Larger font size
                g.DrawString($"Total Amount: {formattedTotalAmount}", totalAmountFont, Brushes.Blue, x, y);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error drawing total amount: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FormatCurrency(decimal amount)
        {
            try
            {
                return amount.ToString("C", new CultureInfo("en-IN"));
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        private string GetFormattedCellValue(DataGridViewCell cell)
        {
            if (cell.Value != null)
            {
                try
                {
                    return Convert.ToString(cell.Value);
                }
                catch (FormatException)
                {
                    // Handle the format exception (e.g., for non-string cell values)
                    return "N/A";
                }
            }
            else
            {
                return string.Empty;
            }
        }

        private decimal CalculateTotalAmount()
        {
            decimal totalAmount = 0;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells["TotalPrice"].Value != null)
                {
                    if (decimal.TryParse(row.Cells["TotalPrice"].Value.ToString(), NumberStyles.Currency, new CultureInfo("en-IN"), out decimal rowTotal))
                    {
                        totalAmount += rowTotal;
                    }
                    else
                    {
                        // Log the problematic value and row index
                        Console.WriteLine($"Error parsing total price for row {row.Index}: {row.Cells["TotalPrice"].Value}");
                        MessageBox.Show($"Error parsing total price for row {row.Index}: {row.Cells["TotalPrice"].Value}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            return totalAmount;
        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

