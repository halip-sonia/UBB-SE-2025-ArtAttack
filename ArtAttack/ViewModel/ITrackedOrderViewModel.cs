using ArtAttack.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtAttack.ViewModel
{
    internal interface ITrackedOrderViewModel
    {
        Task<int> AddOrderCheckpointAsync(OrderCheckpoint checkpoint);
        Task<int> AddTrackedOrderAsync(TrackedOrder order);
        Task<bool> DeleteOrderCheckpointAsync(int checkpointID);
        Task<bool> DeleteTrackedOrderAsync(int trackOrderID);
        Task<List<OrderCheckpoint>> GetAllOrderCheckpointsAsync(int trackedOrderID);
        Task<List<TrackedOrder>> GetAllTrackedOrdersAsync();
        Task<OrderCheckpoint?> GetLastCheckpoint(TrackedOrder order);
        Task<int> GetNumberOfCheckpoints(TrackedOrder order);
        Task<OrderCheckpoint> GetOrderCheckpointByIDAsync(int checkpointID);
        Task<TrackedOrder> GetTrackedOrderByIDAsync(int trackedOrderID);
        Task RevertToPreviousCheckpoint(TrackedOrder order);
        Task UpdateOrderCheckpointAsync(int checkpointID, DateTime timestamp, string? location, string description, OrderStatus status);
        Task UpdateTrackedOrderAsync(int trackedOrderID, DateOnly estimatedDeliveryDate, OrderStatus currentStatus);
    }
}