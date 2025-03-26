using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAttack.Domain;

namespace ArtAttack.Model
{
    public class NotificationDataAdapter : IDisposable
    {
        private readonly SqlConnection _connection;

        public NotificationDataAdapter(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        public List<Notification> GetNotificationsForUser(int recipientId)
        {
            var notifications = new List<Notification>();
            const string query = @"
            SELECT * FROM Notification 
            WHERE recipientID = @RecipientId
            ORDER BY timestamp DESC";

            using (var command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@RecipientId", recipientId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        notifications.Add(NotificationFactory.CreateFromDataReader(reader));
                    }
                }
            }
            return notifications;
        }

        public void MarkAsRead(int notificationId)
        {
            const string query = @"
            UPDATE Notification 
            SET isRead = 1 
            WHERE notificationID = @NotificationId";

            using (var command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@NotificationId", notificationId);
                command.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
