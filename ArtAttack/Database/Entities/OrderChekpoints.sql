--create TrackedOrders first

create table OrderCheckpoints
(
	[CheckpointID] int primary key identity(1,1),
	[Timestamp] datetime not null,
	[Location] varchar(255),
	[Description] varchar(255) not null,
	[CheckpointStatus] varchar(100) not null,
	constraint [OrderChekpointConstraint] 
		check ([CheckpointStatus] in ('PROCESSING','SHIPPED','IN_WAREHOUSE','IN_TRANSIT','OUT_FOR_DELIVERY','DELIVERED')),
	[TrackedOrderID] bigint foreign key references TrackedOrders([TrackedOrderID]) on delete cascade not null
)