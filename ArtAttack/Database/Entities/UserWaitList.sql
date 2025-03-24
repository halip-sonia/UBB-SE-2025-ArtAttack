create table [UserWaitList]
(
	[UserWaitListID] bigint identity primary key,

	[productWaitListID] bigint not null foreign key references [WaitListProduct]([WaitListProductID]),

	[userID] integer not null foreign key references [DummyBuyer]([ID]),

	[joinedTime] DateTime not null,

	[positionInQueue] int not null
);

