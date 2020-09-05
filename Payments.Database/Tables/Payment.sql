CREATE TABLE [dbo].[Payment]
(
	[PaymentID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Date] DATE NOT NULL, 
    [TargetMonth] DATE NOT NULL, 
    [Amount] MONEY NOT NULL, 
    [RegistrationID] INT NOT NULL, 
    [ServiceID] INT NOT NULL, 
    CONSTRAINT [FK_Payment_Registration] FOREIGN KEY ([RegistrationID]) REFERENCES [Registration]([RegistrationID]), 
    CONSTRAINT [FK_Payment_Service] FOREIGN KEY ([ServiceID]) REFERENCES [Service]([ServiceID]), 
    CONSTRAINT [CK_Payment_Amount] CHECK (Amount > 0)
)
