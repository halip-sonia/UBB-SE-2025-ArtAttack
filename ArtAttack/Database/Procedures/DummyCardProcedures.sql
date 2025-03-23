create or alter proc [add_card] 
@cname varchar(50),
@cnumber varchar(20),
@cvc varchar(3),
@mon varchar(2), 
@yr varchar(2),
@country varchar(30),
@balance float as
begin 

insert into [DummyCard] 
values (@cname,@cnumber,@cvc,@mon,@yr,@country)

end
go


create or alter proc [delete_card] @cardnumber varchar(20) as
begin

 delete from [DummyCard] where [cardNumber]=@cardnumber

end
go

create or alter proc [update_card_balance]
@cnumber varchar(20),
@balance float
as
begin
update [DummyCard] 
set [balance]=@balance
where [cardNumber]=@cnumber
end
go


