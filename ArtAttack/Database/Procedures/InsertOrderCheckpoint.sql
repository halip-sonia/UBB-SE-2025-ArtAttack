go
create procedure uspInsertOrderCheckpoint (@timestamp datetime, @location varchar(255), @description varchar(255), @checkpointStatus varchar(100), @trackedOrderID bigint) as
begin
	begin try
		insert into OrderCheckpoints values(@timestamp, @location, @description, @checkpointStatus, @trackedOrderID)
	end try
	begin catch
		declare @err_message varchar(2000)
		declare @err_number int
		select @err_message = error_message(), @err_number = error_number()
		raiserror(N'uspInsertOrderCheckpoint: Error %i: %s', 16, 1, @err_number, @err_message)
	end catch
end
go
