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
            SET PAGE_VERIFY NONE,
                DISABLE_BROKER 
            WITH ROLLBACK IMMEDIATE;
    END


GO
USE [$(DatabaseName)];


GO
IF fulltextserviceproperty(N'IsFulltextInstalled') = 1
    EXECUTE sp_fulltext_database 'enable';


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
PRINT N'正在建立 [dbo].[T_Account_Area_Mapping]...';


GO
CREATE TABLE [dbo].[T_Account_Area_Mapping] (
    [Id]           NVARCHAR (32) NOT NULL,
    [AccountId]    NVARCHAR (32) NOT NULL,
    [AreaId]       NVARCHAR (32) NOT NULL,
    [CreateUserId] NVARCHAR (32) NOT NULL,
    [CreateDate]   DATETIME      NOT NULL,
    CONSTRAINT [PK_T_Account_Group_Mapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Account_Log]...';


GO
CREATE TABLE [dbo].[T_Account_Log] (
    [Id]         NVARCHAR (32) NOT NULL,
    [AccountID]  NVARCHAR (32) NOT NULL,
    [Status]     INT           NOT NULL,
    [CreateDate] DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Account_Permissions_Mapping]...';


GO
CREATE TABLE [dbo].[T_Account_Permissions_Mapping] (
    [Id]           NVARCHAR (32) NOT NULL,
    [AccountId]    NVARCHAR (32) NOT NULL,
    [Permission]   NVARCHAR (32) NOT NULL,
    [CreateUserId] NVARCHAR (32) NOT NULL,
    [CreateDate]   DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Area]...';


GO
CREATE TABLE [dbo].[T_Area] (
    [Id]         NVARCHAR (32)  NOT NULL,
    [Area]       NVARCHAR (256) NOT NULL,
    [AreaEN]     NVARCHAR (256) NOT NULL,
    [AreaNumber] INT            NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Device]...';


GO
CREATE TABLE [dbo].[T_Device] (
    [Id]         NVARCHAR (32)  NOT NULL,
    [MemberID]   NVARCHAR (32)  NOT NULL,
    [DXID]       NVARCHAR (50)  NOT NULL,
    [Token]      NVARCHAR (256) NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Device_PhoneChange]...';


GO
CREATE TABLE [dbo].[T_Device_PhoneChange] (
    [Id]           NVARCHAR (32) NOT NULL,
    [MemberID]     NVARCHAR (32) NOT NULL,
    [DXID]         NVARCHAR (50) NOT NULL,
    [SecurityCode] NVARCHAR (50) NOT NULL,
    [Phone]        NVARCHAR (50) NOT NULL,
    [CreateDate]   DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Device_PushLog]...';


GO
CREATE TABLE [dbo].[T_Device_PushLog] (
    [Id]         NVARCHAR (32)  NOT NULL,
    [MemberID]   NVARCHAR (32)  NOT NULL,
    [Input]      NVARCHAR (MAX) NOT NULL,
    [Output]     NVARCHAR (MAX) NOT NULL,
    [Status]     INT            NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Feedback]...';


GO
CREATE TABLE [dbo].[T_Feedback] (
    [Id]             NVARCHAR (32)  NOT NULL,
    [MemberID]       NVARCHAR (32)  NOT NULL,
    [Title]          NVARCHAR (32)  NOT NULL,
    [Contents]       NVARCHAR (MAX) NOT NULL,
    [AnswerContents] NVARCHAR (MAX) NOT NULL,
    [AnswerAccount]  NVARCHAR (32)  NOT NULL,
    [UpdateDate ]    DATETIME       NOT NULL,
    [CreateDate]     DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Friends]...';


GO
CREATE TABLE [dbo].[T_Friends] (
    [Id]              NVARCHAR (32) NOT NULL,
    [MemberID]        NVARCHAR (32) NOT NULL,
    [InviterMemberID] NVARCHAR (32) NOT NULL,
    [Status]          INT           NOT NULL,
    [CreateDate]      DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_FriendsGroup]...';


GO
CREATE TABLE [dbo].[T_FriendsGroup] (
    [Id]         NVARCHAR (32)  NOT NULL,
    [Number]     INT            NOT NULL,
    [Name]       NVARCHAR (100) NOT NULL,
    [Source]     INT            NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_InBox]...';


GO
CREATE TABLE [dbo].[T_InBox] (
    [Id]             NVARCHAR (32)  NOT NULL,
    [MemberID]       NVARCHAR (32)  NOT NULL,
    [Title]          NVARCHAR (32)  NOT NULL,
    [Contents]       NVARCHAR (MAX) NULL,
    [Type]           INT            NOT NULL,
    [MessageType]    INT            NOT NULL,
    [Status]         INT            NOT NULL,
    [Source]         INT            NOT NULL,
    [CreateMemberID] NVARCHAR (32)  NOT NULL,
    [CreateDate]     DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Member]...';


GO
CREATE TABLE [dbo].[T_Member] (
    [Id]                NVARCHAR (32)  NOT NULL,
    [Member]            NVARCHAR (256) NOT NULL,
    [Name]              NVARCHAR (256) NOT NULL,
    [PhoneNumber]       NVARCHAR (256) NOT NULL,
    [RemittanceAccount] NVARCHAR (5)   NOT NULL,
    [AreaId]            NVARCHAR (32)  NOT NULL,
    [Remark]            NVARCHAR (MAX) NULL,
    [StartTime]         DATETIME       NULL,
    [EndTime]           DATETIME       NULL,
    [Address]           NVARCHAR (MAX) NULL,
    [Source]            INT            NOT NULL,
    [LastLoginDate]     DATETIME       NULL,
    [CreateDate]        DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Member_ContactPhone]...';


GO
CREATE TABLE [dbo].[T_Member_ContactPhone] (
    [Id]         NVARCHAR (32)  NOT NULL,
    [MemberID]   NVARCHAR (32)  NOT NULL,
    [Phone]      NVARCHAR (256) NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Member_FriendsGroup_Mapping]...';


GO
CREATE TABLE [dbo].[T_Member_FriendsGroup_Mapping] (
    [Id]         NVARCHAR (32) NOT NULL,
    [GroupID]    NVARCHAR (32) NOT NULL,
    [MemberID]   NVARCHAR (32) NOT NULL,
    [CreateDate] DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Member_ReceiveAddress]...';


GO
CREATE TABLE [dbo].[T_Member_ReceiveAddress] (
    [Id]         NVARCHAR (32)  NOT NULL,
    [MemberID]   NVARCHAR (32)  NULL,
    [Address]    NVARCHAR (256) NULL,
    [CreateDate] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Member_ShoppingGroup_Mapping]...';


GO
CREATE TABLE [dbo].[T_Member_ShoppingGroup_Mapping] (
    [Id]              NVARCHAR (32) NOT NULL,
    [ShoppingGroupId] NVARCHAR (32) NOT NULL,
    [MemberID]        NVARCHAR (32) NOT NULL,
    [Status]          INT           NOT NULL,
    [Source]          INT           NOT NULL,
    [CreateDate]      DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Member_ShoppingGroupDefaultSetting]...';


GO
CREATE TABLE [dbo].[T_Member_ShoppingGroupDefaultSetting] (
    [Id]           NVARCHAR (32)  NOT NULL,
    [MemberID]     NVARCHAR (32)  NOT NULL,
    [EndTime]      DATETIME       NULL,
    [EndWeekDate]  INT            NULL,
    [EndWeekTime]  DATETIME       NULL,
    [Type]         INT            NOT NULL,
    [ISDirectPay]  INT            NOT NULL,
    [ISRemittance] INT            NOT NULL,
    [Explain]      NVARCHAR (MAX) NULL,
    [ServiceCosts] INT            NULL,
    [StorageCosts] INT            NULL,
    [UpdateDate ]  DATETIME       NOT NULL,
    [CreateDate]   DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Member_ShoppingGroupDefaultSetting_ReceiveMethod]...';


GO
CREATE TABLE [dbo].[T_Member_ShoppingGroupDefaultSetting_ReceiveMethod] (
    [Id]              NVARCHAR (32)  NOT NULL,
    [ShoppingGroupId] NVARCHAR (32)  NOT NULL,
    [StartTime]       DATETIME       NULL,
    [EndTime]         DATETIME       NULL,
    [Address]         NVARCHAR (256) NOT NULL,
    [CreateDate]      DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Merchandise]...';


GO
CREATE TABLE [dbo].[T_Merchandise] (
    [Id]              NVARCHAR (32)  NOT NULL,
    [ShoppingGroupId] NVARCHAR (32)  NOT NULL,
    [Name]            NVARCHAR (100) NOT NULL,
    [FileName]        NVARCHAR (100) NOT NULL,
    [Unit]            NVARCHAR (32)  NOT NULL,
    [Explain]         NVARCHAR (100) NULL,
    [CreateUserId]    NVARCHAR (32)  NOT NULL,
    [UpdateUserId]    NVARCHAR (32)  NOT NULL,
    [UpdateDate ]     DATETIME       NOT NULL,
    [CreateDate]      DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Merchandise_Price]...';


GO
CREATE TABLE [dbo].[T_Merchandise_Price] (
    [Id]            NVARCHAR (32) NOT NULL,
    [MerchandiseID] NVARCHAR (32) NOT NULL,
    [OfficialPrice] FLOAT (53)    NOT NULL,
    [EditPrice]     FLOAT (53)    NOT NULL,
    [QuantityLevel] INT           NOT NULL,
    [CreateDate]    DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Order]...';


GO
CREATE TABLE [dbo].[T_Order] (
    [Id]                NVARCHAR (32)  NOT NULL,
    [MemberID]          NVARCHAR (32)  NOT NULL,
    [ShoppingGroupId]   NVARCHAR (32)  NOT NULL,
    [TransactionCode]   NVARCHAR (100) NOT NULL,
    [ReceiveStartDate]  DATETIME       NULL,
    [ReceiveEndDate]    DATETIME       NULL,
    [ReceiveAddress]    NVARCHAR (100) NOT NULL,
    [PayMode]           INT            NOT NULL,
    [ISPay]             INT            NOT NULL,
    [RemittanceAccount] NVARCHAR (5)   NOT NULL,
    [UpdateDate ]       DATETIME       NOT NULL,
    [CreateDate]        DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_OrderDetail]...';


GO
CREATE TABLE [dbo].[T_OrderDetail] (
    [Id]            NVARCHAR (32) NOT NULL,
    [OrderId]       NVARCHAR (32) NOT NULL,
    [MerchandiseId] NVARCHAR (32) NOT NULL,
    [Quantity]      INT           NOT NULL,
    [CreateDate]    DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_ShoppingGroup]...';


GO
CREATE TABLE [dbo].[T_ShoppingGroup] (
    [Id]               NVARCHAR (32)  NOT NULL,
    [Name]             NVARCHAR (32)  NOT NULL,
    [FileName]         NVARCHAR (100) NULL,
    [EndDate ]         DATETIME       NOT NULL,
    [Quantity]         INT            NOT NULL,
    [MultipleQuantity] INT            NOT NULL,
    [TotalSum]         INT            NOT NULL,
    [ISDirectPay]      INT            NOT NULL,
    [ISRemittance]     INT            NOT NULL,
    [Source]           INT            NOT NULL,
    [EndMethod]        INT            NOT NULL,
    [Explain]          NVARCHAR (MAX) NOT NULL,
    [ServiceCosts]     INT            NOT NULL,
    [StorageCosts]     INT            NOT NULL,
    [Status]           INT            NOT NULL,
    [CreateUserId]     NVARCHAR (32)  NOT NULL,
    [UpdateUserId]     NVARCHAR (32)  NOT NULL,
    [UpdateDate ]      DATETIME       NOT NULL,
    [CreateDate]       DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_ShoppingGroup_Blog]...';


GO
CREATE TABLE [dbo].[T_ShoppingGroup_Blog] (
    [Id] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_ShoppingGroup_ReceiveMethod]...';


GO
CREATE TABLE [dbo].[T_ShoppingGroup_ReceiveMethod] (
    [Id]              NVARCHAR (32)  NOT NULL,
    [ShoppingGroupId] NVARCHAR (32)  NOT NULL,
    [StartTime]       DATETIME       NULL,
    [EndTime]         DATETIME       NULL,
    [Address]         NVARCHAR (256) NOT NULL,
    [CreateDate]      DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'正在建立 [dbo].[T_Version]...';


GO
CREATE TABLE [dbo].[T_Version] (
    [Id]            NVARCHAR (32)  NOT NULL,
    [AppName]       NVARCHAR (256) NOT NULL,
    [Version]       NVARCHAR (32)  NOT NULL,
    [UpdateAccount] NVARCHAR (32)  NOT NULL,
    [UpdateDate ]   DATETIME       NOT NULL,
    [CreateDate]    DATETIME       NOT NULL,
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
PRINT N'正在建立 [dbo].[T_Account_Area_Mapping] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Account_Area_Mapping]
    ADD DEFAULT (getdate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Account_Log] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Account_Log]
    ADD DEFAULT (getdate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Account_Permissions_Mapping] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Account_Permissions_Mapping]
    ADD DEFAULT (getdate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Area] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Area]
    ADD DEFAULT (getdate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Device_PhoneChange] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Device_PhoneChange]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Device_PushLog] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Device_PushLog]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Feedback] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Feedback]
    ADD DEFAULT (getDate()) FOR [UpdateDate ];


GO
PRINT N'正在建立 [dbo].[T_Feedback] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Feedback]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Friends] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Friends]
    ADD DEFAULT (0) FOR [Status];


GO
PRINT N'正在建立 [dbo].[T_Friends] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Friends]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_FriendsGroup] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_FriendsGroup]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_InBox] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_InBox]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Member] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Member]
    ADD DEFAULT (getdate()) FOR [LastLoginDate];


GO
PRINT N'正在建立 [dbo].[T_Member] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Member]
    ADD DEFAULT (getdate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Member_ContactPhone] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Member_ContactPhone]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Member_FriendsGroup_Mapping] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Member_FriendsGroup_Mapping]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Member_ReceiveAddress] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Member_ReceiveAddress]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Member_ShoppingGroup_Mapping] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Member_ShoppingGroup_Mapping]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Member_ShoppingGroupDefaultSetting] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Member_ShoppingGroupDefaultSetting]
    ADD DEFAULT (0) FOR [ISDirectPay];


GO
PRINT N'正在建立 [dbo].[T_Member_ShoppingGroupDefaultSetting] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Member_ShoppingGroupDefaultSetting]
    ADD DEFAULT (0) FOR [ISRemittance];


GO
PRINT N'正在建立 [dbo].[T_Member_ShoppingGroupDefaultSetting] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Member_ShoppingGroupDefaultSetting]
    ADD DEFAULT (getDate()) FOR [UpdateDate ];


GO
PRINT N'正在建立 [dbo].[T_Member_ShoppingGroupDefaultSetting] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Member_ShoppingGroupDefaultSetting]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Member_ShoppingGroupDefaultSetting_ReceiveMethod] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Member_ShoppingGroupDefaultSetting_ReceiveMethod]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Merchandise] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Merchandise]
    ADD DEFAULT (getDate()) FOR [UpdateDate ];


GO
PRINT N'正在建立 [dbo].[T_Merchandise] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Merchandise]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Merchandise_Price] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Merchandise_Price]
    ADD DEFAULT (0.0) FOR [OfficialPrice];


GO
PRINT N'正在建立 [dbo].[T_Merchandise_Price] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Merchandise_Price]
    ADD DEFAULT (0.0) FOR [EditPrice];


GO
PRINT N'正在建立 [dbo].[T_Merchandise_Price] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Merchandise_Price]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Order] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Order]
    ADD DEFAULT (0) FOR [ISPay];


GO
PRINT N'正在建立 [dbo].[T_Order] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Order]
    ADD DEFAULT (getDate()) FOR [UpdateDate ];


GO
PRINT N'正在建立 [dbo].[T_Order] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Order]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_ShoppingGroup] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_ShoppingGroup]
    ADD DEFAULT (0) FOR [ISDirectPay];


GO
PRINT N'正在建立 [dbo].[T_ShoppingGroup] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_ShoppingGroup]
    ADD DEFAULT (0) FOR [ISRemittance];


GO
PRINT N'正在建立 [dbo].[T_ShoppingGroup] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_ShoppingGroup]
    ADD DEFAULT (0) FOR [Status];


GO
PRINT N'正在建立 [dbo].[T_ShoppingGroup_ReceiveMethod] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_ShoppingGroup_ReceiveMethod]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'正在建立 [dbo].[T_Version] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Version]
    ADD DEFAULT (getDate()) FOR [UpdateDate ];


GO
PRINT N'正在建立 [dbo].[T_Version] 上的未命名條件約束...';


GO
ALTER TABLE [dbo].[T_Version]
    ADD DEFAULT (getDate()) FOR [CreateDate];


GO
PRINT N'更新完成。';


GO
