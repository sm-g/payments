CREATE TABLE [dbo].[Reading]
(
    [ReadingID] INT NOT NULL PRIMARY KEY IDENTITY,
    [Value] Decimal(8,3) NOT NULL,
    [MeterID] INT NOT NULL,
    [Date] DATE NOT NULL, 
    CONSTRAINT [FK_Reading_Meter] FOREIGN KEY ([MeterID]) REFERENCES [Meter]([MeterID]), 
    CONSTRAINT [CK_Reading_Value] CHECK (Value > 0) 
)