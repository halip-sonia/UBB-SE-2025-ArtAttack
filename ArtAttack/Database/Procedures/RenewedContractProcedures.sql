 --INSERT RENEWED CONTRACT
 --UPDATE RENEWED CONTRACT
 --SELECT RENEWED CONTRACT


GO

/*
  This procedure inserts a renewed contract into the [Contract] table.
  Note: This procedure is used exclusively for contract renewals.
        The [contractStatus] is set to 'RENEWED' by default.
        The [renewedFromContractID] must reference the original contract being renewed.
*/
CREATE PROCEDURE [AddRenewedContract]
    @orderID INT,
    @startDate DATETIME,
    @endDate DATETIME,
    @contractContent TEXT,
    @renewalCount INT,
    @predefinedContractID INT,
    @pdfID INT,
    @renewedFromContractID BIGINT
AS
BEGIN
    INSERT INTO [Contract] (
        [orderID],
        [startDate],
        [endDate],
        [contractStatus],
        [contractContent],
        [renewalCount],
        [predefinedContractID],
        [pdfID],
        [renewedFromContractID]
    )
    VALUES (
        @orderID,
        @startDate,
        @endDate,
        'RENEWED',
        @contractContent,
        @renewalCount,
        @predefinedContractID,
        @pdfID,
        @renewedFromContractID
    );
END;
GO



/*
  This procedure updates the details of an existing renewed contract in the [Contract] table.
  Note: This procedure is used exclusively for contract renewals.
        The contract identified by [@contractID] must already have the status 'RENEWED'.
        Updates are applied to the borrowing period, contract content, renewal count, 
        predefined contract reference, and associated PDF file.
*/
CREATE PROCEDURE [UpdateRenewedContract]
    @contractID BIGINT,
    @startDate DATETIME,
    @endDate DATETIME,
    @contractContent TEXT,
    @renewalCount INT,
    @predefinedContractID INT,
    @pdfID INT
AS
BEGIN
    -- Updates a renewed contract's details based on its ID.
    -- This should be used only for contracts with status 'RENEWED'.
    UPDATE [Contract]
    SET 
        [startDate] = @startDate,
        [endDate] = @endDate,
        [contractContent] = @contractContent,
        [renewalCount] = @renewalCount,
        [predefinedContractID] = @predefinedContractID,
        [pdfID] = @pdfID
    WHERE 
        [ID] = @contractID AND [contractStatus] = 'RENEWED';
END;
GO



/*
  This procedure retrieves all renewed contracts from the [Contract] table.
  Only contracts with [contractStatus] = 'RENEWED' are returned.
*/
CREATE PROCEDURE [GetRenewedContracts]
AS
BEGIN
    SELECT 
        [ID],
        [orderID],
        [startDate],
        [endDate],
        [contractStatus],
        [contractContent],
        [renewalCount],
        [predefinedContractID],
        [pdfID],
        [renewedFromContractID]
    FROM [Contract]
    WHERE [contractStatus] = 'RENEWED';
END;
GO