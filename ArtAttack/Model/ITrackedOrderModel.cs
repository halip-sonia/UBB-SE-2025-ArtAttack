using ArtAttack.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.Model
{
    interface ITrackedOrderModel
    {
        int AddTrackedOrder(TrackedOrder order);
        TrackedOrder? GetTrackedOrderById(int trackOrderID);
        List<TrackedOrder> GetAllTrackedOrders();
        bool UpdateTrackedOrder(TrackedOrder order);
        bool DeleteTrackedOrder(int trackOrderID);

        int AddOrderCheckpoint(OrderCheckpoint checkpoint);
        OrderCheckpoint? GetOrderCheckpointById(int checkpointID);
        List<OrderCheckpoint> GetAllOrderCheckpoints(int trackedOrderID);
        bool UpdateOrderCheckpoint(OrderCheckpoint checkpoint);
        bool DeleteOrderCheckpoint(int checkpointID);
    }
}
