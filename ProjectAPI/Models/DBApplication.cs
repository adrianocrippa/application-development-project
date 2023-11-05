using Npgsql;
using System.Data;

namespace ProjectAPI.Models
{
    public class DBApplication
    {
        //create the method to query the database

        public Response GetAllTickets(NpgsqlConnection con) 
        
        {
            //1st thing, fazer a query
            string Query = "SELECT * FROM ticket";

            //2nd step, you will need an adpater
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(Query,con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            //3rd step
            Response response = new Response();
            List<Ticket> tickets = new List<Ticket>();

            if(dt.Rows.Count >0)
            {

                //CONTINUAR DESTE FOR LOOP

                for(int = 0; int<dt.Rows.Count; int++) 
                {
                    Ticket ticket = new Ticket();

                    ticket
                }
            }


            

        }
    }
}
