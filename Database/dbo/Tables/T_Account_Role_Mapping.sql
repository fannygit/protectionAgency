CREATE TABLE [dbo].[T_Account_Role_Mapping]
(
	[Id] NVARCHAR(32) NOT NULL PRIMARY KEY, 
    [AccountId] NVARCHAR(32) NOT NULL, 
    [RoleId] NVARCHAR(32) NOT NULL, 
    [CreateUserId] NVARCHAR(32) NOT NULL, 
    [CreateDate] DATETIME NOT NULL DEFAULT (getdate())
)
