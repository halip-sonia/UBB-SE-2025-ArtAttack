--drop table [DummyBuyer]

create table [DummyBuyer](
	[ID] integer identity primary key,
	[name] varchar(64) not null
);