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
    public class AdminForTesting
    {
        public string TripNumber { get; set; }
        public string DepartureStation { get; set; }
        public string DestinationStation { get; set; }
        public string TrainNumber { get; set; }
        public string Class { get; set; }
        public string SeatAvailability { get; set; }
        public string Price { get; set; }

        public void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            // Your testing logic here
        }
    }
}
