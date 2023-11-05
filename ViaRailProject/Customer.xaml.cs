using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
using Npgsql;

namespace ViaRailProject
{
    public partial class Customer : Window
    {
        private double totalSales = 0;
        private NpgsqlConnection con;
        private NpgsqlCommand cmd;

        public Customer()
        {
            InitializeComponent();
            con = new NpgsqlConnection(get_ConnectionString());
            establishConnection();        
            LoadData();
        }

        //CONECTION
        private void establishConnection()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                    // Connection is now open
                }
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
            string dbName = "Database=App Project;";
            string userName = "Username=postgres;";
            string password = "Password=postgres;";

            string connectionString = string.Format("{0}{1}{2}{3}{4}", host, port, dbName, userName, password);
            return connectionString;
        }

        private void LoadData()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open(); // Open the connection if it's closed
                }
                string query = "SELECT * FROM ticket";
                cmd = new NpgsqlCommand(query, con);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        {
                            int tripid = reader.GetInt32(reader.GetOrdinal("tripid"));
                            string tripNumber = reader["tripnumber"].ToString();
                            string departureStation = reader["departurestation"].ToString();
                            string destinationStation = reader["destinationstation"].ToString();
                            string trainNumber = reader["trainnumber"].ToString();
                            string tripClass = reader["class"].ToString();
                            int seatAvailability = reader.GetInt32(reader.GetOrdinal("seatavailability"));
                            double price = reader.GetDouble(reader.GetOrdinal("price"));

                            // Create a ListBoxItem with ticket details
                            ListBoxItem item = new ListBoxItem();
                            item.Content = $"Trip Number: {tripNumber} // {departureStation} to {destinationStation} // Train: {trainNumber} // Class: {tripClass} // Seat Availability: {seatAvailability} // Price: ${price:0.00}";
                            item.Tag = new TicketDetails(tripid, tripNumber, departureStation, destinationStation, trainNumber, tripClass, seatAvailability, price);
                            tripListBox.Items.Add(item); // Add the ListBoxItem to the ListBox
                        }
                    }
                }
            
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void UpdateInventoryInDatabase(int tripid, int seatavailability)
        {
            try
            {
                using (NpgsqlCommand updateCmd = new NpgsqlCommand("UPDATE ticket SET seatavailability = seatavailability - @seatavailability WHERE tripid = @tripid", con))
                {
                    updateCmd.Parameters.AddWithValue("@seatavailability", seatavailability);
                    updateCmd.Parameters.AddWithValue("@tripid", tripid);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

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
            tripListBox.Items.Clear();
            LoadData();
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            // Iterate through the cart items and update the inventory for each selected ticket
            foreach (ListBoxItem cartItem in cartListBox.Items)
            {
                TicketDetails selectedTicket = (TicketDetails)cartItem.Tag;
                int quantity = selectedTicket.SelectedQuantity;

                if (quantity <= 0 || quantity > selectedTicket.SeatAvailability)
                {
                    MessageBox.Show($"Invalid quantity for {selectedTicket.TripNumber}. Please check the quantity.");
                    return;
                }

                UpdateInventoryInDatabase(selectedTicket.TripID, quantity);
            }

            MessageBox.Show("Checkout completed. Inventory updated.");
            RefreshProductList();
            cartListBox.Items.Clear();
            totalSales = 0;
            totalSalesTextBlock.Text = "$0.00";
        }




        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)tripListBox.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Please select a ticket.");
                return;
            }

            TicketDetails selectedTicket = (TicketDetails)selectedItem.Tag;

            if (!int.TryParse(quantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Invalid quantity.");
                return;
            }

            if (quantity > selectedTicket.SeatAvailability)
            {
                MessageBox.Show("Not enough seats available for this ticket.");
                return;
            }

            double ticketPrice = selectedTicket.Price;
            double ticketSalesAmount = ticketPrice * quantity;

            totalSales += ticketSalesAmount;
            totalSalesTextBlock.Text = "$" + totalSales.ToString("0.00");
            selectedTicket.SelectedQuantity = quantity;

            // Add the selected product to the cartListBox
            ListBoxItem cartItem = new ListBoxItem();
            cartItem.Content = $"{selectedTicket.DepartureStation} to {selectedTicket.DestinationStation} // ID: {selectedTicket.TripID} // Quantity: {quantity} // Total Price: ${totalSales:0.00}";
            cartItem.Tag = selectedTicket;
            cartListBox.Items.Add(cartItem);
        }

        public class TicketDetails
        {
            public int TripID { get; set; }
            public string TripNumber { get; set; }
            public string DepartureStation { get; set; }
            public string DestinationStation { get; set; }
            public string TrainNumber { get; set; }
            public string Class { get; set; }
            public int SeatAvailability { get; set; }
            public double Price { get; set; }
            public int SelectedQuantity { get; set; } // New property for selected quantity

            public TicketDetails(int tripID, string tripNumber, string departureStation, string destinationStation, string trainNumber, string tripClass, int seatAvailability, double price)
            {
                TripID = tripID;
                TripNumber = tripNumber;
                DepartureStation = departureStation;
                DestinationStation = destinationStation;
                TrainNumber = trainNumber;
                Class = tripClass;
                SeatAvailability = seatAvailability;
                Price = price;
            }
        }

        private void BackToMainButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
