

create table [WaitListProduct]
(
	[waitListProductID] bigint identity primary key,

	[productID] bigint not null foreign key references [DummyProduct]([ID]),

	[availableAgain] DateTime not null foreign key references [Contract]([endDate])

);
