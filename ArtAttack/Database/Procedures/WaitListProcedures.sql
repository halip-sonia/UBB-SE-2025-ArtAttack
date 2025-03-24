create procedure AddUserToWaitlist
    @UserID int,
    @ProductWaitListID bigint
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

create procedure RemoveUserFromWaitlist
    @UserID int,
    @ProductWaitListID bigint
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
