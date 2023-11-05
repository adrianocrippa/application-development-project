namespace ProjectAPI.Models
{
    public class Response
    {
        //response status with a code
        public int statusCode {  get; set; }

        //response have a message
        public string message { get; set; }

        //one reponse
        public Ticket ticket { get; set; } 

        //list of responses
        public List<Ticket> tickets { get; set; }   
    }
}
