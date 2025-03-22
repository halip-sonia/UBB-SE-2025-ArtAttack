--drop table [PredefinedContract]

create table [PredefinedContract](
	[ID] integer identity primary key,
	[content] TEXT not null,
	--contract type should be expunged from Class Diagram as it is held by the Order Summary Table
);