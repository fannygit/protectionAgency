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
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET PAGE_VERIFY NONE 
            WITH ROLLBACK IMMEDIATE;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'已略過索引鍵為 a209de94-04ac-488c-b705-26f2f1864c3d, 643b89de-e41c-4614-8f91-c003de19be8e, 3e06c0c6-df04-46e6-99e1-34f95ba242d2, 60145ec0-e4d4-4b01-8dde-7a4f3269cce5 的重新命名重構作業，項目 [dbo].[T_Account_Permissions_Mapping].[RoleId] (SqlSimpleColumn) 將不會重新命名為 PermissionId';


GO
PRINT N'已略過索引鍵為 07fa5ea3-4197-42dd-842d-6d8e229023b1 的重新命名重構作業，項目 [dbo].[T_Account_Area_Mapping].[RoleId] (SqlSimpleColumn) 將不會重新命名為 AccountId';


GO
PRINT N'已略過索引鍵為 ff593c51-7a60-446f-a0f4-a36f6fedaef0 的重新命名重構作業，項目 [dbo].[T_Account_Area_Mapping].[PermissionId] (SqlSimpleColumn) 將不會重新命名為 AreaId';


GO
PRINT N'已略過索引鍵為 4e730aa2-eb2f-45c0-bdf3-618372bd71a1, 9c7077a2-6576-477e-8d49-098633768b7b 的重新命名重構作業，項目 [dbo].[T_Member_Friends].[InviteID] (SqlSimpleColumn) 將不會重新命名為 InviterMemberID';


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
PRINT N'正在建立 [dbo].[T_Account_Permissions_Mapping]...';


GO
CREATE TABLE [dbo].[T_Account_Permissions_Mapping] (
    [Id]           NVARCHAR (32) NOT NULL,
    [AccountId]    NVARCHAR (32) NOT NULL,
    [PermissionId] NVARCHAR (32) NOT NULL,
    [CreateUserId] NVARCHAR (32) NOT NULL,
    [CreateDate]   DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Permissions]...';


GO
CREATE TABLE [dbo].[T_Permissions] (
    [Id]             NVARCHAR (32) NOT NULL,
    [PermissionName] NVARCHAR (32) NOT NULL,
    [CreateDate]     DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
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
PRINT N'正在建立 [dbo].[T_Account_Permissions_Mapping] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Account_Permissions_Mapping]
    ADD DEFAULT (getdate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Permissions] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Permissions]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
-- 用於更新含有部署交易記錄之目標伺服器的重構步驟

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'a209de94-04ac-488c-b705-26f2f1864c3d')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('a209de94-04ac-488c-b705-26f2f1864c3d')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '643b89de-e41c-4614-8f91-c003de19be8e')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('643b89de-e41c-4614-8f91-c003de19be8e')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '3e06c0c6-df04-46e6-99e1-34f95ba242d2')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('3e06c0c6-df04-46e6-99e1-34f95ba242d2')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '07fa5ea3-4197-42dd-842d-6d8e229023b1')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('07fa5ea3-4197-42dd-842d-6d8e229023b1')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'ff593c51-7a60-446f-a0f4-a36f6fedaef0')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('ff593c51-7a60-446f-a0f4-a36f6fedaef0')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '4e730aa2-eb2f-45c0-bdf3-618372bd71a1')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('4e730aa2-eb2f-45c0-bdf3-618372bd71a1')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '9c7077a2-6576-477e-8d49-098633768b7b')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('9c7077a2-6576-477e-8d49-098633768b7b')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '60145ec0-e4d4-4b01-8dde-7a4f3269cce5')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('60145ec0-e4d4-4b01-8dde-7a4f3269cce5')

GO

GO
PRINT N'更新完成。';


GO
