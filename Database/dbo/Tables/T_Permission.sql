CREATE TABLE [dbo].[T_Permission]
(
	[Id] NVARCHAR(32) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(256) NOT NULL, 
    [CreateDate] DATETIME NOT NULL DEFAULT getdate(), 
    [UpdateDate] DATETIME NOT NULL DEFAULT getdate(), 
    [CreateUserId] NVARCHAR(32) NOT NULL, 
    [UpdateUserId] NVARCHAR(32) NOT NULL
)
