CREATE TABLE [dbo].[House]
(
    [HouseID] int NOT NULL PRIMARY KEY IDENTITY,
    [Number] varchar(10) NOT NULL,
    [Building] nvarchar(10) NULL,
    [StreetID] int NOT NULL, 
    CONSTRAINT [FK_House_ToStreet] FOREIGN KEY ([StreetID]) REFERENCES [Street]([StreetID])
)
