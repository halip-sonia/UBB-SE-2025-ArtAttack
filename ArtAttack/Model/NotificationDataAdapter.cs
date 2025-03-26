using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAttack.Domain;
using System.Runtime.Intrinsics.Arm;
using Windows.UI.Notifications;
using Notification = ArtAttack.Domain.Notification;
using ArtAttack.Model;

namespace ArtAttack.Model
{
    public class NotificationDataAdapter : IDisposable
    {
        private SqlConnection _connection;

        public NotificationDataAdapter(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        public List<Notification> GetNotificationsForUser(int recipientId)
        {
            var notifications = new List<Notification>();

            SqlCommand command = new SqlCommand("GetNotificationsByRecipient", _connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@RecipientId", recipientId);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    notifications.Add(NotificationFactory.CreateFromDataReader(reader));
                }
            }
            return notifications;
        }

        public void MarkAsRead(int notificationId)
        {
            using (var command = new SqlCommand("MarkNotificationAsRead", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@notificationID", notificationId);

                int rowsAffected = command.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
