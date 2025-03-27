insert into [DummyWallet] 
values (120.0);

insert into [DummyCard]
values 
('Alice Johnson', '5425233430109903', '123', '08', '26', 'Romania', 1500.50),
('Bob Smith', '2222420000001113	', '456', '12', '27', 'Canada', 12.00),
('Charlie Brown', '2223000048410010', '789', '01', '25', 'Spain', 500.00)

insert into [DummyBuyer]
values
('Buyer1'),('Buyer2'),('Buyer3')

insert into [DummySeller]
values
('Seller1'),('Seller2'),('Seller3')


insert into [DummyProduct]
values
('Toothbrush', 10.00, 1, 'new', NULL,NULL),
('Hair Dryer', 150.00, 1, 'borrowed', NULL,NULL),
('Ancient Chinese Vase', 100, 2, 'bid',NULL,NULL),
('Wallet Top Up', 200,NULL,'refill',NULL,NULL)

insert into [DummyProduct]
values
('IPhone 4 Penguin Phone Case', 75.00, 3, 'used', NULL,NULL)