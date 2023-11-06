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
using Npgsql;
using NpgsqlTypes;

namespace ViaRailProject
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void login_button_Click(object sender, RoutedEventArgs e)
        {

            // get user and passw
            string usernametoCheck = username.Text;
            string passwordtoCheck = password.Text;

            // check user and password
            if (IsUserAuthenticated(usernametoCheck, passwordtoCheck))
            {
                // auth success
                MessageBox.Show("Login successful!");

                Admin adminWindow = new Admin();
                adminWindow.Show();
                this.Close();
            }
            else
            {
                // failed auth
                MessageBox.Show("Username or Password incorrect. Please try again");
            }

           //
        }

        // function to check auth
        

        
        private bool IsUserAuthenticated(string username, string password)
        {
            string connectionString = "Host=localhost;Port=5432;Database=App Project;Username=postgres;Password=postgres;";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM user2 WHERE nameuser = @username AND password = @password";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    // Parâmetros para evitar SQL Injection
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    int result =Convert.ToInt32(command.ExecuteScalar());

                    return result > 0;
                }
            }
        }


    }
      
}
