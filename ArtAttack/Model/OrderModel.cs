
using Microsoft.Data.SqlClient;
using ArtAttack.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ArtAttack.Model
{
    public class OrderModel
    {
        private readonly string _connectionString;
        public string ConnectionString => _connectionString;

        public OrderModel(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddOrder(int productId, int buyerId, int productType, string paymentMethod, int orderSummaryId, DateTime orderDate)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddOrder", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.Parameters.AddWithValue("@BuyerID", buyerId);
                    cmd.Parameters.AddWithValue("@ProductType", productType);
                    cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                    cmd.Parameters.AddWithValue("@OrderSummaryID", orderSummaryId);
                    cmd.Parameters.AddWithValue("@OrderDate", orderDate);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateOrder(int orderId, int productType, string paymentMethod, DateTime orderDate)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateOrder", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    cmd.Parameters.AddWithValue("@ProductType", productType);
                    cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                    cmd.Parameters.AddWithValue("@OrderDate", orderDate);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOrder(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteOrder", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Order> GetBorrowedOrderHistory(int buyerId)
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("get_borrowed_order_history", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@BuyerID", SqlDbType.Int).Value = buyerId;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order()
                            {
                                OrderID = reader.GetInt32("OrderID"),

                                ProductID = reader.GetInt32("ProductID"),
                                BuyerID = reader.GetInt32("BuyerID"),
                                OrderSummaryID = reader.GetInt32("OrderSummaryID"),
                                OrderHistoryID = reader.GetInt32("OrderHistoryID"),
                                ProductType = reader.GetInt32("ProductType"),
                                PaymentMethod = reader.GetString("PaymentMethod"),
                                OrderDate = reader.GetDateTime("OrderDate")

                            };
                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }

        public List<Order> GetNewOrUsedOrderHistory(int buyerId)
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("get_new_or_used_order_history", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@BuyerID", SqlDbType.Int).Value = buyerId;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order()
                            {
                                OrderID = reader.GetInt32("OrderID"),

                                ProductID = reader.GetInt32("ProductID"),
                                BuyerID = reader.GetInt32("BuyerID"),
                                OrderSummaryID = reader.GetInt32("OrderSummaryID"),
                                OrderHistoryID = reader.GetInt32("OrderHistoryID"),
                                ProductType = reader.GetInt32("ProductType"),
                                PaymentMethod = reader.GetString("PaymentMethod"),
                                OrderDate = reader.GetDateTime("OrderDate")
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }


        public List<Order> GetOrdersFromLastThreeMonths(int buyerId)
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("get_orders_from_last_3_months", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@BuyerID", SqlDbType.Int).Value = buyerId;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order()
                            {
                                OrderID = reader.GetInt32("OrderID"),

                                ProductID = reader.GetInt32("ProductID"),
                                BuyerID = reader.GetInt32("BuyerID"),
                                OrderSummaryID = reader.GetInt32("OrderSummaryID"),
                                OrderHistoryID = reader.GetInt32("OrderHistoryID"),
                                ProductType = reader.GetInt32("ProductType"),
                                PaymentMethod = reader.GetString("PaymentMethod"),
                                OrderDate = reader.GetDateTime("OrderDate")
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }

        public List<Order> GetOrdersFromLastSixMonths(int buyerId)
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("get_orders_from_last_6_months", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@BuyerID", SqlDbType.Int).Value = buyerId;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order()
                            {
                                OrderID = reader.GetInt32("OrderID"),

                                ProductID = reader.GetInt32("ProductID"),
                                BuyerID = reader.GetInt32("BuyerID"),
                                OrderSummaryID = reader.GetInt32("OrderSummaryID"),
                                OrderHistoryID = reader.GetInt32("OrderHistoryID"),
                                ProductType = reader.GetInt32("ProductType"),
                                PaymentMethod = reader.GetString("PaymentMethod"),
                                OrderDate = reader.GetDateTime("OrderDate")
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }

        public List<Order> GetOrdersFrom2025(int buyerId)
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("get_orders_from_2025", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@BuyerID", SqlDbType.Int).Value = buyerId;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order()
                            {
                                OrderID = reader.GetInt32("OrderID"),

                                ProductID = reader.GetInt32("ProductID"),
                                BuyerID = reader.GetInt32("BuyerID"),
                                OrderSummaryID = reader.GetInt32("OrderSummaryID"),
                                OrderHistoryID = reader.GetInt32("OrderHistoryID"),
                                ProductType = reader.GetInt32("ProductType"),
                                PaymentMethod = reader.GetString("PaymentMethod"),
                                OrderDate = reader.GetDateTime("OrderDate")
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }

        public List<Order> GetOrdersFrom2024(int buyerId)
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("get_orders_from_2024", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@BuyerID", SqlDbType.Int).Value = buyerId;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order()
                            {
                                OrderID = reader.GetInt32("OrderID"),

                                ProductID = reader.GetInt32("ProductID"),
                                BuyerID = reader.GetInt32("BuyerID"),
                                OrderSummaryID = reader.GetInt32("OrderSummaryID"),
                                OrderHistoryID = reader.GetInt32("OrderHistoryID"),
                                ProductType = reader.GetInt32("ProductType"),
                                PaymentMethod = reader.GetString("PaymentMethod"),
                                OrderDate = reader.GetDateTime("OrderDate")
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }
        public List<Order> GetOrdersByName(int buyerId, string text)
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("get_orders_by_name", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@BuyerID", SqlDbType.Int).Value = buyerId;
                    cmd.Parameters.Add("@text", SqlDbType.NVarChar, 250).Value = text;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order()
                            {
                                OrderID = reader.GetInt32("OrderID"),
                                ProductID = reader.GetInt32("ProductID"),


                                BuyerID = reader.GetInt32("BuyerID"),
                                OrderSummaryID = reader.GetInt32("OrderSummaryID"),
                                OrderHistoryID = reader.GetInt32("OrderHistoryID"),
                                ProductType = reader.GetInt32("ProductType"),
                                PaymentMethod = reader.GetString("PaymentMethod"),
                                OrderDate = reader.GetDateTime("OrderDate")
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }

        public List<Order> GetOrdersFromOrderHistory(int orderHistoryId)
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("get_orders_from_order_history", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@OrderHistoryID", SqlDbType.Int).Value = orderHistoryId;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order()
                            {
                                OrderID = reader.GetInt32("OrderID"),

                                ProductID = reader.GetInt32("ProductID"),
                                BuyerID = reader.GetInt32("BuyerID"),
                                OrderSummaryID = reader.GetInt32("OrderSummaryID"),
                                OrderHistoryID = reader.GetInt32("OrderHistoryID"),
                                ProductType = reader.GetInt32("ProductType"),
                                PaymentMethod = reader.GetString("PaymentMethod"),
                                OrderDate = reader.GetDateTime("OrderDate")
                            };
                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }


    }
}
