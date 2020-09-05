CREATE TABLE [dbo].[Payer]
(
    [PayerID] Int NOT NULL PRIMARY KEY IDENTITY,
    [FirstName] NVARCHAR(50) NOT NULL,
    [MiddleName] NVARCHAR(50) NULL,
    [LastName] NVARCHAR(50) NOT NULL,
    [Sex] TINYINT NOT NULL,
    [BirthDate] Datetime NULL,
    [BenefitID] Int NOT NULL,
    [UserID] Int NULL, 
    CONSTRAINT [FK_Payer_Benefit] FOREIGN KEY ([BenefitID]) REFERENCES [Benefit]([BenefitID]),
    CONSTRAINT [FK_Payer_User] FOREIGN KEY ([UserID]) REFERENCES [User]([UserID]), 
    CONSTRAINT [CK_Payer_Sex] CHECK ([Sex] IN (0,1,2,9)) -- see ISO/IEC 5218
)

