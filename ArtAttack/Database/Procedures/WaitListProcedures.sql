
--procedure to add a new user to the waitlist of a product
--procedure to delete a user from a waitlist of a product
-- Select all users in the waitlist for the given product
-- Select all waitlists the user has joined

/*alter procedure AddUserToWaitlist
    @UserID int,
    @ProductID int
as
begin
    set nocount on;

     DECLARE @WaitListProductID INT;
        
     SELECT @WaitListProductID = waitListProductID 
     FROM WaitListProduct 
     WHERE productID = @ProductID;

    declare @NextPosition int;
    select @NextPosition = isnull(max(positionInQueue), 0) + 1
    from UserWaitList
    where productWaitListID = @WaitListProductID;

    insert into UserWaitList (productWaitListID, userID, joinedTime, positionInQueue)
    values (@WaitListProductID, @UserID, getdate(), @NextPosition);
end;
go*/

--procedure to delete a user from a given waitList
ALTER PROCEDURE RemoveUserFromWaitlist
    @UserID INT,
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @WaitListProductID INT, @UserPosition INT;
    
    -- Find the waitlist product ID for the given product
    SELECT @WaitListProductID = WaitListProductID 
    FROM WaitListProduct 
    WHERE ProductID = @ProductID;
    
    -- If no corresponding waitlist product, exit procedure
    IF @WaitListProductID IS NULL
    BEGIN
        PRINT 'No waitlist entry found for the given product.';
        RETURN;
    END;

    -- Get the user's position in the queue before deleting
    SELECT @UserPosition = positionInQueue
    FROM UserWaitList
    WHERE UserID = @UserID AND ProductWaitListID = @WaitListProductID;

    -- If the user is not in the waitlist, exit procedure
    IF @UserPosition IS NULL
    BEGIN
        PRINT 'User is not on the waitlist for this product.';
        RETURN;
    END;

    BEGIN TRANSACTION;
    
    -- Delete the user from the waitlist
    DELETE FROM UserWaitList
    WHERE UserID = @UserID AND ProductWaitListID = @WaitListProductID;

    -- Update the positions of users who were behind in the queue
    UPDATE UserWaitList
    SET positionInQueue = positionInQueue - 1
    WHERE ProductWaitListID = @WaitListProductID 
          AND positionInQueue > @UserPosition;

    COMMIT TRANSACTION;
END;

        
/*
-- Select all users in the waitlist for the given product
go
CREATE PROCEDURE GetUsersInWaitlist
    @WaitListProductID BIGINT
AS
BEGIN
    SELECT
        UserWaitList.[userID],
        UserWaitList.[positionInQueue],
        UserWaitList.[joinedTime]
    FROM [UserWaitList] UserWaitList
    WHERE UserWaitList.[productWaitListID] = @WaitListProductID
    ORDER BY UserWaitList.[positionInQueue] ASC;
END;


 -- Select all waitlists the user has joined
 go

CREATE PROCEDURE GetUserWaitlists
    @UserID INT
AS
BEGIN
    SELECT
        UserWaitList.productWaitListID,
        WaitListProduct.productID,
        UserWaitList.positionInQueue,
        UserWaitList.joinedTime,
        WaitListProduct.availableAgain
    FROM UserWaitList
    INNER JOIN WaitListProduct ON UserWaitList.productWaitListID = WaitListProduct.waitListProductID
    WHERE UserWaitList.userID = @UserID
    ORDER BY UserWaitList.joinedTime ASC;
END;

go
--procedure to check if a user is or not in a waitlist
CREATE PROCEDURE CheckUserInWaitlist
    @UserID INT,
    @ProductWaitListID INT,
    @IsInWaitlist BIT OUTPUT
AS
BEGIN
    SET @IsInWaitlist = 0;

    IF EXISTS (SELECT 1 FROM UserWaitList WHERE userID = @UserID AND productWaitListID = @ProductWaitListID)
    BEGIN
        SET @IsInWaitlist = 1;
    END
END;
go
--count the users in a waitList
CREATE PROCEDURE GetWaitlistSize
    @ProductWaitListID INT,
    @TotalUsers INT OUTPUT
AS
BEGIN
    SELECT @TotalUsers = COUNT(*)
    FROM UserWaitList
    WHERE productWaitListID = @ProductWaitListID;
END;
*/


