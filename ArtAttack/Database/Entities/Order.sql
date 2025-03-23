/*DummyProduct, DummyBuyer*/

create table [Order](
	[OrderID] bigint identity primary key,
	[ProductID] bigint not null foreign key references [DummyProduct]([ID]),
	[BuyerID] bigint not null foreign key references [DummyBuyer]([ID]),
    [OrderType] varchar(10) not null, 
    constraint [OrderTypeConstraint] check (OrderType in ('new', 'used', 'borrowed')),
 [PaymentMethod] varchar(10) not null,
    constraint [PaymentMethodConstraint] check (PaymentMethod in ('card', 'cash', 'wallet')),>>>>>>> main
	[OrderSummaryID] integer foreign key references [OrderSummary]([ID]),
	[OrderDate] timestamp default current_timestamp,
	[OrderHistoryID] bigint foreign key references [OrderHistory]([ID])

);

create table [OrderSummary](
    [ID] int  identity primary key,

    [Subtotal] float ,
    [WarrantyTax] float,
    [DeliveryFee] float,
    [FinalTotal] float,
    [FullName] varchar(255),
    [Email] varchar(255),
    [PhoneNumber] varchar(10),
    [Address] varchar(255),
    [PostalCode] varchar(255),
    [AdditionalInfo] varchar(255),
	[ContractDetails] text null

);

create table [OrderHistory](
	[ID] bigint identity primary key
);

go

create procedure get_borrowed_order_history @BuyerID bigint
as
begin
    select o.[OrderID], o.[ProductID], o.[OrderType], o.[PaymentMethod], 
    o.[OrderDate], os.[Subtotal], os.[WarrantyTax], os.[DeliveryFee], 
    os.[finalTotal], os.[address], os.[AdditionalInfo], os.[ContractDetails] , p.[ProductName]
    from [Order] o inner join [OrderSummary] os on o.[OrderSummaryID]=os.[ID]
    inner join [DummyProduct] p on o.[ProductID]=p.[ProductID]
    where o.[OrderType]='borrowed' and o.[BuyerID]=@BuyerID order by o.[OrderDate] desc;

end

go

create procedure get_new_or_used_order_history @BuyerID bigint
as
begin
    select o.[OrderID], o.[ProductID], o.[OrderType], o.[PaymentMethod], 
    o.[OrderDate], os.[Subtotal], os.[DeliveryFee], 
    os.[finalTotal], os.[address], os.[AdditionalInfo], p.[ProductName]
    from [Order] o inner join [OrderSummary] os on o.[OrderSummaryID]=os.[ID]
    inner join [DummyProduct] p on o.[ProductID]=p.[ProductID]
    where o.[OrderType]='new' or o.[OrderType]='used' and o.[BuyerID]=@BuyerID order by o.[OrderDate] desc;

end
go


create procedure get_orders_from_last_3_months @BuyerID bigint
as
begin
    select o.[OrderID], o.[ProductID], o.[OrderType], o.[PaymentMethod], 
    o.[OrderDate], os.[Subtotal], os.[WarrantyTax], os.[DeliveryFee], 
    os.[finalTotal], os.[address], os.[AdditionalInfo], os.[ContractDetails] , p.[ProductName]
    from [Order] o inner join [OrderSummary] os on o.[OrderSummaryID]=os.[ID]
    inner join [DummyProduct] p on o.[ProductID]=p.[ProductID]
    where o.[BuyerID]=@BuyerID and o.[OrderDate]>=dateadd(month, -3, getdate()) order by o.[OrderDate] desc;

end
go

create procedure get_orders_from_last_6_months @BuyerID bigint
as
begin
    select o.[OrderID], o.[ProductID], o.[OrderType], o.[PaymentMethod], 
    o.[OrderDate], os.[Subtotal], os.[WarrantyTax], os.[DeliveryFee], 
    os.[finalTotal], os.[address], os.[AdditionalInfo], os.[ContractDetails] , p.[ProductName]
    from [Order] o inner join [OrderSummary] os on o.[OrderSummaryID]=os.[ID]
    inner join [DummyProduct] p on o.[ProductID]=p.[ProductID]
    where o.[BuyerID]=@BuyerID and o.[OrderDate]>=dateadd(month, -6, getdate()) order by o.[OrderDate] desc;

end
go

create procedure get_orders_from_2025 @BuyerID bigint
as
begin
    select o.[OrderID], o.[ProductID], o.[OrderType], o.[PaymentMethod], 
    o.[OrderDate], os.[Subtotal], os.[WarrantyTax], os.[DeliveryFee], 
    os.[finalTotal], os.[address], os.[AdditionalInfo], os.[ContractDetails] , p.[ProductName]
    from [Order] o inner join [OrderSummary] os on o.[OrderSummaryID]=os.[ID]
    inner join [DummyProduct] p on o.[ProductID]=p.[ProductID]
    where o.[BuyerID]=@BuyerID and year(o.[OrderDate])=2025 order by o.[OrderDate] desc;

end

go

create procedure get_orders_from_2024 @BuyerID bigint
as
begin
    select o.[OrderID], o.[ProductID], o.[OrderType], o.[PaymentMethod], 
    o.[OrderDate], os.[Subtotal], os.[WarrantyTax], os.[DeliveryFee], 
    os.[finalTotal], os.[address], os.[AdditionalInfo], os.[ContractDetails], p.[ProductName]
    from [Order] o inner join [OrderSummary] os on o.[OrderSummaryID]=os.[ID]
    inner join [DummyProduct] p on o.[ProductID]=p.[ProductID]
    where o.[BuyerID]=@BuyerID and year(o.[OrderDate])=2024 order by o.[OrderDate] desc;

end
go

create procedure get_orders_by_name @BuyerID bigint, @text nvarchar(250)
as
begin
    select o.[OrderID], o.[ProductID], o.[OrderType], o.[PaymentMethod], 
    o.[OrderDate], os.[Subtotal], os.[WarrantyTax], os.[DeliveryFee], 
    os.[finalTotal], os.[address], os.[AdditionalInfo], os.[ContractDetails] , p.[ProductName]
    from [Order] o inner join [OrderSummary] os on o.[OrderSummaryID]=os.[ID]
    inner join [DummyProduct] p on o.[ProductID]=p.[ProductID]
    where o.[BuyerID]=@BuyerID and p.[ProductName] like '%@'+@text+'%' order by o.[OrderDate] desc;

end