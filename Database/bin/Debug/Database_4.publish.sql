/*
jz-proj-ezbuy-b 的部署指令碼

這段程式碼由工具產生。
變更這個檔案可能導致不正確的行為，而且如果重新產生程式碼，
變更將會遺失。
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "jz-proj-ezbuy-b"
:setvar DefaultFilePrefix "jz-proj-ezbuy-b"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\"

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
/*
正在卸除資料行 [dbo].[T_Account_Permissions_Mapping].[Permission]，因此可能導致資料遺失。

必須加入資料表 [dbo].[T_Account_Permissions_Mapping] 的資料行 [dbo].[T_Account_Permissions_Mapping].[PermissionId]，但該資料行沒有預設值而且不允許 NULL 值。如果資料表包含資料，則 ALTER 指令碼將無法運作。若要避免這個問題，您必須: 在資料行加入預設值、將它標示為允許 NULL 值，或啟用產生智慧型預設值做為部署選項。
*/

IF EXISTS (select top 1 1 from [dbo].[T_Account_Permissions_Mapping])
    RAISERROR (N'偵測到資料列。結構描述更新即將終止，因為可能造成資料遺失。', 16, 127) WITH NOWAIT

GO
PRINT N'已略過索引鍵為 a209de94-04ac-488c-b705-26f2f1864c3d, 643b89de-e41c-4614-8f91-c003de19be8e, 3e06c0c6-df04-46e6-99e1-34f95ba242d2, 60145ec0-e4d4-4b01-8dde-7a4f3269cce5 的重新命名重構作業，項目 [dbo].[T_Account_Permissions_Mapping].[RoleId] (SqlSimpleColumn) 將不會重新命名為 PermissionId';


GO
PRINT N'已略過索引鍵為 07fa5ea3-4197-42dd-842d-6d8e229023b1 的重新命名重構作業，項目 [dbo].[T_Account_Area_Mapping].[RoleId] (SqlSimpleColumn) 將不會重新命名為 AccountId';


GO
PRINT N'已略過索引鍵為 ff593c51-7a60-446f-a0f4-a36f6fedaef0 的重新命名重構作業，項目 [dbo].[T_Account_Area_Mapping].[PermissionId] (SqlSimpleColumn) 將不會重新命名為 AreaId';


GO
PRINT N'已略過索引鍵為 4e730aa2-eb2f-45c0-bdf3-618372bd71a1, 9c7077a2-6576-477e-8d49-098633768b7b 的重新命名重構作業，項目 [dbo].[T_Member_Friends].[InviteID] (SqlSimpleColumn) 將不會重新命名為 InviterMemberID';


GO
PRINT N'正在卸除 [dbo].[T_Account_Permissions_Mapping] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Account_Permissions_Mapping] DROP CONSTRAINT [DF__T_Account__Creat__34C8D9D1];


GO
PRINT N'開始重建資料表 [dbo].[T_Account_Permissions_Mapping]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_T_Account_Permissions_Mapping] (
    [Id]           NVARCHAR (32) NOT NULL,
    [AccountId]    NVARCHAR (32) NOT NULL,
    [PermissionId] NVARCHAR (32) NOT NULL,
    [CreateUserId] NVARCHAR (32) NOT NULL,
    [CreateDate]   DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[T_Account_Permissions_Mapping])
    BEGIN
        INSERT INTO [dbo].[tmp_ms_xx_T_Account_Permissions_Mapping] ([Id], [AccountId], [CreateUserId], [CreateDate])
        SELECT   [Id],
                 [AccountId],
                 [CreateUserId],
                 [CreateDate]
        FROM     [dbo].[T_Account_Permissions_Mapping]
        ORDER BY [Id] ASC;
    END

DROP TABLE [dbo].[T_Account_Permissions_Mapping];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_T_Account_Permissions_Mapping]', N'T_Account_Permissions_Mapping';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


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
