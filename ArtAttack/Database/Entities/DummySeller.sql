--drop table [DummySeller]

create table [DummySeller](
	[ID] integer identity primary key,
	[name] varchar(64) not null
);