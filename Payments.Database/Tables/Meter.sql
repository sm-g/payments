CREATE TABLE [dbo].[Meter]
(
    [MeterID] INT NOT NULL PRIMARY KEY IDENTITY,
    [RegistrationID] INT NOT NULL,
    [MeterTypeID] INT NOT NULL, 
    CONSTRAINT [FK_Meter_MeterType] FOREIGN KEY ([MeterTypeID]) REFERENCES [MeterType]([MeterTypeID]), 
    CONSTRAINT [FK_Meter_Registration] FOREIGN KEY ([RegistrationID]) REFERENCES [Registration]([RegistrationID])
)
