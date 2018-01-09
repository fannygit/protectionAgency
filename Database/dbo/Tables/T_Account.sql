CREATE TABLE [dbo].[T_Account] (
    [Id]            NVARCHAR (32)   NOT NULL,
    [Account]       NVARCHAR (256)  NOT NULL,
	[Name]       NVARCHAR (256)   NULL,
    [Password]      NVARCHAR (256)  NOT NULL,
    [Status]        INT             CONSTRAINT [DF_T_Account_Status] DEFAULT (0) NOT NULL,
    [CreateDate]    DATETIME        CONSTRAINT [DF_T_Account_CreateDate] DEFAULT (getdate()) NOT NULL,
    [LastLoginTime] DATETIME        NULL DEFAULT (getdate()),
    [UpdateDate ] DATETIME NOT NULL DEFAULT (getdate()), 
    [CreateUserID] NVARCHAR(32) NOT NULL, 
    [UpdateUserID] NVARCHAR(32) NOT NULL, 
    [IP] NVARCHAR(256) NULL, 
    CONSTRAINT [PK_T_Account] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO



GO



GO

GO

GO
