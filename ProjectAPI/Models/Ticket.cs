namespace ProjectAPI.Models
{
    public class Ticket
    {

        public int tripnumber { get; set; }
        
        public string departurestation { get; set; }

        public string destinationstation { get; set; }

        public int trainnumber { get; set; }    

        public string ticket_class { get; set; }

        public int seatavailability {  get; set; }
        public double price {  get; set; }
    }
}
