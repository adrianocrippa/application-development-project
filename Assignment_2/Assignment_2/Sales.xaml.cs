using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Assignment_2
{
    public partial class Sales : Window
    {
        private double totalSales = 0;
        private NpgsqlConnection con;

        public Sales()
        {
            InitializeComponent();
            estabilishConnection(); // Establish the database connection when the window is created.
            LoadProductsFromDatabase(); // Load product names from the database.
        }

        private void estabilishConnection()
        {
            try
            {
                con = new NpgsqlConnection(get_ConnectionString());
                con.Open(); // Open the connection
                MessageBox.Show("Connection Established");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string get_ConnectionString()
        {
            string host = "Host=localhost;";
            string port = "Port=5432;";
            string dbName = "Database=Assignment2;";
            string userName = "Username=postgres;";
            string password = "Password=Tbez71192;";

            string connectionString = string.Format("{0}{1}{2}{3}{4}", host, port, dbName, userName, password);
            return connectionString;
        }

        private void LoadProductsFromDatabase()
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT productname, productid, price, amount FROM market", con))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string productName = reader["productname"].ToString();
                            int productId = reader.GetInt32(reader.GetOrdinal("productid"));
                            double productPrice = reader.GetDouble(reader.GetOrdinal("price"));
                            int productAmount = reader.GetInt32(reader.GetOrdinal("amount"));

                            // Create a ListBoxItem with product details
                            ListBoxItem item = new ListBoxItem();
                            item.Content = $"{productName} // Price: ${productPrice:0.00} // Available Amount(kg): {productAmount}";
                            item.Tag = new ProductDetails(productId, productName, productPrice, productAmount);
                            productListBox.Items.Add(item); // Add the ListBoxItem to the ListBox
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)productListBox.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Please select a product.");
                return;
            }

            ProductDetails selectedProduct = (ProductDetails)selectedItem.Tag;

            if (!int.TryParse(quantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Invalid quantity.");
                return;
            }

            if (quantity > selectedProduct.Amount)
            {
                MessageBox.Show("Not enough stock available for this product.");
                return;
            }

            double productPrice = selectedProduct.Price;

            double productSalesAmount = productPrice * quantity;

            totalSales += productSalesAmount;
            totalSalesTextBlock.Text = "Total Sales: $" + totalSales.ToString("0.00");

            // Update the selected quantity in the ProductDetails
            selectedProduct.SelectedQuantity = quantity;

            // Add the selected product to the cartListBox
            ListBoxItem cartItem = new ListBoxItem();
            cartItem.Content = $"{selectedProduct.ProductName} // Quantity(kg): {quantity} // Total Price: ${productSalesAmount:0.00}";
            cartItem.Tag = selectedProduct;
            cartListBox.Items.Add(cartItem);
        }

        private void UpdateInventoryInDatabase(int productId, int quantity)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE market SET amount = amount - @quantity WHERE productid = @productId", con))
                {
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@productId", productId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("Failed to update inventory.");
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RefreshProductList()
        {
            // Clear and reload the product list with updated information
            productListBox.Items.Clear();
            LoadProductsFromDatabase();
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            // Iterate through the cart items and update the inventory for each selected product
            foreach (ListBoxItem cartItem in cartListBox.Items)
            {
                ProductDetails selectedProduct = (ProductDetails)cartItem.Tag;
                int quantity = selectedProduct.SelectedQuantity;

                if (quantity <= 0 || quantity > selectedProduct.Amount)
                {
                    MessageBox.Show($"Invalid quantity for {selectedProduct.ProductName}. Please check the quantity.");
                    return;
                }

                UpdateInventoryInDatabase(selectedProduct.ProductId, quantity);
            }

            MessageBox.Show("Checkout completed. Inventory updated.");
            RefreshProductList();
            cartListBox.Items.Clear(); // Clear the cart after checkout
            totalSales = 0; // Reset total sales
            totalSalesTextBlock.Text = "Total Sales: $0.00";
        }
    }

    // Create a class to store product details
    public class ProductDetails
    {
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public double Price { get; private set; }
        public int Amount { get; private set; }
        public int SelectedQuantity { get; set; } // New property for selected quantity

        public ProductDetails(int productId, string productName, double price, int amount)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Amount = amount;
            SelectedQuantity = 0; // Initialize selected quantity to zero
        }
    }
}
