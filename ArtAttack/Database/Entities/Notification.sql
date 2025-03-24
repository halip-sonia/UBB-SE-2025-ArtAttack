--drop table [Notification]

create table [Notification](
	[notificationID] int identity primary key not null,
	[recipientID] int not null,
	[category] varchar(25) not null, 
	constraint [NotificationCategoryConstraint]check (category in('CONTRACT_EXPIRATION', 'OUTBIDDED','ORDER_SHIPPING_PROGRESS','PRODUCT_AVAILABLE','PAYMENT_CONFIRMATION','PRODUCT_REMOVED','CONTRACT_RENEWAL_REQ','CONTRACT_RENEWAL_ANS')),
	[timestamp] DateTime not null,
	[isRead] bit,
	--optional fields
	[contractID] int foreign key references [DummyContract]([ID]),
	[isAccepted] smallint check (isRead in (0,1)),
	[productID] int,
	[orderID] int,
	[shippingState] varchar(25),
	[deliveryDate] Datetime,
	[expirationDate] Datetime,
);
