--drop table [DummyCard]

create table [DummyCard](
	[ID] int identity primary key,
	[cardholderName] varchar(50) not null,
	[cardNumber] varchar(20) not null,
	[cvc] varchar(3) not null,
	[month] varchar(2) not null,
	[year] varchar(2) not null,
	[country] varchar(30) not null,
	[balance] float 

);