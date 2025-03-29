using ArtAttack.Domain;
using ArtAttack.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.ViewModel
{
    internal class TrackedOrderViewModel : ITrackedOrderViewModel
    {
        private readonly TrackedOrderModel model;

        public TrackedOrderViewModel(string connectionString)
        {
            model = new TrackedOrderModel(connectionString);
        }

        public async Task<TrackedOrder> GetTrackedOrderByIDAsync(int trackedOrderID)
        {
            return await model.GetTrackedOrderByIdAsync(trackedOrderID);
        }

        public async Task<OrderCheckpoint> GetOrderCheckpointByIDAsync(int checkpointID)
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

        public async Task UpdateOrderCheckpointAsync(int checkpointID, DateTime timestamp, string? location, string description, OrderStatus status)
        {
            await model.UpdateOrderCheckpointAsync(checkpointID, timestamp, location, description, status);
        }

        public async Task UpdateTrackedOrderAsync(int trackedOrderID, DateOnly estimatedDeliveryDate, OrderStatus currentStatus)
        {
            await model.UpdateTrackedOrderAsync(trackedOrderID, estimatedDeliveryDate, currentStatus);
        }

        public async Task RevertToPreviousCheckpoint(TrackedOrder order)
        {
            int initialNrOfCheckpoints = await GetNumberOfCheckpoints(order);
            if (initialNrOfCheckpoints == 0)
                throw new Exception("Cannot revert further");

            var lastCheckpoint = await GetLastCheckpoint(order);
            if (lastCheckpoint != null)
            {
                OrderCheckpoint lastCheckpointCast = (OrderCheckpoint)lastCheckpoint;
                bool deleteSuccessful = await DeleteOrderCheckpointAsync(lastCheckpointCast.CheckpointID);
                if (deleteSuccessful)
                {
                    if(initialNrOfCheckpoints == 1)
                    {
                        await UpdateTrackedOrderAsync(order.TrackedOrderID, order.EstimatedDeliveryDate, OrderStatus.PROCESSING);
                    }
                    else 
                    {
                        OrderCheckpoint newLastCheckpoint = (OrderCheckpoint)await GetLastCheckpoint(order);
                        await UpdateTrackedOrderAsync(order.TrackedOrderID, order.EstimatedDeliveryDate, newLastCheckpoint.Status);
                    }
                    
                }
                else
                    throw new Exception("Unexpected error when trying to delete the current checkpoint");
            }
            else
                throw new Exception("Unexpected error when trying to revert to the previous checkpoint");
        }

        public async Task<OrderCheckpoint?> GetLastCheckpoint(TrackedOrder order)
        {
            List<OrderCheckpoint> allCheckpoints = await GetAllOrderCheckpointsAsync(order.TrackedOrderID);
            OrderCheckpoint? lastCheckpoint = allCheckpoints.LastOrDefault();
            return lastCheckpoint;
        }

        public async Task<int> GetNumberOfCheckpoints(TrackedOrder order)
        {
            List<OrderCheckpoint> allCheckpoints = await GetAllOrderCheckpointsAsync(order.TrackedOrderID);
            return allCheckpoints.Count;
        }
    }
}
