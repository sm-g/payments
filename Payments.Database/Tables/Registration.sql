CREATE TABLE [dbo].[Registration]
(
	[RegistrationID] INT NOT NULL PRIMARY KEY IDENTITY,
    [FlatID] INT NOT NULL,
    [PayerID] INT NOT NULL,
	[PaymentStartDate] DATE NOT NULL,
    [PaymentFinishDate] DATE NULL, 
    CONSTRAINT [AK_Registration] UNIQUE ([FlatID], [PayerID]), 
    CONSTRAINT [FK_Registration_Payer] FOREIGN KEY ([PayerID]) REFERENCES [Payer]([PayerID]),
    CONSTRAINT [FK_Registration_Flat] FOREIGN KEY ([FlatID]) REFERENCES [Flat]([FlatID]), 
    CONSTRAINT [CK_Registration_Dates] CHECK ([PaymentFinishDate] is NULL OR [PaymentStartDate]<[PaymentFinishDate])
)
