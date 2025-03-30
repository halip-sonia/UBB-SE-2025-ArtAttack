using ArtAttack.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ArtAttack.Model
{
    class TrackedOrderModel
    {
        private readonly string connectionString;

        public TrackedOrderModel(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<int> AddOrderCheckpointAsync(OrderCheckpoint checkpoint)
        {
            int insertedID = -1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
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

                    SqlParameter outputParam = new SqlParameter("@newCheckpointID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    await cmd.ExecuteNonQueryAsync();

                    int newID = (int)cmd.Parameters["@newCheckpointID"].Value;
                    if (newID < 0)
                        throw new Exception("Unexpected error when trying to add the OrderCheckpoint");
                    checkpoint.CheckpointID = newID;
                    return newID;
                }
            }

            return insertedID;
        }

        public async Task<int> AddTrackedOrderAsync(TrackedOrder order)
        {
            int insertedID = -1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO TrackedOrder (EstimatedDeliveryDate, DeliveryAddress, OrderStatus, OrderID) 
                    OUTPUT inserted.TrackedOrderID 
                    VALUES (@estimatedDeliveryDate, @deliveryAddress, @status, @orderID)", conn))
                {
                    cmd.Parameters.AddWithValue("@estimatedDeliveryDate", order.EstimatedDeliveryDate);
                    cmd.Parameters.AddWithValue("@deliveryAddress", order.DeliveryAddress);
                    cmd.Parameters.AddWithValue("@status", order.CurrentStatus.ToString());
                    cmd.Parameters.AddWithValue("@orderID", order.OrderID);

                    SqlParameter outputParam = new SqlParameter("@newTrackedOrderID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    await cmd.ExecuteNonQueryAsync();

                    int newID = (int)cmd.Parameters["@newTrackedOrderID"].Value;
                    if (newID < 0)
                        throw new Exception("Unexpected error when trying to add the TrackedOrder");
                    order.TrackedOrderID = newID;
                    return newID;
                }
            }

            return insertedID;
        }

        public async Task<bool> DeleteOrderCheckpointAsync(int checkpointID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM OrderCheckpoints WHERE CheckpointID = @checkpointID", conn))
                {
                    cmd.Parameters.AddWithValue("@checkpointID", checkpointID);
                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public async Task<bool> DeleteTrackedOrderAsync(int trackOrderID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM TrackedOrders WHERE TrackedOrderID = @trackOrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@trackOrderID", trackOrderID);
                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public async Task<List<OrderCheckpoint>> GetAllOrderCheckpointsAsync(int trackedOrderID)
        {
            List<OrderCheckpoint> checkpoints = new List<OrderCheckpoint>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM OrderCheckpoints WHERE TrackedOrderID = @trackedOrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@trackedOrderID", trackedOrderID);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
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

        public async Task<List<TrackedOrder>> GetAllTrackedOrdersAsync()
        {
            List<TrackedOrder> orders = new List<TrackedOrder>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM TrackedOrders", conn))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            orders.Add(new TrackedOrder
                            {
                                TrackedOrderID = reader.GetInt32(reader.GetOrdinal("TrackedOrderID")),
                                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                CurrentStatus = Enum.Parse<OrderStatus>(reader.GetString(reader.GetOrdinal("OrderStatus"))),
                                EstimatedDeliveryDate = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("EstimatedDeliveryDate")),
                                DeliveryAddress = reader.GetString(reader.GetOrdinal("DeliveryAddress"))
                            });
                        }
                    }
                }
            }
            return orders;
        }

        public async Task<OrderCheckpoint?> GetOrderCheckpointByIdAsync(int checkpointID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM OrderCheckpoints WHERE CheckpointID = @checkpointID", conn))
                {
                    cmd.Parameters.AddWithValue("@checkpointID", checkpointID);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
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

        public async Task<TrackedOrder?> GetTrackedOrderByIdAsync(int trackOrderID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM TrackedOrders WHERE TrackedOrderID = @trackedOrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@trackedOrderID", trackOrderID);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new TrackedOrder
                            {
                                TrackedOrderID = reader.GetInt32(reader.GetOrdinal("TrackedOrderID")),
                                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                CurrentStatus = Enum.Parse<OrderStatus>(reader.GetString(reader.GetOrdinal("OrderStatus"))),
                                EstimatedDeliveryDate = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("EstimatedDeliveryDate")),
                                DeliveryAddress = reader.GetString(reader.GetOrdinal("DeliveryAddress"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task<bool> UpdateOrderCheckpointAsync(int checkpointID, DateTime timestamp, string? location, string description, OrderStatus status, int trackedOrderID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(
                    @"UPDATE OrderCheckpoints 
                    SET Timestamp = @timestamp,
                        Location = @location,
                        Description = @description,
                        Status = @status,
                        trackedOrderID = @trackedOrderID
                    WHERE CheckpointID = @checkpointID", conn))
                {
                    cmd.Parameters.AddWithValue("@timestamp", timestamp.ToString());
                    cmd.Parameters.AddWithValue("@location", location ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@status", status.ToString());
                    cmd.Parameters.AddWithValue("@trackedOrderID", trackedOrderID);
                    cmd.Parameters.AddWithValue("@checkpointID", checkpointID);

                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public async Task<bool> UpdateTrackedOrderAsync(int trackedOrderID, DateOnly estimatedDeliveryDate, string deliveryAddress, OrderStatus currentStatus, int orderID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(
                    @"UPDATE TrackedOrder 
                    SET EstimatedDeliveryDate = @estimatedDeliveryDate, 
                        DeliveryAddress = @deliveryAddress,
                        CurrentStatus= @status, 
                        OrderID = @orderID
                    WHERE TrackedOrderID = @trackOrderID"
                , conn))

                {
                    DateTime estimatedDeliveryDateTime = estimatedDeliveryDate.ToDateTime(TimeOnly.MinValue);

                    cmd.Parameters.Add("@estimatedDeliveryDate", SqlDbType.Date).Value = estimatedDeliveryDateTime;
                    cmd.Parameters.AddWithValue("@deliveryAddress", deliveryAddress);
                    cmd.Parameters.AddWithValue("@status", currentStatus.ToString());
                    cmd.Parameters.AddWithValue("@orderID", orderID);
                    cmd.Parameters.AddWithValue("@trackOrderID", trackedOrderID);

                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }
    }
}
