using ArtAttack.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.Model
{
    internal class OrderModel
    {
        private readonly string _connectionString;

        public OrderModel(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddOrder(int productId, int buyerId, string productType, string paymentMethod, int orderSummaryId, DateTime orderDate)
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

        public void UpdateOrder(int orderId, string productType, string paymentMethod, DateTime orderDate)
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
            List<Order> orders = new List<DummyProduct>();
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
                                ProductType = reader.GetString("ProductType"),
                                PaymentMethod = reader.GetString("PaymentMethod"),
                                OrderDate = reader.GetDateTime("OrderDate"),
                                Subtotal = reader.GetDecimal("Subtotal"),
                                WarrantyTax = reader.GetDecimal("WarrantyTax"),
                                DeliveryFee = reader.GetDecimal("DeliveryFee"),
                                FinalTotal = reader.GetDecimal("finalTotal"),
                                Address = reader.GetString("address"),
                                AdditionalInfo = reader["AdditionalInfo"] as string,
                                ContractDetails = reader["ContractDetails"] as string,
                                ProductName = reader.GetString("ProductName")
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
                                ProductType = reader.GetString("ProductType"),
                                PaymentMethod = reader.GetString("PaymentMethod"),
                                OrderDate = reader.GetDateTime("OrderDate"),
                                Subtotal = reader.GetDecimal("Subtotal"),
                                DeliveryFee = reader.GetDecimal("DeliveryFee"),
                                FinalTotal = reader.GetDecimal("finalTotal"),
                                Address = reader.GetString("address"),
                                AdditionalInfo = reader["AdditionalInfo"] as string,
                                ProductName = reader.GetString("ProductName")
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
                                ProductType = reader.GetString("ProductType"),
                                PaymentMethod = reader.GetString("PaymentMethod"),
                                OrderDate = reader.GetDateTime("OrderDate"),
                                Subtotal = reader.GetDecimal("Subtotal"),
                                WarrantyTax = reader.GetDecimal("WarrantyTax"),
                                DeliveryFee = reader.GetDecimal("DeliveryFee"),
                                FinalTotal = reader.GetDecimal("finalTotal"),
                                Address = reader.GetString("address"),
                                AdditionalInfo = reader["AdditionalInfo"] as string,
                                ContractDetails = reader["ContractDetails"] as string,
                                ProductName = reader.GetString("ProductName")
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
                                ProductType = reader.GetString("ProductType"),
                                PaymentMethod = reader.GetString("PaymentMethod"),
                                OrderDate = reader.GetDateTime("OrderDate"),
                                Subtotal = reader.GetDecimal("Subtotal"),
                                WarrantyTax = reader.GetDecimal("WarrantyTax"),
                                DeliveryFee = reader.GetDecimal("DeliveryFee"),
                                FinalTotal = reader.GetDecimal("finalTotal"),
                                Address = reader.GetString("address"),
                                AdditionalInfo = reader["AdditionalInfo"] as string,
                                ContractDetails = reader["ContractDetails"] as string,
                                ProductName = reader.GetString("ProductName")
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
                                ProductType = reader.GetString("ProductType"),
                                PaymentMethod = reader.GetString("PaymentMethod"),
                                OrderDate = reader.GetDateTime("OrderDate"),
                                Subtotal = reader.GetDecimal("Subtotal"),
                                WarrantyTax = reader.GetDecimal("WarrantyTax"),
                                DeliveryFee = reader.GetDecimal("DeliveryFee"),
                                FinalTotal = reader.GetDecimal("finalTotal"),
                                Address = reader.GetString("address"),
                                AdditionalInfo = reader["AdditionalInfo"] as string,
                                ContractDetails = reader["ContractDetails"] as string,
                                ProductName = reader.GetString("ProductName")
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
                                ProductType = reader.GetString("ProductType"),
                                PaymentMethod = reader.GetString("PaymentMethod"),
                                OrderDate = reader.GetDateTime("OrderDate"),
                                Subtotal = reader.GetDecimal("Subtotal"),
                                WarrantyTax = reader.GetDecimal("WarrantyTax"),
                                DeliveryFee = reader.GetDecimal("DeliveryFee"),
                                FinalTotal = reader.GetDecimal("finalTotal"),
                                Address = reader.GetString("address"),
                                AdditionalInfo = reader["AdditionalInfo"] as string,
                                ContractDetails = reader["ContractDetails"] as string,
                                ProductName = reader.GetString("ProductName")
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
                                ProductType = reader.GetString("ProductType"),
                                PaymentMethod = reader.GetString("PaymentMethod"),
                                OrderDate = reader.GetDateTime("OrderDate"),
                                Subtotal = reader.GetDecimal("Subtotal"),
                                WarrantyTax = reader.GetDecimal("WarrantyTax"),
                                DeliveryFee = reader.GetDecimal("DeliveryFee"),
                                FinalTotal = reader.GetDecimal("finalTotal"),
                                Address = reader.GetString("address"),
                                AdditionalInfo = reader["AdditionalInfo"] as string,
                                ContractDetails = reader["ContractDetails"] as string,
                                ProductName = reader.GetString("ProductName")
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
                                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                OrderHistoryID = reader.GetInt32(reader.GetOrdinal("OrderHistoryID")),
                                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                            };
                            orders.Add(order);
                        }
                    }
                }
            }

            return products;
        }


    }
}
