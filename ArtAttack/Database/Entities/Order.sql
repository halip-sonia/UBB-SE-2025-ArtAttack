/*DummyProduct, DummyBuyer*/

create table [Order](
	[OrderID] bigint identity primary key,
	[ProductId] bigint not null foreign key references [DummyProduct]([ID]),
	[BuyerId] bigint not null foreign key references [DummyBuyer]([ID]),--Do we not concern ourselves with the seller anymore
	[OrderType] varchar(10) not null, 
    constraint [OrderTypeConstraint] check (OrderType in ('new', 'used', 'borrowed')),
    [PaymentMethod] varchar(10) not null,
    constraint [PaymentMethodConstraint] check (PaymentMethod in ('card', 'cash', 'wallet')),
	[OrderSummaryID] integer foreign key references [OrderSummary]([ID]),
	[OrderDate] timestamp default current_timestamp,
	[OrderHistoryID] bigint foreign key references [OrderHistory]([ID])

);

create table [OrderSummary](
    [ID] int  identity primary key,
    [subtotal] float ,
    [warrantyTax] float,
    [deliveryFee] float,
    [finalTotal] float,
    [fullName] varchar(255),--if the seller gets added there needs to be made a notification here as well
    [email] varchar(255),
    [phoneNumber] varchar(10),
    [address] varchar(255),
    [postalCode] varchar(255),
    [additionalInfo] varchar(255),
	[ContractDetails] text null --This is fine we will do a "manarie", and by that I mean another stored procedure
);

create table [OrderHistory](
	[ID] bigint identity primary key
);