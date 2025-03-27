using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArtAttack.Domain;
using ArtAttack.Model;
using Microsoft.Data.SqlClient;

namespace ArtAttack.ViewModel
{
    public interface IOrderViewModel
    {
        Task AddOrderAsync(int productId, int buyerId, int productType, string paymentMethod, int orderSummaryId, DateTime orderDate);
        Task UpdateOrderAsync(int orderId, int productType, string paymentMethod, DateTime orderDate);
        Task DeleteOrderAsync(int orderId);
        Task<List<Order>> GetBorrowedOrderHistoryAsync(int buyerId);
        Task<List<Order>> GetNewOrUsedOrderHistoryAsync(int buyerId);
        Task<List<Order>> GetOrdersFromLastThreeMonthsAsync(int buyerId);
        Task<List<Order>> GetOrdersFromLastSixMonthsAsync(int buyerId);
        Task<List<Order>> GetOrdersFrom2024Async(int buyerId);
        Task<List<Order>> GetOrdersFrom2025Async(int buyerId);
        Task<List<Order>> GetOrdersByNameAsync(int buyerId, string text);
        Task<List<Order>> GetOrdersFromOrderHistoryAsync(int orderHistoryId);
        Task<OrderSummary> GetOrderSummaryAsync(int orderSummaryId);
        Task<Order> GetOrderByIdAsync(int orderId);
    }

    public class OrderViewModel : IOrderViewModel
    {
        private readonly OrderModel _model;

        public OrderViewModel(string connectionString)
        {
            _model = new OrderModel(connectionString);
        }

        public async Task AddOrderAsync(int productId, int buyerId, int productType, string paymentMethod, int orderSummaryId, DateTime orderDate)
        {
            await Task.Run(() => _model.AddOrder(productId, buyerId, productType, paymentMethod, orderSummaryId, orderDate));
        }

        public async Task UpdateOrderAsync(int orderId, int productType, string paymentMethod, DateTime orderDate)
        {
            await Task.Run(() => _model.UpdateOrder(orderId, productType, paymentMethod, orderDate));
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            await Task.Run(() => _model.DeleteOrder(orderId));
        }

        public async Task<List<Order>> GetBorrowedOrderHistoryAsync(int buyerId)
        {
            return await Task.Run(() => _model.GetBorrowedOrderHistory(buyerId));
        }

        public async Task<List<Order>> GetNewOrUsedOrderHistoryAsync(int buyerId)
        {
            return await Task.Run(() => _model.GetNewOrUsedOrderHistory(buyerId));
        }

        public async Task<List<Order>> GetOrdersFromLastThreeMonthsAsync(int buyerId)
        {
            return await Task.Run(() => _model.GetOrdersFromLastThreeMonths(buyerId));
        }

        public async Task<List<Order>> GetOrdersFromLastSixMonthsAsync(int buyerId)
        {
            return await Task.Run(() => _model.GetOrdersFromLastSixMonths(buyerId));
        }

        public async Task<List<Order>> GetOrdersFrom2024Async(int buyerId)
        {
            return await Task.Run(() => _model.GetOrdersFrom2024(buyerId));
        }

        public async Task<List<Order>> GetOrdersFrom2025Async(int buyerId)
        {
            return await Task.Run(() => _model.GetOrdersFrom2025(buyerId));
        }

        public async Task<List<Order>> GetOrdersByNameAsync(int buyerId, string text)
        {
            return await Task.Run(() => _model.GetOrdersByName(buyerId, text));
        }

        public async Task<List<Order>> GetOrdersFromOrderHistoryAsync(int orderHistoryId)
        {
            return await Task.Run(() => _model.GetOrdersFromOrderHistory(orderHistoryId));
        }

        public async Task<OrderSummary> GetOrderSummaryAsync(int orderSummaryId)
        {
            return await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_model.ConnectionString))
                {
                    string query = @"SELECT * FROM [OrderSummary] WHERE ID = @OrderSummaryId";
                    SqlCommand cmd = new SqlCommand(query, conn);

                   
                    cmd.Parameters.Add("@OrderSummaryId", SqlDbType.Int).Value = orderSummaryId;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new OrderSummary()
                            {
                                ID = reader.GetInt32("ID"),
                                Subtotal = (float)reader.GetDecimal("Subtotal"),
                                WarrantyTax = (float)reader.GetDecimal("WarrantyTax"),
                                DeliveryFee = (float)reader.GetDecimal("DeliveryFee"),
                                FinalTotal = (float)reader.GetDecimal("FinalTotal"),
                                FullName = reader.GetString("FullName"),
                                Email = reader.GetString("Email"),
                                PhoneNumber = reader.GetString("PhoneNumber"),
                                Address = reader.GetString("Address"),
                                PostalCode = reader.GetString("PostalCode"),
                                AdditionalInfo = reader.IsDBNull("AdditionalInfo") ? "" : reader.GetString("AdditionalInfo"),
                                ContractDetails = reader.IsDBNull("ContractDetails") ? "" : reader.GetString("ContractDetails")
                            };
                        }
                    }
                }
                throw new KeyNotFoundException($"OrderSummary with ID {orderSummaryId} not found");
            });
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await Task.Run(() =>
            {
                
                var borrowedOrders = _model.GetBorrowedOrderHistory(0); 
                var order = borrowedOrders.FirstOrDefault(o => o.OrderID == orderId);
                if (order != null) return order;

                
                var newUsedOrders = _model.GetNewOrUsedOrderHistory(0); 
                return newUsedOrders.FirstOrDefault(o => o.OrderID == orderId);
            });
        }

        public async Task<List<Order>> GetCombinedOrderHistoryAsync(int buyerId, string timePeriodFilter = "all")
        {
            return await Task.Run(() =>
            {
                List<Order> orders = new List<Order>();

                switch (timePeriodFilter.ToLower())
                {
                    case "3months":
                        orders = _model.GetOrdersFromLastThreeMonths(buyerId);
                        break;
                    case "6months":
                        orders = _model.GetOrdersFromLastSixMonths(buyerId);
                        break;
                    case "2024":
                        orders = _model.GetOrdersFrom2024(buyerId);
                        break;
                    case "2025":
                        orders = _model.GetOrdersFrom2025(buyerId);
                        break;
                    case "all":
                    default:
                        var borrowedOrders = _model.GetBorrowedOrderHistory(buyerId);
                        var newUsedOrders = _model.GetNewOrUsedOrderHistory(buyerId);
                        orders.AddRange(borrowedOrders);
                        orders.AddRange(newUsedOrders);
                        break;
                }

                return orders;
            });
        }
    }
}