/*DummyProduct, DummyBuyer*/

create table [Order](
	[OrderID] int identity primary key,
	[ProductID] int not null foreign key references [DummyProduct]([ID]),
	[BuyerID] int not null foreign key references [DummyBuyer]([ID]),
    [ProductType] varchar(20) not null foreign key references [DummyProduct]([ProductType]),
    
    constraint [PaymentMethodConstraint] check (PaymentMethod in ('card', 'cash', 'wallet')),
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
	[ID] int identity primary key
);

go

create procedure get_borrowed_order_history @BuyerID int
as
begin
    select o.[OrderID], o.[ProductID], o.[ProductType], o.[PaymentMethod], 
    o.[OrderDate], os.[Subtotal], os.[WarrantyTax], os.[DeliveryFee], 
    os.[finalTotal], os.[address], os.[AdditionalInfo], os.[ContractDetails] , p.[ProductName]
    from [Order] o inner join [OrderSummary] os on o.[OrderSummaryID]=os.[ID]
    inner join [DummyProduct] p on o.[ProductID]=p.[ProductID]
    where o.[ProductType]='borrowed' and o.[BuyerID]=@BuyerID order by o.[OrderDate] desc;

end

go

create procedure get_new_or_used_order_history @BuyerID int
as
begin
    select o.[OrderID], o.[ProductID], o.[ProductType], o.[PaymentMethod], 
    o.[OrderDate], os.[Subtotal], os.[DeliveryFee], 
    os.[finalTotal], os.[address], os.[AdditionalInfo], p.[ProductName]
    from [Order] o inner join [OrderSummary] os on o.[OrderSummaryID]=os.[ID]
    inner join [DummyProduct] p on o.[ProductID]=p.[ProductID]
    where o.[ProductType]='new' or o.[ProductType]='used' and o.[BuyerID]=@BuyerID order by o.[OrderDate] desc;

end
go


create procedure get_orders_from_last_3_months @BuyerID int
as
begin
    select o.[OrderID], o.[ProductID], o.[ProductType], o.[PaymentMethod], 
    o.[OrderDate], os.[Subtotal], os.[WarrantyTax], os.[DeliveryFee], 
    os.[finalTotal], os.[address], os.[AdditionalInfo], os.[ContractDetails] , p.[ProductName]
    from [Order] o inner join [OrderSummary] os on o.[OrderSummaryID]=os.[ID]
    inner join [DummyProduct] p on o.[ProductID]=p.[ProductID]
    where o.[BuyerID]=@BuyerID and o.[OrderDate]>=dateadd(month, -3, getdate()) order by o.[OrderDate] desc;

end
go

create procedure get_orders_from_last_6_months @BuyerID int
as
begin
    select o.[OrderID], o.[ProductID], o.[ProductType], o.[PaymentMethod], 
    o.[OrderDate], os.[Subtotal], os.[WarrantyTax], os.[DeliveryFee], 
    os.[finalTotal], os.[address], os.[AdditionalInfo], os.[ContractDetails] , p.[ProductName]
    from [Order] o inner join [OrderSummary] os on o.[OrderSummaryID]=os.[ID]
    inner join [DummyProduct] p on o.[ProductID]=p.[ProductID]
    where o.[BuyerID]=@BuyerID and o.[OrderDate]>=dateadd(month, -6, getdate()) order by o.[OrderDate] desc;

end
go

create procedure get_orders_from_2025 @BuyerID int
as
begin
    select o.[OrderID], o.[ProductID], o.[ProductType], o.[PaymentMethod], 
    o.[OrderDate], os.[Subtotal], os.[WarrantyTax], os.[DeliveryFee], 
    os.[finalTotal], os.[address], os.[AdditionalInfo], os.[ContractDetails] , p.[ProductName]
    from [Order] o inner join [OrderSummary] os on o.[OrderSummaryID]=os.[ID]
    inner join [DummyProduct] p on o.[ProductID]=p.[ProductID]
    where o.[BuyerID]=@BuyerID and year(o.[OrderDate])=2025 order by o.[OrderDate] desc;

end

go

create procedure get_orders_from_2024 @BuyerID int
as
begin
    select o.[OrderID], o.[ProductID], o.[ProductType],  o.[PaymentMethod], 
    o.[OrderDate], os.[Subtotal], os.[WarrantyTax], os.[DeliveryFee], 
    os.[finalTotal], os.[address], os.[AdditionalInfo], os.[ContractDetails], p.[ProductName]
    from [Order] o inner join [OrderSummary] os on o.[OrderSummaryID]=os.[ID]
    inner join [DummyProduct] p on o.[ProductID]=p.[ProductID]
    where o.[BuyerID]=@BuyerID and year(o.[OrderDate])=2024 order by o.[OrderDate] desc;

end
go

create procedure get_orders_by_name @BuyerID int, @text nvarchar(250)
as
begin
    select o.[OrderID], o.[ProductID], o.[ProductType], o.[PaymentMethod], 
    o.[OrderDate], os.[Subtotal], os.[WarrantyTax], os.[DeliveryFee], 
    os.[finalTotal], os.[address], os.[AdditionalInfo], os.[ContractDetails] , p.[ProductName]
    from [Order] o inner join [OrderSummary] os on o.[OrderSummaryID]=os.[ID]
    inner join [DummyProduct] p on o.[ProductID]=p.[ProductID]
    where o.[BuyerID]=@BuyerID and p.[ProductName] like '%@'+@text+'%' order by o.[OrderDate] desc;

end