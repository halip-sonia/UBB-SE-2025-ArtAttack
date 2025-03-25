using ArtAttack.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using System.Reflection.PortableExecutable;

namespace ArtAttack.Model
{
    class TrackedOrderModel : ITrackedOrderModel
    {
        private readonly string connectionString;

        public TrackedOrderModel(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public int AddOrderCheckpoint(OrderCheckpoint checkpoint)
        {
            int insertedID = -1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO OrderCheckpoints (Timestamp, Location, Description, Status, TrackedOrderID) 
                    OUTPUT inserted.CheckpointID 
                    VALUES (@timestamp, @location, @description, @status, @trackOrderID)", conn))
                {
                    cmd.Parameters.AddWithValue("@timestamp", checkpoint.Timestamp);
                    cmd.Parameters.AddWithValue("@location", checkpoint.Location ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@description", checkpoint.Description);
                    cmd.Parameters.AddWithValue("@status", checkpoint.Status.ToString());
                    cmd.Parameters.AddWithValue("@trackOrderID", checkpoint.TrackedOrderID);

                    insertedID = (int)cmd.ExecuteScalar();
                }
            }

            return insertedID;
        }

        public int AddTrackedOrder(TrackedOrder order)
        {
            int insertedID = -1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO TrackedOrder (EstimatedDeliveryDate, DeliveryAddress, OrderStatus, OrderID) 
                    OUTPUT inserted.TrackedOrderID 
                    VALUES (@estimatedDeliveryDate, @deliveryAddress, @status, @orderID)", conn))
                {
                    cmd.Parameters.AddWithValue("@estimatedDeliveryDate", order.EstimatedDeliveryDate);
                    cmd.Parameters.AddWithValue("@deliveryAddress", order.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@status", order.CurrentStatus.ToString());
                    cmd.Parameters.AddWithValue("@orderID", order.OrderID);

                    insertedID = (int)cmd.ExecuteScalar();
                }
            }

            return insertedID;
        }

        public bool DeleteOrderCheckpoint(int checkpointID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM OrderCheckpoints WHERE CheckpointID = @checkpointID", conn))
                {
                    cmd.Parameters.AddWithValue("@checkpointID", checkpointID);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteTrackedOrder(int trackOrderID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM TrackedOrders WHERE TrackedOrderID = @trackOrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@trackOrderID", trackOrderID);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<OrderCheckpoint> GetAllOrderCheckpoints(int trackedOrderID)
        {
            List<OrderCheckpoint> checkpoints = new List<OrderCheckpoint>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM OrderCheckpoints WHERE TrackedOrderID = @trackedOrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@trackedOrderID", trackedOrderID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            checkpoints.Add(new OrderCheckpoint
                            {
                                CheckpointID = reader.GetInt32(reader.GetOrdinal("CheckpointID")),
                                Timestamp = reader.GetDateTime(reader.GetOrdinal("Timestamp")),
                                Location = reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader.GetString(reader.GetOrdinal("Location")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Status = Enum.Parse<OrderStatus>(reader.GetString(reader.GetOrdinal("Status"))),
                                TrackedOrderID = reader.GetInt32(reader.GetOrdinal("TrackedOrderID"))
                            });
                        }
                    }
                }
            }
            return checkpoints;
        }

        public List<TrackedOrder> GetAllTrackedOrders()
        {
            List<TrackedOrder> orders = new List<TrackedOrder>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM TrackedOrders", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add( new TrackedOrder
                            {
                                TrackedOrderID = reader.GetInt32(reader.GetOrdinal("TrackedOrderID")),
                                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                CurrentStatus = Enum.Parse<OrderStatus>(reader.GetString(reader.GetOrdinal("OrderStatus"))),
                                EstimatedDeliveryDate = reader.GetDateTime(reader.GetOrdinal("EstimatedDeliveryDate")),
                                DeliveryAddress = reader.GetString(reader.GetOrdinal("DeliveryAddress"))
                            });
                        }
                    }
                }
            }
            return orders;
        }

        public OrderCheckpoint? GetOrderCheckpointById(int checkpointID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM OrderCheckpoints WHERE CheckpointID = @checkpointID", conn))
                {
                    cmd.Parameters.AddWithValue("@checkpointID", checkpointID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new OrderCheckpoint
                            {
                                CheckpointID = reader.GetInt32(reader.GetOrdinal("CheckpointID")),
                                Timestamp = reader.GetDateTime(reader.GetOrdinal("Timestamp")),
                                Location = reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader.GetString(reader.GetOrdinal("Location")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Status = Enum.Parse<OrderStatus>(reader.GetString(reader.GetOrdinal("Status"))),
                                TrackedOrderID = reader.GetInt32(reader.GetOrdinal("TrackedOrderID"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public TrackedOrder? GetTrackedOrderById(int trackOrderID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SELECT * FROM TrackedOrders WHERE TrackedOrderID = @trackedOrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@trackedOrderID", trackOrderID);

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TrackedOrder
                            {
                                TrackedOrderID = reader.GetInt32(reader.GetOrdinal("TrackedOrderID")),
                                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                CurrentStatus = Enum.Parse<OrderStatus>(reader.GetString(reader.GetOrdinal("OrderStatus"))),
                                EstimatedDeliveryDate = reader.GetDateTime(reader.GetOrdinal("EstimatedDeliveryDate")),
                                DeliveryAddress = reader.GetString(reader.GetOrdinal("DeliveryAddress"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool UpdateOrderCheckpoint(OrderCheckpoint checkpoint)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    @"UPDATE OrderCheckpoints 
                    SET Timestamp = @timestamp,
                        Location = @location,
                        Description = @description,
                        Status = @status,
                        trackedOrderID = @trackedOrderID
                    WHERE CheckpointID = @checkpointID", conn))
                {
                    cmd.Parameters.AddWithValue("@timestamp", checkpoint.Timestamp);
                    cmd.Parameters.AddWithValue("@location", checkpoint.Location ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@description", checkpoint.Description);
                    cmd.Parameters.AddWithValue("@status", checkpoint.Status.ToString());
                    cmd.Parameters.AddWithValue("@trackedOrderID", checkpoint.TrackedOrderID);
                    cmd.Parameters.AddWithValue("@checkpointID", checkpoint.CheckpointID);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateTrackedOrder(TrackedOrder order)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    @"UPDATE TrackedOrder 
                    SET EstimatedDeliveryDate = @estimatedDeliveryDate, 
                        DeliveryAddress = @deliveryAddress,
                        CurrentStatus= @status, 
                        OrderID = @orderID
                    WHERE TrackedOrderID = @trackOrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@estimatedDeliveryDate", order.EstimatedDeliveryDate);
                    cmd.Parameters.AddWithValue("@deliveryAddress", order.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@status", order.CurrentStatus.ToString());
                    cmd.Parameters.AddWithValue("@orderID", order.OrderID);
                    cmd.Parameters.AddWithValue("@trackOrderID", order.TrackedOrderID);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
