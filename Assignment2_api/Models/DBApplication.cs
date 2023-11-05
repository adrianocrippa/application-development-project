using Npgsql;
using System.Data;

namespace Assignment2.Models
{
    public class DBApplication
    {
        public Response GetAllProducts(NpgsqlConnection con)
        {
            string Query = "Select * from market";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(Query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            List<Products> products = new List<Products>(); 

            if(dt.Rows.Count>0) 
            {
                for(int i = 0; i<dt.Rows.Count;i++)
                {
                    Products product = new Products();

                    product.productName = (string)dt.Rows[i]["productName"];
                    product.productId = (int)dt.Rows[i]["productId"];
                    product.amount = (int)dt.Rows[i]["amount"];
                    product.price = (double)dt.Rows[i]["price"];

                    products.Add(product);
                }
            }

            if (products.Count >0)
            {
                response.statusCode = 200;
                response.message = "Data Retrieved Sucessfully";
                response.product = null;
                response.products = products;
            }
            else
            {
                response.statusCode = 100;
                response.message = "Data failed ro Retrieve or maybe table is empty";
                response.product = null;
                response.products = null;
            }
            return response;

        }

        public Response GetProductbyId(NpgsqlConnection con, int id)
        {
            // step 1: Configure/ Create Response instance
            Response response = new Response();
            // Generate the Query
            string Query = "Select * from market where productid='" + id + "'"; // inline parameter
                                                                               // with query
                                                                               // Step 3: Create the data adapter
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(Query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Step 4: Need to verify the search query result
            if (dt.Rows.Count > 0) // this condition satisfies whether the data/entry is properly
                                   // searched out from the database
            {
                Products product = new Products();

                product.productName = (string)dt.Rows[0]["productName"];
                product.productId = (int)dt.Rows[0]["productId"];
                product.amount = (int)dt.Rows[0]["amount"];
                product.price = (double)dt.Rows[0]["price"];

                // configure the response message
                response.statusCode = 200;
                response.message = "Successfully Retrieved";
                response.product = product;
                response.products = null;
            }
            else
            {
                response.statusCode = 100;
                response.message = "Data couldn't found.. check the id";
                response.products = null;
                response.product = null;
            }
            // step 5: return the response
            return response;

        }

        public Response AddProduct(NpgsqlConnection con, Products product)
        {
            con.Open();
            Response response = new Response();
            string Query = "INSERT INTO market (productName, productId, amount, price) " +
                "VALUES (@productName, @productId, @amount, @price)";
            // step 3: need the command to execute
            NpgsqlCommand cmd = new NpgsqlCommand(Query, con);
            cmd.Parameters.AddWithValue("@productName", product.productName);
            cmd.Parameters.AddWithValue("@productId", product.productId);
            cmd.Parameters.AddWithValue("@amount", product.amount);
            cmd.Parameters.AddWithValue("@price", product.price);

            int i = cmd.ExecuteNonQuery();

            if (i > 0) // that means, the command is executed successfully
            {
                response.statusCode = 200;
                response.message = "Successfully inserted";
                response.product = product;
                response.products = null;
            }
            else
            {
                response.statusCode = 100;
                response.message = "Insertion is not successfull";
                response.product = null;
                response.products = null;
            }
            con.Close();
            return response;
        }

        public Response UpdateProduct(NpgsqlConnection con, Products product)
        {
            con.Open();
            Response response = new Response();
            string Query = "Update market set productName=@productName, productId=@productId, amount=@amount, price=@price";
            NpgsqlCommand cmd = new NpgsqlCommand(Query, con);
            cmd.Parameters.AddWithValue("@productName", product.productName);
            cmd.Parameters.AddWithValue("@productId", product.productId);
            cmd.Parameters.AddWithValue("@amount", product.amount);
            cmd.Parameters.AddWithValue("@price", product.price);

            int i = cmd.ExecuteNonQuery();

            if (i > 0)
            {
                response.statusCode = 200;
                response.message = "Updated Successfully";
                response.product = product;
               
            }
            else
            {
                response.statusCode = 100;
                response.message = "Update failed or id wasn't in correct form";
            }
            con.Close();
            return response;
        }

        public Response DeleteProductbyId(NpgsqlConnection con, int id)
        {
            con.Open();
            Response response = new Response();
            string Query = "Delete from market where productId='" + id + "'";
            NpgsqlCommand cmd = new NpgsqlCommand(Query, con);

            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                response.statusCode = 200;
                response.message = "Product Delected Successfully";
            }
            else
            {
                response.statusCode = 100;
                response.message = "Product not found!!! Could perform delete Ops";
            }
            con.Close();
            return response;
        }

    }
}
