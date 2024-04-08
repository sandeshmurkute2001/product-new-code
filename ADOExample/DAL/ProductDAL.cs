using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ADOExample.Models;
using System.Runtime.Remoting.Messaging;


namespace ADOExample.DAL
{
    public class ProductDAL
    {
        string conString = ConfigurationManager.ConnectionStrings["adoconnectionstring"].ToString();

        // Get All Product
        public List<Product> GetAllProducts()
        {
            List<Product> productList = new List<Product>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command= connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetAllProducts";
                SqlDataAdapter sqlDA= new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach(DataRow dr in dtProducts.Rows)
                {
                    productList.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = (dr["ProductName"]).ToString(),
                        Price = Convert.ToInt32(dr["Price"]),
                        Qty = Convert.ToInt32(dr["Qty"]),
                        Remarks = dr["Remarks"].ToString(),
                    });
                }


            }
                return productList;
        }

        // Insert Product
        public bool InsertProduct(Product product)
        {
            int id = 0;
            using (SqlConnection connection= new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_InsertProducts",connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@productName", product.ProductName);
                command.Parameters.AddWithValue("@price", product.Price);
                command.Parameters.AddWithValue("@qty", product.Qty);
                command.Parameters.AddWithValue("@remarks", product.Remarks);

                connection.Open();
                id=command.ExecuteNonQuery();
                connection.Close();
            }
            if(id>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Get All ProductByID
        public List<Product> GetProductsByID(int ProductID)
        {
            List<Product> productList = new List<Product>();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetProductByID";
                command.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    productList.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = (dr["ProductName"]).ToString(),
                        Price = Convert.ToInt32(dr["Price"]),
                        Qty = Convert.ToInt32(dr["Qty"]),
                        Remarks = dr["Remarks"].ToString(),
                    });
                }


            }
            return productList;
        }

        //Update Product
        public bool UpdateProduct(Product product)
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateProducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@productID", product.ProductID);
                command.Parameters.AddWithValue("@productName", product.ProductName);
                command.Parameters.AddWithValue("@price", product.Price);
                command.Parameters.AddWithValue("@qty", product.Qty);
                command.Parameters.AddWithValue("@remarks", product.Remarks);

                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Delete Product
        public string DeleteProduct(int productid)
        {
            string result = "";
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteProduct", connection);
                command.CommandType= CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@productid", productid);
                command.Parameters.Add("@OutputMessage", SqlDbType.VarChar, 50).Direction=ParameterDirection.Output;
                connection.Open();
                command.ExecuteNonQuery();
                result = command.Parameters["@OutputMessage"].Value.ToString();
                connection.Close();
            }
            return result;
        }
    }
}