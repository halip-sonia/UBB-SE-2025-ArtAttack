

create table [WaitListProduct]
(
	[waitListProductID] int identity primary key,

	[productID] int not null foreign key references [DummyProduct]([ID]),

	[availableAgain] DateTime not null foreign key references [DummyProduct]([endDate])

);
