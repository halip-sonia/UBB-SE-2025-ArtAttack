using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.Domain{  
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
        public ContractRenewalAnswerNotification(int notificationId, int recipientID, DateTime timestamp, int contractID, bool isAccepted) 
        {
            this.notificationID = notificationId;
            this.recipientID = recipientID;
            this.timestamp = timestamp;
            this.isRead = false;
            this.contractID = contractID;    
            category = NotificationCategory.CONTRACT_RENEWAL_ANS;
            this.isAccepted = isAccepted;
        }
        public int getContractID()
        {
            return contractID;
        }

        public override string getContent()
        {
            if (isAccepted ) {
                return "Contract:" + this.contractID + " has been renewed!\r\n You can download it from below!";
            }
            return "\"Unfortunately contract:\" + this.contractId + \" has not been renewed!\\r\\n The owner refused the renewal request :( \"";
        }

        public override string getTitle()
        {
            return "Contract Renewal Answer";
        }

        public override string getSubtitle()
        {
            return "You have recieved an answer on the renewal request for cotract: " + this.contractID + ".";
        }
    }


    public class ContractRenewalWaitlistNotification : Notification
    {
        private int productID;
        public ContractRenewalWaitlistNotification(int notificationId, int recipientID, DateTime timestamp, int productID)
        {
            this.notificationID = notificationId;
            this.recipientID = recipientID;
            this.timestamp = timestamp;
            this.isRead = false;
            this.productID = productID;
            category = NotificationCategory.CONTRACT_RENEWAL_WAITLIST;
        }

        public override string getContent()
        {
            return "The user that borrowed product:"+this.productID+ " that you are part of the waitlist for, has renewed it's contract. The time in which the product will be available will be extended, if this change doesn't fit your schedule you can update your participation in the waitlist.";
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


    public class OutbiddedNotification: Notification
    {
        private int productID;
        public OutbiddedNotification(int notificationId, int recipientId, DateTime timestamp, int productId)
        {
            this.notificationID = notificationId;
            this.recipientID = recipientId;
            this.timestamp = timestamp;
            this.isRead = false;
            this.productID = productId;
            category = NotificationCategory.OUTBIDDED;
        }
        public int getProductID()
        {
            return productID;
        }
        public override string getContent()
        {
            return "You've been outbidded!\nAnother buyer has placed a higher bid than you on product: " + this.productID + ", surpassing your offer. If you’re still interested, you can increase your bid before the auction ends. Don’t miss out on this opportunity!\r\n Place a new bid now!";
        }

        public override string getTitle()
        {
            return "Outbidded";
        }

        public override string getSubtitle()
        {
            return "You've been outbidded on product: " + this.productID + ".";
        }
    }

    public class OrderShippingProgressNotification: Notification
    {
        private int orderID;
        private string shippingState;
        private DateTime deliveryDate;

        public OrderShippingProgressNotification(int notificationId, int recipientId, DateTime timestamp, int id, string state, DateTime deliveryDate)
        {
            this.notificationID = notificationId;
            this.recipientID = recipientId;
            this.timestamp = timestamp;
            this.isRead = false;
            orderID = id;
            shippingState = state;
            category = NotificationCategory.ORDER_SHIPPING_PROGRESS;
            this.deliveryDate = deliveryDate;
        }

        public int getOrderID()
        {
            return orderID;
        }

        public string getShippingState()
        {
            return shippingState;
        }

        public override string getContent()
        {
            return "Great news! Your order:" + this.orderID + " has reached the " + this.shippingState + " stage. Estimated delivery is on" + this.deliveryDate + "Further updates will be provided along the procces.";
        }

        public override string getTitle()
        {
            return "Order Shipping Update";
        }

        public override string getSubtitle()
        {
            return "New info on order: " + this.orderID + " is available.";
        }

    }

    public class PaymentConfirmationNotification: Notification
    {
        private int productID;
        private int orderID;

        public PaymentConfirmationNotification(int notificationId, int recipientId, DateTime timestamp, int productId, int orderId)
        {
            this.notificationID = notificationId;
            this.recipientID = recipientId;
            this.timestamp = timestamp;
            this.isRead = false;
            this.productID = productId;
            this.orderID = orderId;
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
            return "Thank You for Your Purchase!\r\nYour order:" + this.orderID + " for product:" + this.productID + " has been successfully processed. You can view your order details in your account. If you have any questions, feel free to reach out to our support team.";
        }

        public override string getTitle()
        {
            return "Payment Confirmation";
        }

        public override string getSubtitle()
        {
            return "Order:" + this.orderID + "has been processed successfuly!";
        }

    }

    public class ProductRemovedNotification: Notification
    {
        private int productID;
        
        public ProductRemovedNotification(int notificationId, int recipientId, DateTime timestamp, int productId)
        {
            this.notificationID = notificationId;
            this.recipientID = recipientId;
            this.timestamp = timestamp;
            this.productID = productId;
            this.isRead=false;
            category = NotificationCategory.PRODUCT_REMOVED;
        }

        public int getProductID()
        {
            return this.productID;
        }

        public override string getContent()
        {
            return "Unfortunatelly the Product: " + this.productID + " that you were waiting for was removed form the martketplace by the seller. We encourage you to search for similar items on the app, we are sure you will find something that fits your needs!";
        }

        public override string getTitle()
        {
            return "Product removed";
        }

        public override string getSubtitle()
        {
            return "Product: " + this.productID + " was removed  from the marketplace!";
        }

    }

    public class ProductAvailableNotification : Notification
    {
        private int productID;

        public ProductAvailableNotification(int notificationId, int recipientId, DateTime timestamp, int productId)
        {
            this.notificationID = notificationId;
            this.recipientID = recipientId;
            this.timestamp = timestamp;
            this.productID = productId;
            this.isRead = false;
            category = NotificationCategory.PRODUCT_AVAILABLE;
        }

        public int getProductID()
        {
            return this.productID;
        }

        public override string getContent()
        {
            return "Good news!The Product: " + this.productID + " that you were waiting for is now back in stock. Don’t miss your chance to grab it before it sells out again!\nOrder now before it’s gone";
        }

        public override string getTitle()
        {
            return "Product available";
        }

        public override string getSubtitle()
        {
            return "Product: " + this.productID + " is available now!";
        }

    }

    public class ContractRenewalRequestNotification : Notification
    {
        private int contractID;

        public ContractRenewalRequestNotification(int notificationId, int recipientId, DateTime timestamp, int contractId)
        {
            this.notificationID = notificationId;
            this.recipientID = recipientId;
            this.timestamp = timestamp;
            this.contractID = contractId;
            this.isRead = false;
            category = NotificationCategory.CONTRACT_RENEWAL_REQ;
        }

        public int getContractID()
        {
            return this.contractID;
        }

        public override string getContent()
        {
            return "User: " + this.recipientID + " would like to renew contract: " + this.contractID + " !\n Please accept or deny this proposal as soon as posible, such that the waitlist will be updated corespondingly.";
        }

        public override string getTitle()
        {
            return "Contract Renewal Request";
        }

        public override string getSubtitle()
        {
            return "User: " + this.recipientID + " wants to renew contract: "+ this.contractID +" !";
        }

    }

    public class ContractExpirationNotification : Notification
    {
        private int contractID;
        private DateTime expirationDate;

        public ContractExpirationNotification(int notificationId, int recipientId, DateTime timestamp, int contractId, DateTime expirationDate)
        {
            this.notificationID = notificationId;
            this.recipientID = recipientId;
            this.timestamp = timestamp;
            this.contractID = contractId;
            this.isRead = false;
            category = NotificationCategory.CONTRACT_EXPIRATION;
            this.expirationDate = expirationDate;
        }

        public int getContractID()
        {
            return this.contractID;
        }

        public override string getContent()
        {
            return "Contract: " + this.contractID + " is set to expire on " + this.expirationDate + "!\n Please check out the sellers contract renewal policies for more information.";
        }

        public override string getTitle()
        {
            return "Contract Expiration";
        }

        public override string getSubtitle()
        {
            return "Contract: " + this.contractID + " is about to expire!";

        }

    }

}
