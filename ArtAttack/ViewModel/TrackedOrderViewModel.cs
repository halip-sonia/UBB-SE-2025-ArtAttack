using ArtAttack.Domain;
using ArtAttack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtAttack.ViewModel
{
    class TrackedOrderViewModel : ITrackedOrderViewModel
    {
        private readonly TrackedOrderModel model;

        public TrackedOrderViewModel(string connectionString)
        {
            model = new TrackedOrderModel(connectionString);
        }

        public async Task<TrackedOrder?> GetTrackedOrderByIDAsync(int trackedOrderID)
        {
            return await model.GetTrackedOrderByIdAsync(trackedOrderID);
        }

        public async Task<OrderCheckpoint?> GetOrderCheckpointByIDAsync(int checkpointID)
        {
            return await model.GetOrderCheckpointByIdAsync(checkpointID);
        }

        public async Task<List<TrackedOrder>> GetAllTrackedOrdersAsync()
        {
            return await model.GetAllTrackedOrdersAsync();
        }

        public async Task<List<OrderCheckpoint>> GetAllOrderCheckpointsAsync(int trackedOrderID)
        {
            return await model.GetAllOrderCheckpointsAsync(trackedOrderID);
        }

        public async Task<bool> DeleteTrackedOrderAsync(int trackOrderID)
        {
            return await model.DeleteTrackedOrderAsync(trackOrderID);
        }

        public async Task<bool> DeleteOrderCheckpointAsync(int checkpointID)
        {
            return await model.DeleteOrderCheckpointAsync(checkpointID);
        }

        public async Task<int> AddTrackedOrderAsync(TrackedOrder order)
        {
            return await model.AddTrackedOrderAsync(order);
        }

        public async Task<int> AddOrderCheckpointAsync(OrderCheckpoint checkpoint)
        {
            return await model.AddOrderCheckpointAsync(checkpoint);
        }

        public async Task<bool> UpdateOrderCheckpointAsync(int checkpointID, DateTime timestamp, string? location, string description, OrderStatus status, int trackedOrderID)
        {
            return await model.UpdateOrderCheckpointAsync(checkpointID, timestamp, location, description, status, trackedOrderID);
        }

        public async Task<bool> UpdateTrackedOrderAsync(int trackedOrderID, DateOnly estimatedDeliveryDate, string deliveryAddress, OrderStatus currentStatus, int orderID)
        {
            return await model.UpdateTrackedOrderAsync(trackedOrderID, estimatedDeliveryDate, deliveryAddress, currentStatus, orderID);
        }

        public async Task<bool> RevertToLastCheckpoint(TrackedOrder order)
        {
            List<OrderCheckpoint> allCheckpoints = await GetAllOrderCheckpointsAsync(order.TrackedOrderID);
            OrderCheckpoint? lastCheckpoint = allCheckpoints.LastOrDefault();
            if (lastCheckpoint != null)
                return await DeleteOrderCheckpointAsync(lastCheckpoint.CheckpointID);
            return false;
        }
    }
}
