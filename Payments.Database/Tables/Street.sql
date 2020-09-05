CREATE TABLE [dbo].[Street]
(
	[StreetID] Int NOT NULL PRIMARY KEY IDENTITY,
	[Name] nVarchar(50) NOT NULL,
	[StreetTypeID] INT NULL,
	[SettlementID] Int NULL, 
    CONSTRAINT [FK_Street_Settlement] FOREIGN KEY ([SettlementID]) REFERENCES [Settlement]([SettlementID]),
    CONSTRAINT [FK_Street_StreetType] FOREIGN KEY ([StreetTypeID]) REFERENCES [StreetType]([StreetTypeID])
)
