using System.Data;
using Microsoft.Data.SqlClient;
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

        public List<DummyProduct> GetBorrowedOrderHistory(int buyerId)
        {
            List<BorrowedOrderHistory> orders = new List<BorrowedOrderHistory>();
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
                            BorrowedOrderHistory order = new BorrowedOrderHistory
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

        public List<DummyProduct> GetNewOrUsedOrderHistory(int buyerId)
        {
            List<NewOrUsedOrderHistory> orders = new List<NewOrUsedOrderHistory>();
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
                            NewOrUsedOrderHistory order = new NewOrUsedOrderHistory
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


        public List<DummyProduct> GetOrdersFromLastThreeMonths(int buyerId)
        {
            List<OrderHistory> orders = new List<OrderHistory>();
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
                            OrderHistory order = new OrderHistory
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

        public List<DummyProduct> GetOrdersFromLastSixMonths(int buyerId)
        {
            List<OrderHistory> orders = new List<OrderHistory>();
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
                            OrderHistory order = new OrderHistory
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

        public List<DummyProduct> GetOrdersFrom2025(int buyerId)
        {
            List<OrderHistory> orders = new List<OrderHistory>();
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
                            OrderHistory order = new OrderHistory
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

        public List<DummyProduct> GetOrdersFrom2024(int buyerId)
        {
            List<OrderHistory> orders = new List<OrderHistory>();
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
                            OrderHistory order = new OrderHistory
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

        public List<DummyProduct> GetOrdersByName(int buyerId, string text)
        {
            List<OrderHistory> orders = new List<OrderHistory>();
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
                            OrderHistory order = new OrderHistory
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

        public List<DummyProduct> GetProductsFromOrderHistory(int orderHistoryId)
        {
            List<DummyProduct> products = new List<DummyProduct>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("get_products_from_order_history", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@OrderHistoryID", SqlDbType.Int).Value = orderHistoryId;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderProduct product = new OrderProduct
                            {
                                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                OrderHistoryID = reader.GetInt32(reader.GetOrdinal("BuyerID")),
                                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                ProductType = reader.GetString(reader.GetOrdinal("ProductType")),
                                PaymentMethod = reader.GetString(reader.GetOrdinal("PaymentMethod")),
                                OrderSummaryID = reader.GetInt32(reader.GetOrdinal("OrderSummaryID")),
                                OrderDate=reader.GetDateTime(reader.GetOrdinal("Orderdate")),
                                orderHistoryId = reader.GetInt32(reader.GetOrdinal("OrderHistoryID"))

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
