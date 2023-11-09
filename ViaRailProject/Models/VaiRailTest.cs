using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ViaRailProject.Models
{
    public class VaiRailTest
    {
        public NpgsqlConnection Connection { get; set; }

        public VaiRailTest(string connectionString)
        {
            Connection = new NpgsqlConnection(connectionString);
        }
        public bool VerifyInsertion(string tripnumber, string departurestation, string destinationstation, string trainnumber, string @class, string seatavailability, string price)
        {
            try
            {
                Connection.Open();

                string query = "SELECT * FROM ticket WHERE tripnumber = @tripnumber AND departurestation = @departurestation AND destinationstation = @destinationstation AND trainnumber = @trainnumber AND class = @class AND seatavailability = @seatavailability AND price = @price";

                NpgsqlCommand cmd = new NpgsqlCommand(query, Connection);

                cmd.Parameters.AddWithValue("@tripnumber", int.Parse(tripnumber));
                cmd.Parameters.AddWithValue("@departurestation", departurestation);
                cmd.Parameters.AddWithValue("@destinationstation", destinationstation);
                cmd.Parameters.AddWithValue("@trainnumber", int.Parse(trainnumber));
                cmd.Parameters.AddWithValue("@class", @class);
                cmd.Parameters.AddWithValue("@seatavailability", int.Parse(seatavailability));
                cmd.Parameters.AddWithValue("@price", double.Parse(price));

                NpgsqlDataReader reader = cmd.ExecuteReader();

                bool insertionVerified = reader.HasRows;

                reader.Close();
                Connection.Close();

                return insertionVerified;
            }
            catch (NpgsqlException)
            {
                Connection.Close();
                return false;
            }
        }
    }
}
