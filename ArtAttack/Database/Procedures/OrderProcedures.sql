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
go


CREATE PROCEDURE UpdateOrder
    @OrderID INT,
    @ProductType varchar(20),
    @PaymentMethod VARCHAR(20),
    @OrderDate timestamp
   
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Orders
        SET [ProductType] = @ProductType,
            [PaymentMethod] = @PaymentMethod,
            [OrderDate]=@OrderDate
           
        WHERE OrderID = @OrderID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

CREATE PROCEDURE DeleteOrder
    @OrderID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM Orders
        WHERE OrderID = @OrderID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

CREATE PROCEDURE AddOrder
   @ProductID INT,
    @BuyerID INT,
    @ProductType varchar(20),
    @PaymentMethod VARCHAR(20),
    @OrderSummaryID INT,
    @OrderDate timestamp
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Orders ([ProductID],[BuyerID], [ProductType], [PaymentMethod], [OrderSummaryID], [OrderDate])
        VALUES (@ProductID, @BuyerID, @ProductType, @PaymentMethod, @OrderSummaryID, @OrderDate);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO



CREATE PROCEDURE UpdateOrderSummary
    @ID INT,
    @Subtotal float,
    @WarrantyTax float,
    @DeliveryFee float,
    @FinalTotal float,
    @FullName varchar(255),
    @Email varchar(255),
    @PhoneNumber varchar(10),
    @Address varchar(255),
    @PostalCode varchar(255),
    @AdditionalInfo varchar(255),
	@ContractDetails text null
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE OrderSummary
        SET [ProductType] = @ProductType,
             [Subtotal]=@Subtotal,
             [WarrantyTax]=@WarrantyTax,
             [DeliveryFee]=@DeliveryFee,
             [FinalTotal]=@FinalTotal,
             [FullName]=@FullName,
             [Email]=@Email,
             [PhoneNumber]=@PhoneNumber,
             [Address]=@Address,
             [PostalCode]=@PostalCode,
             [AdditionalInfo]=@AdditionalInfo,
	         [ContractDetails]=@ContractDetails
           
        WHERE ID = @ID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

CREATE PROCEDURE DeleteOrderSummary
    @ID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM OrderSummary
        WHERE ID = @ID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

CREATE PROCEDURE AddOrderSummary
   @Subtotal float,
    @WarrantyTax float,
    @DeliveryFee float,
    @FinalTotal float,
    @FullName varchar(255),
    @Email varchar(255),
    @PhoneNumber varchar(10),
    @Address varchar(255),
    @PostalCode varchar(255),
    @AdditionalInfo varchar(255),
	@ContractDetails text null
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO OrderSummary ([Subtotal],[WarrantyTax], [DeliveryFee], [FinalTotal], [FullName], [Email], [PhoneNumber], [Address], [PostalCode], [AdditionalInfo]
, [ContractDetails])    VALUES (@Subtotal,@WarrantyTax, @DeliveryFee, @FinalTotal, @FullName, @Email, @PhoneNumber, @Address, @PostalCode, @AdditionalInfo
, @ContractDetails);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
