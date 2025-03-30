/****************************
* 1. DummyBuyer
*****************************/
INSERT INTO DummyBuyer (name) VALUES ('Alice Johnson');
INSERT INTO DummyBuyer (name) VALUES ('Bob Smith');
INSERT INTO DummyBuyer (name) VALUES ('Carol Davis');
INSERT INTO DummyBuyer (name) VALUES ('David Wilson');
INSERT INTO DummyBuyer (name) VALUES ('Eva Brown');

/****************************
* 2. DummySeller
*****************************/
INSERT INTO DummySeller (name) VALUES ('Seller One');
INSERT INTO DummySeller (name) VALUES ('Seller Two');
INSERT INTO DummySeller (name) VALUES ('Seller Three');
INSERT INTO DummySeller (name) VALUES ('Seller Four');
INSERT INTO DummySeller (name) VALUES ('Seller Five');

/****************************
* 3. DummyCard
*****************************/
INSERT INTO DummyCard (cardholderName, cardNumber, cvc, month, year, country, balance) 
  VALUES ('Alice Johnson', '4111111111111111', '123', '01', '25', 'USA', 1500.00);
INSERT INTO DummyCard (cardholderName, cardNumber, cvc, month, year, country, balance) 
  VALUES ('Bob Smith', '4222222222222222', '234', '02', '26', 'USA', 2000.00);
INSERT INTO DummyCard (cardholderName, cardNumber, cvc, month, year, country, balance) 
  VALUES ('Carol Davis', '4333333333333333', '345', '03', '27', 'Canada', 1800.00);
INSERT INTO DummyCard (cardholderName, cardNumber, cvc, month, year, country, balance) 
  VALUES ('David Wilson', '4444444444444444', '456', '04', '28', 'UK', 2200.00);
INSERT INTO DummyCard (cardholderName, cardNumber, cvc, month, year, country, balance) 
  VALUES ('Eva Brown', '4555555555555555', '567', '05', '29', 'Australia', 1700.00);

/****************************
* 4. DummyWallet
*****************************/
INSERT INTO DummyWallet (balance) VALUES (500.00);
INSERT INTO DummyWallet (balance) VALUES (750.00);
INSERT INTO DummyWallet (balance) VALUES (300.00);
INSERT INTO DummyWallet (balance) VALUES (950.00);
INSERT INTO DummyWallet (balance) VALUES (600.00);

/****************************
* 5. PDF
*****************************/
-- Using sample hex binary data for file content
INSERT INTO PDF ([file]) VALUES (0xCAFEBABE);
INSERT INTO PDF ([file]) VALUES (0xDEADBEEF);
INSERT INTO PDF ([file]) VALUES (0xB16B00B5);
INSERT INTO PDF ([file]) VALUES (0xFEEDFACE);
INSERT INTO PDF ([file]) VALUES (0xC001D00D);

/****************************
* 6. OrderSummary
*****************************/
INSERT INTO OrderSummary 
  (Subtotal, WarrantyTax, DeliveryFee, FinalTotal, FullName, Email, PhoneNumber, Address, PostalCode, AdditionalInfo, ContractDetails)
  VALUES (100.00, 5.00, 10.00, 115.00, 'Alice Johnson', 'alice@example.com', '1234567890', '123 Main St', 'A1B2C3', 'Leave at door', 'Standard contract');
INSERT INTO OrderSummary 
  (Subtotal, WarrantyTax, DeliveryFee, FinalTotal, FullName, Email, PhoneNumber, Address, PostalCode, AdditionalInfo, ContractDetails)
  VALUES (200.00, 10.00, 15.00, 225.00, 'Bob Smith', 'bob@example.com', '2345678901', '456 Oak Ave', 'D4E5F6', 'Ring bell', 'Premium contract');
INSERT INTO OrderSummary 
  (Subtotal, WarrantyTax, DeliveryFee, FinalTotal, FullName, Email, PhoneNumber, Address, PostalCode, AdditionalInfo, ContractDetails)
  VALUES (150.00, 7.50, 12.00, 169.50, 'Carol Davis', 'carol@example.com', '3456789012', '789 Pine Rd', 'G7H8I9', 'Call on arrival', 'Standard contract');
INSERT INTO OrderSummary 
  (Subtotal, WarrantyTax, DeliveryFee, FinalTotal, FullName, Email, PhoneNumber, Address, PostalCode, AdditionalInfo, ContractDetails)
  VALUES (250.00, 12.50, 20.00, 282.50, 'David Wilson', 'david@example.com', '4567890123', '321 Maple Ln', 'J1K2L3', 'Back door', 'Premium contract');
INSERT INTO OrderSummary 
  (Subtotal, WarrantyTax, DeliveryFee, FinalTotal, FullName, Email, PhoneNumber, Address, PostalCode, AdditionalInfo, ContractDetails)
  VALUES (300.00, 15.00, 25.00, 340.00, 'Eva Brown', 'eva@example.com', '5678901234', '654 Cedar Blvd', 'M4N5O6', 'Front desk', 'Standard contract');

/****************************
* 7. OrderHistory
*****************************/
INSERT INTO OrderHistory DEFAULT VALUES;
INSERT INTO OrderHistory DEFAULT VALUES;
INSERT INTO OrderHistory DEFAULT VALUES;
INSERT INTO OrderHistory DEFAULT VALUES;
INSERT INTO OrderHistory DEFAULT VALUES;

/****************************
* 8. DummyProduct
*****************************/
-- Assumes SellerIDs 1 to 5 exist from DummySeller table.
INSERT INTO DummyProduct (name, price, SellerID, productType, startDate, endDate)
  VALUES ('Product A', 49.99, 1, 'Electronics', '2025-03-28', '2025-12-31');
INSERT INTO DummyProduct (name, price, SellerID, productType, startDate, endDate)
  VALUES ('Product B', 79.99, 2, 'Books', '2025-03-28', '2025-12-31');
INSERT INTO DummyProduct (name, price, SellerID, productType, startDate, endDate)
  VALUES ('Product C', 29.99, 3, 'Clothing', '2025-03-28', '2025-12-31');
INSERT INTO DummyProduct (name, price, SellerID, productType, startDate, endDate)
  VALUES ('Product D', 99.99, 4, 'Home', '2025-03-28', '2025-12-31');
INSERT INTO DummyProduct (name, price, SellerID, productType, startDate, endDate)
  VALUES ('Product E', 59.99, 5, 'Garden', '2025-03-28', '2025-12-31');

/****************************
* 9. Order
*****************************/
-- Note: ProductType is a foreign key to DummyProduct so we use a product ID.
-- OrderSummaryID and OrderHistoryID are assumed to be 1 to 5.
INSERT INTO [Order] (ProductID, BuyerID, ProductType, PaymentMethod, OrderSummaryID, OrderDate, OrderHistoryID)
  VALUES (1, 1, 1, 'card', 1, '2025-03-28', 1);
INSERT INTO [Order] (ProductID, BuyerID, ProductType, PaymentMethod, OrderSummaryID, OrderDate, OrderHistoryID)
  VALUES (2, 2, 2, 'cash', 2, '2025-03-28', 2);
INSERT INTO [Order] (ProductID, BuyerID, ProductType, PaymentMethod, OrderSummaryID, OrderDate, OrderHistoryID)
  VALUES (3, 3, 3, 'wallet', 3, '2025-03-28', 3);
INSERT INTO [Order] (ProductID, BuyerID, ProductType, PaymentMethod, OrderSummaryID, OrderDate, OrderHistoryID)
  VALUES (4, 4, 4, 'card', 4, '2025-03-28', 4);
INSERT INTO [Order] (ProductID, BuyerID, ProductType, PaymentMethod, OrderSummaryID, OrderDate, OrderHistoryID)
  VALUES (5, 5, 5, 'cash', 5, '2025-03-28', 5);

/****************************
* 10. Contract
*****************************/
-- For predefinedContractID we use NULL since we are excluding PredefinedContract inserts.
-- pdfID references PDF table and orderID references Order.

DELETE FROM Contract;
  DBCC CHECKIDENT ('Contract', RESEED, 0);
INSERT INTO Contract (orderID, contractStatus, contractContent, renewalCount, predefinedContractID, pdfID, renewedFromContractID)
  VALUES (1, 'ACTIVE', 'Contract for order 1', 0, NULL, 1, NULL);
INSERT INTO Contract (orderID, contractStatus, contractContent, renewalCount, predefinedContractID, pdfID, renewedFromContractID)
  VALUES (2, 'EXPIRED', 'Contract for order 2', 1, NULL, 2, NULL);
INSERT INTO Contract (orderID, contractStatus, contractContent, renewalCount, predefinedContractID, pdfID, renewedFromContractID)
  VALUES (3, 'RENEWED', 'Contract for order 3', 2, NULL, 3, NULL);
INSERT INTO Contract (orderID, contractStatus, contractContent, renewalCount, predefinedContractID, pdfID, renewedFromContractID)
  VALUES (4, 'ACTIVE', 'Contract for order 4', 0, NULL, 4, NULL);
INSERT INTO Contract (orderID, contractStatus, contractContent, renewalCount, predefinedContractID, pdfID, renewedFromContractID)
  VALUES (5, 'ACTIVE', 'Contract for order 5', 0, NULL, 5, NULL);
  Select * from Contract;
  Select * from [Order];

  update [Contract] set orderID = 2 where ID=1

  
/****************************
* 11. Notification
*****************************/
-- Using various allowed categories and assuming recipientID corresponds to DummyBuyer,
-- contractID from Contract, productID from DummyProduct and orderID from Order.
INSERT INTO Notification (recipientID, category, timestamp, isRead, contractID, isAccepted, productID, orderID, shippingState, deliveryDate, expirationDate)
  VALUES (1, 'PAYMENT_CONFIRMATION', '2025-03-28 10:05:00', 0, 1, 1, 1, 1, 'CA', '2025-03-30', '2025-04-30');
INSERT INTO Notification (recipientID, category, timestamp, isRead, contractID, isAccepted, productID, orderID, shippingState, deliveryDate, expirationDate)
  VALUES (2, 'CONTRACT_EXPIRATION', '2025-03-28 11:05:00', 0, 2, 0, 2, 2, 'NY', '2025-03-31', '2025-05-01');
INSERT INTO Notification (recipientID, category, timestamp, isRead, contractID, isAccepted, productID, orderID, shippingState, deliveryDate, expirationDate)
  VALUES (3, 'ORDER_SHIPPING_PROGRESS', '2025-03-28 12:05:00', 1, 3, 1, 3, 3, 'TX', '2025-04-01', '2025-05-02');
INSERT INTO Notification (recipientID, category, timestamp, isRead, contractID, isAccepted, productID, orderID, shippingState, deliveryDate, expirationDate)
  VALUES (4, 'PRODUCT_AVAILABLE', '2025-03-28 13:05:00', 0, 4, 0, 4, 4, 'FL', '2025-04-02', '2025-05-03');
INSERT INTO Notification (recipientID, category, timestamp, isRead, contractID, isAccepted, productID, orderID, shippingState, deliveryDate, expirationDate)
  VALUES (5, 'CONTRACT_RENEWAL_REQ', '2025-03-28 14:05:00', 1, 5, 1, 5, 5, 'IL', '2025-04-03', '2025-05-04');

/****************************
* 12. TrackedOrders
*****************************/
-- Each TrackedOrder must reference a unique OrderID from the Order table.
INSERT INTO TrackedOrders (EstimatedDeliveryDate, DeliveryAddress, OrderStatus, OrderID)
  VALUES ('2025-04-05', '123 Main St', 'PROCESSING', 1);
INSERT INTO TrackedOrders (EstimatedDeliveryDate, DeliveryAddress, OrderStatus, OrderID)
  VALUES ('2025-04-06', '456 Oak Ave', 'SHIPPED', 2);
INSERT INTO TrackedOrders (EstimatedDeliveryDate, DeliveryAddress, OrderStatus, OrderID)
  VALUES ('2025-04-07', '789 Pine Rd', 'IN_WAREHOUSE', 3);
INSERT INTO TrackedOrders (EstimatedDeliveryDate, DeliveryAddress, OrderStatus, OrderID)
  VALUES ('2025-04-08', '321 Maple Ln', 'IN_TRANSIT', 4);
INSERT INTO TrackedOrders (EstimatedDeliveryDate, DeliveryAddress, OrderStatus, OrderID)
  VALUES ('2025-04-09', '654 Cedar Blvd', 'OUT_FOR_DELIVERY', 5);

/****************************
* 13. OrderCheckpoints
*****************************/
-- Each checkpoint is linked to a TrackedOrder (IDs 1 to 5).
INSERT INTO OrderCheckpoints (Timestamp, Location, Description, CheckpointStatus, TrackedOrderID)
  VALUES ('2025-03-29 09:00:00', 'Warehouse', 'Order received at warehouse', 'PROCESSING', 1);
INSERT INTO OrderCheckpoints (Timestamp, Location, Description, CheckpointStatus, TrackedOrderID)
  VALUES ('2025-03-29 12:00:00', 'Distribution Center', 'Order dispatched', 'SHIPPED', 2);
INSERT INTO OrderCheckpoints (Timestamp, Location, Description, CheckpointStatus, TrackedOrderID)
  VALUES ('2025-03-30 08:00:00', 'Local Hub', 'Order arrived at local hub', 'IN_WAREHOUSE', 3);
INSERT INTO OrderCheckpoints (Timestamp, Location, Description, CheckpointStatus, TrackedOrderID)
  VALUES ('2025-03-30 10:00:00', 'On the way', 'Order in transit', 'IN_TRANSIT', 4);
INSERT INTO OrderCheckpoints (Timestamp, Location, Description, CheckpointStatus, TrackedOrderID)
  VALUES ('2025-03-30 15:00:00', 'Customer Area', 'Out for delivery', 'OUT_FOR_DELIVERY', 5);

/****************************
* 14. WaitListProduct
*****************************/
-- Each wait list product references a product from DummyProduct.
INSERT INTO WaitListProduct (productID, availableAgain) VALUES (1, '2025-04-10 09:00:00');
INSERT INTO WaitListProduct (productID, availableAgain) VALUES (2, '2025-04-11 10:00:00');
INSERT INTO WaitListProduct (productID, availableAgain) VALUES (3, '2025-04-12 11:00:00');
INSERT INTO WaitListProduct (productID, availableAgain) VALUES (4, '2025-04-13 12:00:00');
INSERT INTO WaitListProduct (productID, availableAgain) VALUES (5, '2025-04-14 13:00:00');

/****************************
* 15. UserWaitList
*****************************/
-- Each UserWaitList references a WaitListProduct and a DummyBuyer.
INSERT INTO UserWaitList (productWaitListID, userID, joinedTime, positionInQueue)
  VALUES (1, 1, '2025-03-28 09:30:00', 1);
INSERT INTO UserWaitList (productWaitListID, userID, joinedTime, positionInQueue)
  VALUES (2, 2, '2025-03-28 09:35:00', 2);
INSERT INTO UserWaitList (productWaitListID, userID, joinedTime, positionInQueue)
  VALUES (3, 3, '2025-03-28 09:40:00', 3);
INSERT INTO UserWaitList (productWaitListID, userID, joinedTime, positionInQueue)
  VALUES (4, 4, '2025-03-28 09:45:00', 4);
INSERT INTO UserWaitList (productWaitListID, userID, joinedTime, positionInQueue)
  VALUES (5, 5, '2025-03-28 09:50:00', 5);
