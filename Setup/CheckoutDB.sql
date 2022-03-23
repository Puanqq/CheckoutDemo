create database CheckoutDemo

go
use CheckoutDemo

go
create table Pruducts
(
	Id uniqueidentifier primary key,
	PName varchar(50),
	Price float,
	Created_at datetime
)

go
create table Orders
(
	Id uniqueidentifier primary key,
	Total float,
	Created_at datetime
)

go
create table Order_details
(
	Id int identity(1,1) primary key,
	OrderId uniqueidentifier,
	ProductId uniqueidentifier,
	Quantity int,
	Created_at datetime
)

alter table Order_details
add constraint FK_OrderDetail_Order foreign key (OrderId) references Orders(Id)