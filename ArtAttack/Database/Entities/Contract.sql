--drop table [Contract]

/*Before creating the contract table please create the :
Order, PDF and PredefinedContract Tables*/
create table [Contract](
	[ID] bigint identity primary key,

	[orderID] integer foreign key references [Order]([ID]),

	[startDate] DateTime,
	[endDate] DateTime,

	[contractStatus] Varchar(255) not null,
	constraint [ContractStatusConstraint] check ([contractStatus] in ('ACTIVE', 'RENEWED', 'EXPIRED')),
	[contractContent] TEXT,
	[renewalCount] integer not null,

	[predefinedContractID] integer foreign key references [PredefinedContract]([ID]),
	[pdfID] integer not null foreign key references [PDF]([ID]),

	/* Added field to support contract renewals.
	Stores a reference to the original contract being renewed 
	(can be NULL if it's a new/original contract). */
	[renewedFromContractID] bigint null 
);

go

/*
 Added foreign key constraint to enforce self-reference for renewals.
*/
alter table [Contract]
add constraint [FK_Contract_RenewedFrom]
foreign key ([renewedFromContractID]) REFERENCES [Contract]([ID]);
