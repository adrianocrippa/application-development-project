using System;
using System.Collections.Generic;
using System.Data;
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
using Npgsql;

namespace ViaRailProject
{
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();
            establishConnection();
            LoadData();

        }

        //CONECTION

        public static NpgsqlConnection con;
        public static NpgsqlCommand cmd;
        private void establishConnection()

        {
            try
            {
                con = new NpgsqlConnection(get_ConnectionString());
                //MessageBox.Show("Connection Established");
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
                string query = "SELECT * FROM ticket";

                cmd = new NpgsqlCommand(query, con);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                resultsGrid.ItemsSource = dt.AsDataView();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                establishConnection();
                con.Open();

                string Query = "INSERT INTO ticket (tripnumber, departurestation, destinationstation, trainnumber, class, seatavailability, price) VALUES " +
                    "(@tripnumber, @departurestation, @destinationstation, @trainnumber, @class, @seatavailability, @price)";

                cmd = new NpgsqlCommand(Query, con);

                if (string.IsNullOrWhiteSpace(tripnumber.Text) ||
                        string.IsNullOrWhiteSpace(departurestation.Text) ||
                        string.IsNullOrWhiteSpace(destinationstation.Text) ||
                        string.IsNullOrWhiteSpace(trainnumber.Text) ||
                        string.IsNullOrWhiteSpace(@class.Text) ||
                        string.IsNullOrWhiteSpace(seatavailability.Text) ||
                        string.IsNullOrWhiteSpace(price.Text))
                {
                    MessageBox.Show("Please fill all the fields before inserting.");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@tripnumber", int.Parse(tripnumber.Text));
                    cmd.Parameters.AddWithValue("@departurestation", departurestation.Text);
                    cmd.Parameters.AddWithValue("@destinationstation", destinationstation.Text);
                    cmd.Parameters.AddWithValue("@trainnumber", int.Parse(trainnumber.Text));
                    cmd.Parameters.AddWithValue("@class", @class.Text);
                    cmd.Parameters.AddWithValue("@seatavailability", int.Parse(seatavailability.Text));
                    cmd.Parameters.AddWithValue("@price", double.Parse(price.Text));


                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Trip created sucessfully!");

                    LoadData();
                }
                con.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                establishConnection();
                con.Open();

                string tableName = "ticket";
                string columnName = "tripid";

                // Check if the tripidtomod TextBox is empty
                if (string.IsNullOrWhiteSpace(tripidtomod.Text))
                {
                    MessageBox.Show("No Trip ID was entered!");
                }
                else
                {
                    int tripIdToModify = int.Parse(tripidtomod.Text);

                    // Check if the tripID exists in the database
                    string checkQuery = $"SELECT COUNT(*) FROM {tableName} WHERE {columnName} = @tripid";
                    NpgsqlCommand checkCmd = new NpgsqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@tripid", tripIdToModify);

                    int tripCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (tripCount == 0)
                    {
                        MessageBox.Show("No Trip was found with the specified Trip ID.");
                    }
                    else
                    {
                        string query = $"UPDATE {tableName} SET ";
                        NpgsqlCommand updateCmd = new NpgsqlCommand(query, con);

                        string[] columnNames = { "tripnumber", "departurestation", "destinationstation", "trainnumber", "class", "seatavailability", "price" };
                        TextBox[] textBoxes = { tripnumber, departurestation, destinationstation, trainnumber, @class, seatavailability, price };

                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(textBoxes[i].Text))
                            {
                                query += $"{columnNames[i]} = @{columnNames[i]}, ";

                                switch (columnNames[i])
                                {
                                    case "price":
                                        if (decimal.TryParse(textBoxes[i].Text, out decimal priceValue))
                                        {
                                            updateCmd.Parameters.AddWithValue($"@{columnNames[i]}", priceValue);
                                        }
                                        else
                                        {
                                            MessageBox.Show($"Invalid input format for {columnNames[i]}.");
                                        }
                                        break;
                                    case "tripnumber":
                                    case "trainnumber":
                                    case "seatavailability":
                                        if (int.TryParse(textBoxes[i].Text, out int intValue))
                                        {
                                            updateCmd.Parameters.AddWithValue($"@{columnNames[i]}", intValue);
                                        }
                                        else
                                        {
                                            MessageBox.Show($"Invalid input format for {columnNames[i]}.");
                                        }
                                        break;
                                    default:
                                        updateCmd.Parameters.AddWithValue($"@{columnNames[i]}", textBoxes[i].Text);
                                        break;
                                }
                            }
                        }
                        query = query.TrimEnd(',', ' ');

                        query += $" WHERE {columnName} = @tripid";

                        updateCmd.Parameters.AddWithValue("@tripid", tripIdToModify);

                        updateCmd.CommandText = query;

                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            LoadData();
                            MessageBox.Show("Trip updated successfully!");
                        }
                    }
                }
                con.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid input format. Please check your inputs.");
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                establishConnection();
                con.Open();

                string tableName = "ticket";
                string columnName = "tripid";

                // Check if the tripidtomod TextBox is empty
                if (string.IsNullOrWhiteSpace(tripidtomod.Text))
                {
                    // If it's empty, select all records
                    string query = $"SELECT * FROM {tableName}";
                    cmd = new NpgsqlCommand(query, con);

                    MessageBox.Show("No Trip ID was entered!");
                    con.Close();
                }
                else
                {
                    // If not empty, select records based on the entered tripid
                    int tripIdToModify = int.Parse(tripidtomod.Text);
                    string query = $"SELECT * FROM {tableName} WHERE {columnName}=@tripid";
                    cmd = new NpgsqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@tripid", tripIdToModify);

                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    resultsGrid.ItemsSource = dt.AsDataView();

                    if (dt.Rows.Count == 0)
                    {
                        LoadData();
                        MessageBox.Show("No Trip was found with the specified Trip ID.");
                    }
                    else
                    {
                        MessageBox.Show("Trip selected successfully!");
                    }
                }
                con.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                establishConnection();
                con.Open();

                string tableName = "ticket";
                string columnName = "tripid";

                // Check if the tripidtomod TextBox is empty
                if (string.IsNullOrWhiteSpace(tripidtomod.Text))
                {
                    // If it's empty, select all records
                    string query = $"SELECT * FROM {tableName}";
                    cmd = new NpgsqlCommand(query, con);

                    MessageBox.Show("No Trip ID was entered!");
                    con.Close();
                }
                else
                {
                    int tripIdToModify = int.Parse(tripidtomod.Text);
                    string query = $"DELETE FROM {tableName} WHERE {columnName}=@tripid";
                    cmd = new NpgsqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@tripid", tripIdToModify);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Trip deleted sucessfully!");

                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    resultsGrid.ItemsSource = dt.AsDataView();

                    if (dt.Rows.Count == 0)
                    {
                        LoadData();
                        MessageBox.Show("No Trip was found with the specified Trip ID.");
                    }
                    else
                    {
                        MessageBox.Show("Trip deleted successfully!");
                    }
                }
                con.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
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



