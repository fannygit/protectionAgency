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
PRINT N'已略過索引鍵為 214f6851-7df0-480f-8ed6-5803addbfebe 的重新命名重構作業，項目 [dbo].[T_Permission].[Permission] (SqlSimpleColumn) 將不會重新命名為 Name';


GO
PRINT N'已略過索引鍵為 da6faaa5-022f-470a-a223-ec549480b47d 的重新命名重構作業，項目 [dbo].[T_Role].[RoleName] (SqlSimpleColumn) 將不會重新命名為 Name';


GO
PRINT N'正在建立 [dbo].[T_Account]...';


GO
CREATE TABLE [dbo].[T_Account] (
    [Id]            NVARCHAR (32)  NOT NULL,
    [Account]       NVARCHAR (256) NOT NULL,
    [Name]          NVARCHAR (256) NULL,
    [Password]      NVARCHAR (256) NOT NULL,
    [Status]        INT            NOT NULL,
    [CreateDate]    DATETIME       NOT NULL,
    [LastLoginTime] DATETIME       NULL,
    [UpdateDate ]   DATETIME       NOT NULL,
    [CreateUserID]  NVARCHAR (32)  NOT NULL,
    [UpdateUserID]  NVARCHAR (32)  NOT NULL,
    [IP]            NVARCHAR (256) NULL,
    CONSTRAINT [PK_T_Account] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Account_Role_Mapping]...';


GO
CREATE TABLE [dbo].[T_Account_Role_Mapping] (
    [Id]           NVARCHAR (32) NOT NULL,
    [AccountId]    NVARCHAR (32) NOT NULL,
    [RoleId]       NVARCHAR (32) NOT NULL,
    [CreateUserId] NVARCHAR (32) NOT NULL,
    [CreateDate]   DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Permission]...';


GO
CREATE TABLE [dbo].[T_Permission] (
    [Id]           NVARCHAR (32)  NOT NULL,
    [Name]         NVARCHAR (256) NOT NULL,
    [CreateDate]   DATETIME       NOT NULL,
    [UpdateDate]   DATETIME       NOT NULL,
    [CreateUserId] NVARCHAR (32)  NOT NULL,
    [UpdateUserId] NVARCHAR (32)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Role]...';


GO
CREATE TABLE [dbo].[T_Role] (
    [Id]           NVARCHAR (32)  NOT NULL,
    [Name]         NVARCHAR (256) NOT NULL,
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
PRINT N'正在建立 [dbo].[DF_T_Account_Status]...';


GO
ALTER TABLE [dbo].[T_Account]
    ADD CONSTRAINT [DF_T_Account_Status] DEFAULT (0) FOR [Status];


GO
PRINT N'正在建立 [dbo].[DF_T_Account_CreateDate]...';


GO
ALTER TABLE [dbo].[T_Account]
    ADD CONSTRAINT [DF_T_Account_CreateDate] DEFAULT (getdate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Account] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Account]
    ADD DEFAULT (getdate()) FOR [LastLoginTime];


GO
PRINT N'正在建立 [dbo].[T_Account] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Account]
    ADD DEFAULT (getdate()) FOR [UpdateDate ];


GO
PRINT N'正在建立 [dbo].[T_Account_Role_Mapping] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Account_Role_Mapping]
    ADD DEFAULT (getdate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Permission] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Permission]
    ADD DEFAULT getdate() FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Permission] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Permission]
    ADD DEFAULT getdate() FOR [UpdateDate];


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
PRINT N'正在建立 [dbo].[T_Role].[Name].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'群組名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'T_Role', @level2type = N'COLUMN', @level2name = N'Name';


GO
PRINT N'正在建立 [dbo].[T_Role].[CreateDate].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'T_Role', @level2type = N'COLUMN', @level2name = N'CreateDate';


GO
-- 用於更新含有部署交易記錄之目標伺服器的重構步驟
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '214f6851-7df0-480f-8ed6-5803addbfebe')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('214f6851-7df0-480f-8ed6-5803addbfebe')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'da6faaa5-022f-470a-a223-ec549480b47d')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('da6faaa5-022f-470a-a223-ec549480b47d')

GO

GO
PRINT N'更新完成。';


GO
