using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Models;
using Npgsql;

namespace ProjectAPI.Controllers
{

    //giving the name for the API before the controller
    [Route("[controller]")]
    [ApiController]

    public class TicketController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        //TicketController 
        public TicketController(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        //first API wrapper - Get all tickets
        [HttpGet]
        [Route("GetAllTickets")] //Get request

        //we need to create the "GetAllTickets" API method

        public Response GetAllTickets()
        {
            //creating the response obj
            Response response = new Response();

            //create the SQL communication with API
            //we call connectionString

            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("ticketConnection"));

            //need query with command and connection/
            //we create a DB class just for that
            DBApplication dbA = new DBApplication();

            //getting response
            response = dbA.GetAllTickets(con);

            //last step, print
            return response;
        }

        [HttpGet] // because we are going to generate Get request to the database
        [Route("GetTripbyCity/{departurestation}")]
        public Response GetTripByCity(string departurestation)
        {
            // Step 1: Creating the instance of the Response Class
            Response response = new Response();
            // Step 2: Create the Connection for the Database
            NpgsqlConnection con =
                new NpgsqlConnection(_configuration.GetConnectionString("ticketConnection"));
            // Step 3: Creating the query with the Id passed to the system as well as
            // connect to the database and execute the query
            DBApplication dbA = new DBApplication();
            // Step 4: Call the method which is going to search the student by id
            response = dbA.GetTripByCity(con, departurestation);
            // step 5: Return the Response 
            return response;
        }

    }
}
