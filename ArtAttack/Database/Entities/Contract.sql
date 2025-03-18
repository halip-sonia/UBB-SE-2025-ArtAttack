--drop table [Contract]

/*Before creating the contract table please create the :
DummySeller, DummyBuyer, DummyProduct, PDF and PredefinedContract Tables*/
create table [Contract](
	[ID] bigint identity primary key,
	[price] float not null,--This field might be redundant as I can take the price from the Purchase Table.

	[sellerID] integer foreign key references [DummySeller]([ID]),
	[buyerID] integer foreign key references [DummyBuyer]([ID]),
	[productID] integer not null foreign key references [DummyProduct]([ID]),

	[startDate] DateTime not null,
	[endDate] DateTime not null,
	[contractStatus] Varchar(255) not null,
	constraint [ContractStatusConstraint] check ([contractStatus] in ('ACTIVE', 'RENEWED', 'EXPIRED')),
	[contractContent] TEXT,
	[renewalCount] integer not null,

	[predefinedContractID] integer foreign key references [PredefinedContract]([ID]),
	[pdfID] integer not null foreign key references [PDF]([ID]),
);