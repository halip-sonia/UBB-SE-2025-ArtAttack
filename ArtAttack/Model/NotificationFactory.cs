using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAttack.Domain;

namespace ArtAttack.Model
{
    public static class NotificationFactory
    {
        public static Notification CreateFromDataReader(SqlDataReader reader)
        {
            //Common fields
            int notificationId = reader.GetInt32(reader.GetOrdinal("notificationID"));
            int recipientId = reader.GetInt32(reader.GetOrdinal("recipientID"));
            DateTime timestamp = reader.GetDateTime(reader.GetOrdinal("timestamp"));
            bool isRead = reader.GetBoolean(reader.GetOrdinal("isRead"));
            string category = reader.GetString(reader.GetOrdinal("category"));

            switch (category)
            {
                case "CONTRACT_RENEWAL_ANS":
                    int contractId = reader.GetInt32(reader.GetOrdinal("contractID"));
                    bool isAccepted = reader.GetBoolean(reader.GetOrdinal("isAccepted"));
                    return new ContractRenewalAnswerNotification(recipientId, timestamp, isRead, contractId, isAccepted, notificationId);

                case "CONTRACT_RENEWAL_WAITLIST":
                    int productIdWaitlist = reader.GetInt32(reader.GetOrdinal("productID"));
                    return new ContractRenewalWaitlistNotification(recipientId, timestamp, isRead, productIdWaitlist, notificationId);

                case "OUTBIDDED":
                    int productIdOutbidded = reader.GetInt32(reader.GetOrdinal("productID"));
                    return new OutbiddedNotification(recipientId, timestamp, isRead, productIdOutbidded, notificationId);

                case "ORDER_SHIPPING_PROGRESS":
                    int orderId = reader.GetInt32(reader.GetOrdinal("orderID"));
                    string shippingState = reader.GetString(reader.GetOrdinal("shippingState"));
                    DateTime deliveryDate = reader.GetDateTime(reader.GetOrdinal("deliveryDate"));
                    return new OrderShippingProgressNotification(recipientId, timestamp, isRead, orderId, shippingState, deliveryDate, notificationId);

                case "PAYMENT_CONFIRMATION":
                    int productIdPayment = reader.GetInt32(reader.GetOrdinal("productID"));
                    int orderIdPayment = reader.GetInt32(reader.GetOrdinal("orderID"));
                    return new PaymentConfirmationNotification(recipientId, timestamp, isRead, productIdPayment, orderIdPayment, notificationId);

                case "PRODUCT_REMOVED":
                    int productIdRemoved = reader.GetInt32(reader.GetOrdinal("productID"));
                    return new ProductRemovedNotification(recipientId, timestamp, isRead, productIdRemoved, notificationId);

                case "PRODUCT_AVAILABLE":
                    int productIdAvailable = reader.GetInt32(reader.GetOrdinal("productID"));
                    return new ProductAvailableNotification(recipientId, timestamp, isRead, productIdAvailable, notificationId);

                case "CONTRACT_RENEWAL_REQ":
                    int contractIdReq = reader.GetInt32(reader.GetOrdinal("contractID"));
                    return new ContractRenewalRequestNotification(recipientId, timestamp, isRead, contractIdReq, notificationId);

                case "CONTRACT_EXPIRATION":
                    int contractIdExp = reader.GetInt32(reader.GetOrdinal("contractID"));
                    DateTime expirationDate = reader.GetDateTime(reader.GetOrdinal("expirationDate"));
                    return new ContractExpirationNotification(recipientId, timestamp, isRead, contractIdExp, expirationDate, notificationId);

                default:
                    throw new ArgumentException($"Unknown notification category: {category}");
            }
        }
    }
}
