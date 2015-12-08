update dbo.Customers set 
	FirstName = LEFT(Name, CHARINDEX(' ', Name) - 1),
	LastName = RIGHT(Name, LEN(Name) - CHARINDEX(N' ', Name))