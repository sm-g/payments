CREATE TABLE [dbo].[User]
(
    [UserID] Int NOT NULL PRIMARY KEY IDENTITY,
    [Login] nVarchar(50) NOT NULL UNIQUE NONCLUSTERED,
    [Pass] char(44) NOT NULL,
    [Salt] char(44) NOT NULL,
    [UserGroupID] TINYINT NOT NULL, 
    CONSTRAINT [FK_User_UserGroup] FOREIGN KEY ([UserGroupID]) REFERENCES [UserGroup]([UserGroupID])
)
GO