--drop table [DummyProduct]

create table [DummyProduct](
	[ID] integer identity primary key,
  [name] varchar(64) not null,
	[price] float not null,
	[SellerID] integer foreign key references [DummySeller]([ID]), 

);