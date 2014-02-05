CREATE TABLE [dbo].[Application] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [Name]                  NVARCHAR (255) NOT NULL,
    [Publisher]             NVARCHAR (255) NOT NULL,
    [Version]               NVARCHAR (25)  NOT NULL,
    [ProcessorArchitecture] NVARCHAR (25)  NOT NULL,
    [DisplayName]           NVARCHAR (255) NOT NULL,
    [PublisherDisplayName]  NVARCHAR (255) NOT NULL,
    [Description] NVARCHAR(2000) NULL, 
    CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED ([ID] ASC) 
);


GO

CREATE UNIQUE INDEX [IX_Application_Name] ON [dbo].[Application] ([Name])
