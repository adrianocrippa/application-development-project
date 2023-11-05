using Assignment2.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Assignment2.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class assignment2Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public assignment2Controller(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllProducts")]

        public Response GetAllProducts()
        {
            Response response = new Response();

            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("productConnection"));

            DBApplication dbA = new DBApplication();
            response = dbA.GetAllProducts(con);

            return response;
        }

        [HttpGet]
        [Route("GetProductbyId/{id}")]

        public Response GetProductbyId(int id)
        {
            Response response = new Response();

            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("productConnection"));

            DBApplication dbA = new DBApplication();
            response = dbA.GetProductbyId(con, id);

            return response;
        }

        [HttpPost] // to update/send something from client machine to the remote machine/server
        [Route("AddProduct")]
        public Response AddProduct(Products product) // we are passing full student information
                                                    // from local client machine to the remote machine/server
        {
            // step 1: Create Response Instance
            Response response = new Response();
            // Step 2: Create the Connection
            NpgsqlConnection con =
                new NpgsqlConnection(_configuration.GetConnectionString("productConnection"));
            // Step 3: Generate the Query and pass it to the Method
            DBApplication dbA = new DBApplication();
            // step 4: Call the Method
            response = dbA.AddProduct(con, product);
            return response;
        }

        [HttpPut] // to update any information in server, we either use put or post request
        [Route("UpdateProduct")]
        public Response UpdateStudent(Products product) // to update the attributes of the student itself
        {
            Response response = new Response();
            NpgsqlConnection con =
                new NpgsqlConnection(_configuration.GetConnectionString("productConnection"));
            DBApplication dbA = new DBApplication();
            response = dbA.UpdateProduct(con, product);
            return response;
        }

        // Delete the student information
        [HttpDelete]
        [Route("DeleteProductbyId/{id}")]
        public Response DeleteProductbyId(int id)
        {
            Response response = new Response();
            NpgsqlConnection con =
                new NpgsqlConnection(_configuration.GetConnectionString("productConnection"));
            DBApplication dbA = new DBApplication();
            response = dbA.DeleteProductbyId(con, id);
            return response;
        }

    }
}
