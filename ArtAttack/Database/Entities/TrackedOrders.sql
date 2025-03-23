--Order table must exist

create table TrackedOrders
(
	[TrackedOrderID] bigint primary key identity(1,1),
	[EstimatedDeliveryDate] Date not null,
	[DeliveryAddress] varchar(255) not null,
	[OrderStatus] varchar(100) not null,
	[OrderID] bigint foreign key references [Order]([OrderID]) unique not null
)