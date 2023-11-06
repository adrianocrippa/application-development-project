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
    }
}
