CREATE TABLE [dbo].[T_Role_Permission_Mapping]
(
	[Id] NVARCHAR(32) NOT NULL , 
    [RoleId] NVARCHAR(32) NOT NULL, 
    [PermissionId] NVARCHAR(32) NOT NULL, 
    [CreateUserId] NVARCHAR(32) NOT NULL, 
    [CreateDate] DATETIME NOT NULL DEFAULT (getdate()), 
    CONSTRAINT [PK_T_Account_Group_Mapping] PRIMARY KEY ([Id]) 
)
