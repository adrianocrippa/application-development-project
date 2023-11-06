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
                for(int i = 0; i<dt.Rows.Count; i++) 
                {
                    Ticket ticket = new Ticket();

                    ticket.tripnumber = (int)dt.Rows[i]["tripnumber"];
                    ticket.departurestation = (string)dt.Rows[i]["departurestation"];
                    ticket.destinationstation = (string)dt.Rows[i]["destinationstation"];
                    ticket.trainnumber = (int)dt.Rows[i]["trainnumber"];
                    ticket.ticket_class = (string)dt.Rows[i]["class"]; //check error
                    ticket.seatavailability = (int)dt.Rows[i]["seatavailability"];
                    ticket.price = Convert.ToDouble(dt.Rows[i]["price"]);

                    tickets.Add(ticket);

                }
            }

            if(tickets.Count>0)
            {
                response.statusCode = 200;
                response.message = "Data Retrieved Succesfully";
                response.ticket = null;
                response.tickets = tickets;
            }
            else
            {
                response.statusCode = 100;
                response.message = "Data failed to Retrieve or may table is empty";
                response.ticket = null;
                response.tickets = null;
            }
            return response;
        }

        public Response GetTripByCity(NpgsqlConnection con, string departurestation)
        {
            Response response = new Response();

            try
            {
                string selectQuery = "SELECT * FROM ticket WHERE departurestation ILIKE @departurestation || '%'";
                using (NpgsqlCommand cmd = new NpgsqlCommand(selectQuery, con))
                {
                    cmd.Parameters.AddWithValue("@departurestation", departurestation);

                    using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        List<Ticket> tickets = new List<Ticket>();
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Ticket ticket = new Ticket();

                                ticket.tripnumber = (int)dt.Rows[i]["tripnumber"];
                                ticket.departurestation = (string)dt.Rows[i]["departurestation"];
                                ticket.destinationstation = (string)dt.Rows[i]["destinationstation"];
                                ticket.trainnumber = (int)dt.Rows[i]["trainnumber"];
                                ticket.ticket_class = (string)dt.Rows[i]["class"];
                                ticket.seatavailability = (int)dt.Rows[i]["seatavailability"];
                                ticket.price = Convert.ToDouble(dt.Rows[i]["price"]);

                                tickets.Add(ticket);

                            }
                        }

                        if (tickets.Count > 0)
                        {
                            response.statusCode = 200;
                            response.message = "Data Retrieved Succesfully";
                            response.ticket = null;
                            response.tickets = tickets;
                        }
                        else
                        {
                            response.statusCode = 100;
                            response.message = "Data failed to Retrieve or may table is empty";
                            response.ticket = null;
                            response.tickets = null;
                        }
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 500; // Internal Server Error
                response.message = "An error occurred: " + ex.Message;
                response.ticket = null;
                response.tickets = null;
            }

            return response;
        }

    }
}
