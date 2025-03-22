--drop table [DummyCard]

create table [DummyCard](
	[ID] int identity primary key,
	[cardholderName] varchar(50),
	[cardNumber] varchar(20),
	[cvc] varchar(3),
	[month] varchar(2),
	[year] varchar(2),
	[country] varchar(30)

);