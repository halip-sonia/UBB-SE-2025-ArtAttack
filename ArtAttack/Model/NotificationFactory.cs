using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                    return new ContractRenewalAnswerNotification(
                        notificationId, recipientId, timestamp, contractId, isAccepted);

                case "CONTRACT_RENEWAL_WAITLIST":
                    int productIdWaitlist = reader.GetInt32(reader.GetOrdinal("productID"));
                    return new ContractRenewalWaitlistNotification(
                        notificationId, recipientId, timestamp, productIdWaitlist);

                case "OUTBIDDED":
                    int productIdOutbidded = reader.GetInt32(reader.GetOrdinal("productID"));
                    return new OutbiddedNotification(
                        notificationId, recipientId, timestamp, productIdOutbidded);

                case "ORDER_SHIPPING_PROGRESS":
                    int orderId = reader.GetInt32(reader.GetOrdinal("orderID"));
                    string shippingState = reader.GetString(reader.GetOrdinal("shippingState"));
                    DateTime deliveryDate = reader.GetDateTime(reader.GetOrdinal("deliveryDate"));
                    return new OrderShippingProgressNotification(
                        notificationId, recipientId, timestamp, orderId, shippingState, deliveryDate);

                case "PAYMENT_CONFIRMATION":
                    int productIdPayment = reader.GetInt32(reader.GetOrdinal("productID"));
                    int orderIdPayment = reader.GetInt32(reader.GetOrdinal("orderID"));
                    return new PaymentConfirmationNotification(
                        notificationId, recipientId, timestamp, productIdPayment, orderIdPayment);

                case "PRODUCT_REMOVED":
                    int productIdRemoved = reader.GetInt32(reader.GetOrdinal("productID"));
                    return new ProductRemovedNotification(
                        notificationId, recipientId, timestamp, productIdRemoved);

                case "PRODUCT_AVAILABLE":
                    int productIdAvailable = reader.GetInt32(reader.GetOrdinal("productID"));
                    return new ProductAvailableNotification(
                        notificationId, recipientId, timestamp, productIdAvailable);

                case "CONTRACT_RENEWAL_REQ":
                    int contractIdReq = reader.GetInt32(reader.GetOrdinal("contractID"));
                    return new ContractRenewalRequestNotification(
                        notificationId, recipientId, timestamp, contractIdReq);

                case "CONTRACT_EXPIRATION":
                    int contractIdExp = reader.GetInt32(reader.GetOrdinal("contractID"));
                    DateTime expirationDate = reader.GetDateTime(reader.GetOrdinal("expirationDate"));
                    return new ContractExpirationNotification(
                        notificationId, recipientId, timestamp, contractIdExp, expirationDate);

                default:
                    throw new ArgumentException($"Unknown notification category: {category}");
            }
        }
    }
}
