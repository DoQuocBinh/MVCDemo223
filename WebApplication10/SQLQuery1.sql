create table Categories(
	category_Id int primary key identity(1,1),
	CategoryName nvarchar(50),
)

create table Products(
	product_id int primary key identity(1,1),
	productName nvarchar(50),
	price decimal,
	product_img varbinary(max),
	category int references Categories(category_Id)
)

insert into Categories values('Smart Phone')
