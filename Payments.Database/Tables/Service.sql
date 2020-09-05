CREATE TABLE [dbo].[Service]
(
    [ServiceID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [ServiceTypeID] INT NOT NULL, 
    [UnitID] INT NOT NULL, 
    CONSTRAINT [FK_Service_Unit] FOREIGN KEY ([UnitID]) REFERENCES [Unit]([UnitID]), 
    CONSTRAINT [FK_Service_ServiceType] FOREIGN KEY ([ServiceTypeID]) REFERENCES [ServiceType]([ServiceTypeID])
)
