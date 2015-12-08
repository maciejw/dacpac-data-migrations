CREATE TABLE [dbo].[Customers] (
    [Id]             INT            NOT NULL,
    [CustomerTypeId] INT            NOT NULL,
    [Name]           NVARCHAR (255) NULL, 
    [FirstName] NVARCHAR(255) NULL, 
    [LastName] NVARCHAR(255) NULL, 
    CONSTRAINT [FK_Customers_ToTable] FOREIGN KEY ([CustomerTypeId]) REFERENCES [CustomerTypes]([Id]),
);


