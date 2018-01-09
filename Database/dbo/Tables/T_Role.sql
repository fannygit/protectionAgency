CREATE TABLE [dbo].[T_Role]
(
	[Id] NVARCHAR(32) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(256) NOT NULL, 
    [CreateDate] DATETIME NOT NULL DEFAULT getdate(), 
    [UpdateDate] DATETIME NOT NULL DEFAULT getdate(), 
    [CreateUserId] NVARCHAR(32) NOT NULL, 
    [UpdateUserId] NVARCHAR(32) NOT NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'群組名稱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Role',
    @level2type = N'COLUMN',
    @level2name = 'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Role',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'建立時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Role',
    @level2type = N'COLUMN',
    @level2name = N'CreateDate'