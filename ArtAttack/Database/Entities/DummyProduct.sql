--drop table [DummyProduct]

create table [DummyProduct](
	[ID] integer identity primary key
	[price] float not null,
	[SellerID] integer foreign key references [DummySeller]([ID]), 
);