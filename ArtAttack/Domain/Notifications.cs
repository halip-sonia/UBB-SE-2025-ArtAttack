using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.Domain
{
    abstract public class Notification
    {
        protected int notificationID;
        protected int recipientID;
        protected NotificationCategory category;
        protected DateTime timestamp;
        protected bool isRead;

        public int getNotificationID()
        {
            return notificationID;
        }

        public int getRecipientID()
        {
            return recipientID;
        }

        public NotificationCategory getCategory()
        {
            return category;
        }

        public DateTime getTimestamp()
        {
            return timestamp;
        }

        public bool getIsRead()
        {
            return isRead;
        }
        public void markAsRead()
        {
            isRead = true;
        }
        public abstract string getContent();
        public abstract string getTitle();
        public abstract string getSubtitle();

    }

    //TODO
    public class ContractRenewalAnswerNotification : Notification
    {
        private int contractID;
        private bool isAccepted;
        public ContractRenewalAnswerNotification(int recipientID, DateTime timestamp, bool isRead, int contractID, bool isAccepted, int notificationId = 0)
        {
            notificationID = notificationId;
            this.recipientID = recipientID;
            this.timestamp = timestamp;
            this.isRead = isRead;
            this.contractID = contractID;
            category = NotificationCategory.CONTRACT_RENEWAL_ANS;
            this.isAccepted = isAccepted;
        }
        public int getContractID()
        {
            return contractID;
        }

        public bool getIsAccepted()
        {
            return isAccepted;
        }

        public override string getContent()
        {
            if (isAccepted)
            {
                return "Contract:" + contractID + " has been renewed!\r\n You can download it from below!";
            }
            return "\"Unfortunately contract:\" + this.contractId + \" has not been renewed!\\r\\n The owner refused the renewal request :( \"";
        }

        public override string getTitle()
        {
            return "Contract Renewal Answer";
        }

        public override string getSubtitle()
        {
            return "You have recieved an answer on the renewal request for cotract: " + contractID + ".";
        }
    }


    public class ContractRenewalWaitlistNotification : Notification
    {
        private int productID;
        public ContractRenewalWaitlistNotification(int recipientID, DateTime timestamp, bool isRead, int productID, int notificationId = 0)
        {
            notificationID = notificationId;
            this.recipientID = recipientID;
            this.timestamp = timestamp;
            this.isRead = isRead;
            this.productID = productID;
            category = NotificationCategory.CONTRACT_RENEWAL_WAITLIST;
        }

        public int getProductID()
        {
            return productID;
        }

        public override string getContent()
        {
            return "The user that borrowed product:" + productID + " that you are part of the waitlist for, has renewed it's contract. The time in which the product will be available will be extended, if this change doesn't fit your schedule you can update your participation in the waitlist.";
        }

        public override string getTitle()
        {
            return "Contract Renewal in Waitlist";
        }

        public override string getSubtitle()
        {
            return "A user has extended it's contract.";
        }
    }


    public class OutbiddedNotification : Notification
    {
        private int productID;
        public OutbiddedNotification(int recipientId, DateTime timestamp, bool isRead, int productId, int notificationId = 0)
        {
            notificationID = notificationId;
            recipientID = recipientId;
            this.timestamp = timestamp;
            this.isRead = isRead;
            productID = productId;
            category = NotificationCategory.OUTBIDDED;
        }
        public int getProductID()
        {
            return productID;
        }
        public override string getContent()
        {
            return "You've been outbidded!\nAnother buyer has placed a higher bid than you on product: " + productID + ", surpassing your offer. If you’re still interested, you can increase your bid before the auction ends. Don’t miss out on this opportunity!\r\n Place a new bid now!";
        }

        public override string getTitle()
        {
            return "Outbidded";
        }

        public override string getSubtitle()
        {
            return "You've been outbidded on product: " + productID + ".";
        }
    }

    public class OrderShippingProgressNotification : Notification
    {
        private int orderID;
        private string shippingState;
        private DateTime deliveryDate;

        public OrderShippingProgressNotification(int recipientId, DateTime timestamp, bool isRead, int id, string state, DateTime deliveryDate, int notificationId = 0)
        {
            notificationID = notificationId;
            recipientID = recipientId;
            this.timestamp = timestamp;
            this.isRead = isRead;
            orderID = id;
            shippingState = state;
            category = NotificationCategory.ORDER_SHIPPING_PROGRESS;
            this.deliveryDate = deliveryDate;
        }

        public int getOrderID()
        {
            return orderID;
        }

        public DateTime getDeliveryDate()
        {
            return deliveryDate;
        }

        public string getShippingState()
        {
            return shippingState;
        }

        public override string getContent()
        {
            return "Great news! Your order:" + orderID + " has reached the " + shippingState + " stage. Estimated delivery is on" + deliveryDate + "Further updates will be provided along the procces.";
        }

        public override string getTitle()
        {
            return "Order Shipping Update";
        }

        public override string getSubtitle()
        {
            return "New info on order: " + orderID + " is available.";
        }

    }

    public class PaymentConfirmationNotification : Notification
    {
        private int productID;
        private int orderID;

        public PaymentConfirmationNotification(int recipientId, DateTime timestamp, bool isRead, int productId, int orderId, int notificationId = 0)
        {
            notificationID = notificationId;
            recipientID = recipientId;
            this.timestamp = timestamp;
            this.isRead = isRead;
            productID = productId;
            orderID = orderId;
            category = NotificationCategory.PAYMENT_CONFIRMATION;
        }

        public int getProductID()
        {
            return productID;
        }

        public int getOrderID()
        {
            return orderID;
        }

        public override string getContent()
        {
            return "Thank You for Your Purchase!\r\nYour order:" + orderID + " for product:" + productID + " has been successfully processed. You can view your order details in your account. If you have any questions, feel free to reach out to our support team.";
        }

        public override string getTitle()
        {
            return "Payment Confirmation";
        }

        public override string getSubtitle()
        {
            return "Order:" + orderID + "has been processed successfuly!";
        }

    }

    public class ProductRemovedNotification : Notification
    {
        private int productID;

        public ProductRemovedNotification(int recipientId, DateTime timestamp, bool isRead, int productId, int notificationId = 0)
        {
            notificationID = notificationId;
            recipientID = recipientId;
            this.timestamp = timestamp;
            productID = productId;
            this.isRead = isRead;
            category = NotificationCategory.PRODUCT_REMOVED;
        }

        public int getProductID()
        {
            return productID;
        }

        public override string getContent()
        {
            return "Unfortunatelly the Product: " + productID + " that you were waiting for was removed form the martketplace by the seller. We encourage you to search for similar items on the app, we are sure you will find something that fits your needs!";
        }

        public override string getTitle()
        {
            return "Product removed";
        }

        public override string getSubtitle()
        {
            return "Product: " + productID + " was removed  from the marketplace!";
        }

    }

    public class ProductAvailableNotification : Notification
    {
        private int productID;

        public ProductAvailableNotification(int recipientId, DateTime timestamp, bool isRead, int productId, int notificationId = 0)
        {
            notificationID = notificationId;
            recipientID = recipientId;
            this.timestamp = timestamp;
            productID = productId;
            this.isRead = isRead;
            category = NotificationCategory.PRODUCT_AVAILABLE;
        }

        public int getProductID()
        {
            return productID;
        }

        public override string getContent()
        {
            return "Good news!The Product: " + productID + " that you were waiting for is now back in stock. Don’t miss your chance to grab it before it sells out again!\nOrder now before it’s gone";
        }

        public override string getTitle()
        {
            return "Product available";
        }

        public override string getSubtitle()
        {
            return "Product: " + productID + " is available now!";
        }

    }

    public class ContractRenewalRequestNotification : Notification
    {
        private int contractID;

        public ContractRenewalRequestNotification(int recipientId, DateTime timestamp, bool isRead, int contractId, int notificationId = 0)
        {
            notificationID = notificationId;
            recipientID = recipientId;
            this.timestamp = timestamp;
            contractID = contractId;
            this.isRead = isRead;
            category = NotificationCategory.CONTRACT_RENEWAL_REQ;
        }

        public int getContractID()
        {
            return contractID;
        }

        public override string getContent()
        {
            return "User: " + recipientID + " would like to renew contract: " + contractID + " !\n Please accept or deny this proposal as soon as posible, such that the waitlist will be updated corespondingly.";
        }

        public override string getTitle()
        {
            return "Contract Renewal Request";
        }

        public override string getSubtitle()
        {
            return "User: " + recipientID + " wants to renew contract: " + contractID + " !";
        }

    }

    public class ContractExpirationNotification : Notification
    {
        private int contractID;
        private DateTime expirationDate;

        public ContractExpirationNotification(int recipientId, DateTime timestamp, bool isRead, int contractId, DateTime expirationDate, int notificationId = 0)
        {
            notificationID = notificationId;
            recipientID = recipientId;
            this.timestamp = timestamp;
            contractID = contractId;
            this.isRead = isRead;
            category = NotificationCategory.CONTRACT_EXPIRATION;
            this.expirationDate = expirationDate;
        }

        public int getContractID()
        {
            return contractID;
        }

        public override string getContent()
        {
            return "Contract: " + contractID + " is set to expire on " + expirationDate + "!\n Please check out the sellers contract renewal policies for more information.";
        }

        public override string getTitle()
        {
            return "Contract Expiration";
        }

        public override string getSubtitle()
        {
            return "Contract: " + contractID + " is about to expire!";

        }

    }


}
