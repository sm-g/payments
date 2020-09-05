CREATE TABLE [dbo].[Rate]
(
	[RateID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(200) NOT NULL, 
    [Price] MONEY NOT NULL, 
    [Norm] DECIMAL(9, 5) NULL, 
	[PriceAboveNorm] MONEY NULL,
    [BeginDate] DATE NOT NULL, 
	[EndDate] DATE NULL,
    [BenefitID] INT NOT NULL, 
    [FlatTypeID] INT NOT NULL, 
    [ServiceID] INT NOT NULL, 
    CONSTRAINT [FK_Rate_Benefit] FOREIGN KEY ([BenefitID]) REFERENCES [Benefit]([BenefitID]), 
    CONSTRAINT [FK_Rate_FlatType] FOREIGN KEY ([FlatTypeID]) REFERENCES [FlatType]([FlatTypeID]),
	CONSTRAINT [FK_Rate_Service] FOREIGN KEY ([ServiceID]) REFERENCES [Service]([ServiceID]), 
    CONSTRAINT [CK_Rate_Prices] CHECK (Price > 0 AND [PriceAboveNorm] > 0),
	CONSTRAINT [CK_Rate_Dates] CHECK (EndDate IS NULL OR BeginDate < EndDate)

)
