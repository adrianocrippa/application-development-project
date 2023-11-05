using Npgsql;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Assignment_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();
        }

        //subprocess one - connection adapter

        public static NpgsqlConnection con; //creating object of connection addapter

        //subprocess two - command Adapter/execute query
        public static NpgsqlCommand cmd; //creating object of the command adapter

        private void estabilishConnection()
        {
            /*
             * to estabilsh the connection, we need to install the PostgreSQL, adapter/library
             * from the package manager.
             *
             *
             *adapter name is NpgSql
             */

            // step 1 - connect to the PostgreSQL database
            //substep - generate the connection string
            //substep - create the instances of the Connector and Command adapter
            //substep - estabilish the connection and check/verify

            //We should do exception handling

            try
            {
                con = new NpgsqlConnection(get_ConnectionString());
                MessageBox.Show("Connection Established");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private string get_ConnectionString()
        {
            /*
             * For PostGreSQL connectionstring, we need to pass five values
             * host, port, dbName, userName and Password
             */
            string host = "Host=localhost;";
            string port = "Port=5432;";
            string dbName = "Database=Assignment2;";
            string userName = "Username=postgres;";
            string password = "Password=Tbez71192;";

            string connectionString = string.Format("{0}{1}{2}{3}{4}", host, port, dbName, userName, password);
            return connectionString;

        }

        private void insert_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //step 1
                estabilishConnection();

                //step 2 - open the connection
                con.Open();


                //step 3 - generating the query
                string Query = "INSERT INTO market (productName, productId, amount, price) VALUES (@productName, @productId, @amount, @price)";


                //step 4 - pass the query to Command Adapter
                cmd = new NpgsqlCommand(Query, con); //dynamic memory allocation of the command

                //step 4.1 - add/define values for the variable in the query
                cmd.Parameters.AddWithValue("@productName", productName.Text);
                cmd.Parameters.AddWithValue("@productId", int.Parse(productId.Text));
                cmd.Parameters.AddWithValue("@amount", int.Parse(amount.Text));
                cmd.Parameters.AddWithValue("@price", double.Parse(price.Text));

                //step 5 - execute the command
                cmd.ExecuteNonQuery();

                //step 6 - send a confirmation message
                MessageBox.Show("Product created sucessfully!");

                //step 7 - close the connection
                con.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void select_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //step 1
                estabilishConnection();

                //step 2 - open the connection
                con.Open();

                //step 3 - create the query
                string query = "SELECT * FROM market";

                //step 4 -  innitialize the command adappter with connection
                cmd = new NpgsqlCommand(query, con);

                //for show results
                //step 5 -  we need to create a sqldataAdapter and a sqldatatable, read the data at time of
                //executing select command and set in the data table and the push it to the data adapter to set it back to data grid
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                //step 6 - Send the dataTable information to DataGrid itemSource
                resultsGrid.ItemsSource = dt.AsDataView(); //this line will copy the whole datatable,
                                                        //it gets from the adapter to DataGrid view
                                                        //And with AsDataView() we are ensuring that, DataGrid is getting full data

                //step 7 - Reinitialize our wpf controls data, specially Grid Data
                DataContext = da;

                //step 8 -
                con.Close();

            } catch (NpgsqlException ex) { MessageBox.Show(ex.Message); }
        }

        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //step 1
                estabilishConnection();

                //step 2 - open the connection
                con.Open();

                //ERROR//
                //ERROR//
                //step 3 - create the query
                string query = "DELETE FROM market WHERE productId=@product_Id";

                //step 4 -  innitialize the command adappter with connection
                cmd = new NpgsqlCommand(query, con);

                // Add the parameter for productId
                cmd.Parameters.AddWithValue("@product_Id", int.Parse(productId.Text)); 

                // Execute the DELETE query
                cmd.ExecuteNonQuery();

                //for show results
                //step 5 -  we need to create a sqldataAdapter and a sqldatatable, read the data at time of
                //executing select command and set in the data table and the push it to the data adapter to set it back to data grid
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                //step 6 - Send the dataTable information to DataGrid itemSource
                resultsGrid.ItemsSource = dt.AsDataView(); //this line will copy the whole datatable,
                                                           //it gets from the adapter to DataGrid view
                                                           //And with AsDataView() we are ensuring that, DataGrid is getting full data

                //step 7 - Reinitialize our wpf controls data, specially Grid Data
                DataContext = da;

                //step 8 -
                con.Close();

            }
            catch (NpgsqlException ex) { MessageBox.Show(ex.Message); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Sales salesWindow = new Sales();
            salesWindow.Show();
        }

        private void update_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Step 1 - Establish the database connection
                estabilishConnection();

                // Step 2 - Open the connection
                con.Open();

                // Step 3 - Create the UPDATE query
                string query = "UPDATE market SET productName = @newProductName, amount = @newAmount, price = @newPrice WHERE productId = @productIdToUpdate";

                // Step 4 - Initialize the command adapter with the connection
                cmd = new NpgsqlCommand(query, con);

                // Step 4.1 - Add/define values for the variables in the query
                cmd.Parameters.AddWithValue("@newProductName", productName.Text);
                cmd.Parameters.AddWithValue("@newAmount", int.Parse(amount.Text));
                cmd.Parameters.AddWithValue("@newPrice", double.Parse(price.Text));
                cmd.Parameters.AddWithValue("@productIdToUpdate", int.Parse(productId.Text));

                // Step 5 - Execute the UPDATE command
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Product updated successfully!");
                }
                else
                {
                    MessageBox.Show("No product found with the specified Product ID.");
                }

                // Step 6 - Close the connection
                con.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
