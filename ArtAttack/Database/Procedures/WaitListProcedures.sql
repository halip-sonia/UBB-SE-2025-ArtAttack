
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

CREATE OR ALTER PROCEDURE GetUserWaitlistPosition
    @UserID INT,
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Get the user's position in the waitlist for the specified product
    SELECT uw.positionInQueue
    FROM UserWaitList uw
    JOIN WaitListProduct wp ON uw.productWaitListID = wp.waitListProductID
    WHERE uw.userID = @UserID AND wp.productID = @ProductID;
    
    -- If no rows returned, the user isn't on this waitlist
    -- The C# code will handle this as position -1
END
GO

--procedure to delete a user from a given waitList
ALTER PROCEDURE RemoveUserFromWaitlist
    @UserID INT,
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @WaitListProductID INT;
    DECLARE @UserPosition INT;

    -- Get the WaitListProductID
    SELECT @WaitListProductID = waitListProductID 
    FROM WaitListProduct 
    WHERE productID = @ProductID;

    -- Ensure that we found a valid waitlist entry
    IF @WaitListProductID IS NULL
    BEGIN
        PRINT 'No matching product found in the waitlist.';
        RETURN;
    END

    -- Get the User's Position BEFORE deleting them
    SELECT @UserPosition = positionInQueue
    FROM UserWaitList
    WHERE userID = @UserID AND productWaitListID = @WaitListProductID;

    -- Ensure user exists in the waitlist before proceeding
    IF @UserPosition IS NULL
    BEGIN
        PRINT 'User not found in the waitlist.';
        RETURN;
    END

    -- Delete the user from the waitlist
    DELETE FROM UserWaitList
    WHERE userID = @UserID AND productWaitListID = @WaitListProductID;

    -- Update positions of remaining users
    UPDATE UserWaitList
    SET positionInQueue = positionInQueue - 1
    WHERE productWaitListID = @WaitListProductID AND positionInQueue > @UserPosition;

    PRINT 'User removed and positions updated successfully.';
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


