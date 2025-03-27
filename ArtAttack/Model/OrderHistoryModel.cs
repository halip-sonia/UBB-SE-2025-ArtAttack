using ArtAttack.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;

namespace ArtAttack.Model
{
    public class OrderHistoryModel
    {
        private readonly string _connectionString;

        public OrderHistoryModel(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<DummyProduct> GetDummyProductsFromOrderHistory(int orderHistoryID)
        {
            List<DummyProduct> products = new List<DummyProduct>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetDummyProductsFromOrderHistory", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderHistory", orderHistoryID);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DummyProduct product = new DummyProduct()
                            {
                                
                                ID = reader.GetInt32(reader.GetOrdinal("productID")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Price = (float)reader.GetDouble(reader.GetOrdinal("price")),
                                SellerID = reader["SellerID"] != DBNull.Value ? (int?)reader.GetInt32(reader.GetOrdinal("SellerID")) : null,
                                ProductType = reader.GetString(reader.GetOrdinal("productType")),
                                StartDate = reader["startDate"] != DBNull.Value ? (DateTime?)reader.GetDateTime(reader.GetOrdinal("startDate")) : null,
                                EndDate = reader["endDate"] != DBNull.Value ? (DateTime?)reader.GetDateTime(reader.GetOrdinal("endDate")) : null
                            };
                            products.Add(product);
                        }
                    }
                }
            }

            return products;
        }
    }
}
