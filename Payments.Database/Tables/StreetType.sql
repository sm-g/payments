CREATE TABLE [dbo].[StreetType]
(
	[StreetTypeID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Suffix] NCHAR(10) NOT NULL, 
    [Fullname] NVARCHAR(30) NOT NULL
)
