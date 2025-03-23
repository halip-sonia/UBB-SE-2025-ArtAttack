create table TrackedOrders
(
	[TrackedOrderID] bigint primary key identity(1,1),
	[EstimatedDeliveryDate] Date not null,
	[DeliveryAddress] varchar(255) not null,
	[OrderStatus] varchar(100) not null,
	constraint [TrackedOrderConstraint] 
		check ([OrderStatus] in ('PROCESSING','SHIPPED','IN_WAREHOUSE','IN_TRANSIT','OUT_FOR_DELIVERY','DELIVERED')),
	[OrderID] bigint foreign key references [Order]([OrderID]) unique not null
)
