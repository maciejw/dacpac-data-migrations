CREATE TABLE [dbo].[CustomerCategories]
(
	[CustomerId] INT NOT NULL,
	[CategoryId] INT NOT NULL, 
    CONSTRAINT [PK_CustomerCategories] PRIMARY KEY ([CustomerId], [CategoryId]),
	CONSTRAINT [FK_CustomerCategories_ToCustomers] FOREIGN KEY ([CustomerId]) REFERENCES [Customers]([Id]),
	CONSTRAINT [FK_CustomerCategories_ToCategories] FOREIGN KEY ([CategoryId]) REFERENCES [Categories]([Id]),
	
)
