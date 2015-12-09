insert into dbo.Categories(Id) select Id from dbo.CustomerTypes
insert into dbo.CustomerCategories(CustomerId, CategoryId) select Id, CustomerTypeId from dbo.Customers