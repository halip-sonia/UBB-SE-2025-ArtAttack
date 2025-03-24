
--procedure to add a new user to the waitlist of a product
--procedure to delete a user from a waitlist of a product
-- Select all users in the waitlist for the given product
-- Select all waitlists the user has joined

create procedure AddUserToWaitlist
    @UserID int,
    @ProductWaitListID int
as
begin
    set nocount on;

    declare @NextPosition int;
    select @NextPosition = isnull(max(positionInQueue), 0) + 1
    from UserWaitList
    where productWaitListID = @ProductWaitListID;

    insert into UserWaitList (productWaitListID, userID, joinedTime, positionInQueue)
    values (@ProductWaitListID, @UserID, getdate(), @NextPosition);
end;

--procedure to delete a user from a given waitList
create procedure RemoveUserFromWaitlist
    @UserID int,
    @ProductWaitListID int
as
begin
    set nocount on;

    declare @UserPosition int;

    select @UserPosition = positionInQueue
    from UserWaitList
    where userID = @UserID and productWaitListID = @ProductWaitListID;

    delete from UserWaitList
    wehere userID = @UserID and productWaitListID = @ProductWaitListID;

    update UserWaitList
    set positionInQueue = positionInQueue - 1
    where productWaitListID = @ProductWaitListID and positionInQueue > @UserPosition;
end;

-- Select all users in the waitlist for the given product

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

