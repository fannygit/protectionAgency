/*
test 的部署指令碼

這段程式碼由工具產生。
變更這個檔案可能導致不正確的行為，而且如果重新產生程式碼，
變更將會遺失。
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "test"
:setvar DefaultFilePrefix "test"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
偵測 SQLCMD 模式，如果不支援 SQLCMD 模式，則停用指令碼執行。
若要在啟用 SQLCMD 模式後重新啟用指令碼，請執行以下:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'必須啟用 SQLCMD 模式才能成功執行此指令碼。';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'正在建立 [dbo].[T_Role]...';


GO
CREATE TABLE [dbo].[T_Role] (
    [Id]           NVARCHAR (32)  NOT NULL,
    [RoleName]     NVARCHAR (256) NOT NULL,
    [CreateDate]   DATETIME       NOT NULL,
    [UpdateDate]   DATETIME       NOT NULL,
    [CreateUserId] NVARCHAR (32)  NOT NULL,
    [UpdateUserId] NVARCHAR (32)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Role_Permission_Mapping]...';


GO
CREATE TABLE [dbo].[T_Role_Permission_Mapping] (
    [Id]           NVARCHAR (32) NOT NULL,
    [RoleId]       NVARCHAR (32) NOT NULL,
    [PermissionId] NVARCHAR (32) NOT NULL,
    [CreateUserId] NVARCHAR (32) NOT NULL,
    [CreateDate]   DATETIME      NOT NULL,
    CONSTRAINT [PK_T_Account_Group_Mapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Role] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Role]
    ADD DEFAULT getdate() FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Role] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Role]
    ADD DEFAULT getdate() FOR [UpdateDate];


GO
PRINT N'正在建立 [dbo].[T_Role_Permission_Mapping] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Role_Permission_Mapping]
    ADD DEFAULT (getdate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Role].[Id].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'T_Role', @level2type = N'COLUMN', @level2name = N'Id';


GO
PRINT N'正在建立 [dbo].[T_Role].[RoleName].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'群組名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'T_Role', @level2type = N'COLUMN', @level2name = N'RoleName';


GO
PRINT N'正在建立 [dbo].[T_Role].[CreateDate].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'T_Role', @level2type = N'COLUMN', @level2name = N'CreateDate';


GO
PRINT N'更新完成。';


GO
