CREATE PROCEDURE UpdateDummyProduct
    @ID INT,
    @Name VARCHAR(64),
    @Price FLOAT,
    @SellerID INT,
    @ProductType VARCHAR(20),
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE DummyProduct
        SET Name = @Name,
            Price = @Price,
            SellerID = @SellerID,
            ProductType = @ProductType,
            StartDate = @StartDate,
            EndDate = @EndDate
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

CREATE PROCEDURE DeleteDummyProduct
    @ID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM DummyProduct
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

CREATE PROCEDURE AddDummyProduct
    @Name VARCHAR(64),
    @Price FLOAT,
    @SellerID INT,
    @ProductType VARCHAR(20),
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO DummyProduct (Name, Price, SellerID, ProductType, StartDate, EndDate)
        VALUES (@Name, @Price, @SellerID, @ProductType, @StartDate, @EndDate);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
