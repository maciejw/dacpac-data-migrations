CREATE TABLE [dbo].[Customers] (
    [Id]             INT            NOT NULL,
    [CustomerTypeId] INT            NOT NULL,
    [FirstName] NVARCHAR(255) NOT NULL, 
    [LastName] NVARCHAR(255) NOT NULL, 
    CONSTRAINT [FK_Customers_ToTable] FOREIGN KEY ([CustomerTypeId]) REFERENCES [CustomerTypes]([Id]),
);


