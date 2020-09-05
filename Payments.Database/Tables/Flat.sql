CREATE TABLE [dbo].[Flat]
(
	[FlatID] INT NOT NULL PRIMARY KEY IDENTITY,
	[Number] smallint NULL,
	[LivingMen] tinyint NULL,
	[CommonArea] Decimal(7,3) NULL,
	[LivingArea] Decimal(7,3) NULL,
	[FlatTypeID] Int NOT NULL,
	[HouseID] Int NOT NULL,
	CONSTRAINT [AK_Flat_Number_House] UNIQUE ([Number], [HouseID]), 
    CONSTRAINT [CK_Flat_Area] CHECK ([CommonArea] >= [LivingArea]), 
    CONSTRAINT [CK_Flat_LivingArea] CHECK ([LivingArea] > 0),
	CONSTRAINT [CK_Flat_LivingMen] CHECK ([LivingMen] >= 0), 
    CONSTRAINT [FK_Flat_FlatType] FOREIGN KEY ([FlatTypeID]) REFERENCES [FlatType]([FlatTypeID]), 
    CONSTRAINT [FK_Flat_House] FOREIGN KEY ([HouseID]) REFERENCES [House]([HouseID])
)
