CREATE TABLE [dbo].[Application] (
    [PackageFamilyName]		NVARCHAR (500) NOT NULL, 
    [Name]                  NVARCHAR (255) NOT NULL,
    [Publisher]             NVARCHAR (255) NOT NULL,
    [Version]               NVARCHAR (25)  NOT NULL,
    [ProcessorArchitecture] NVARCHAR (25)  NOT NULL,
    [DisplayName]           NVARCHAR (255) NOT NULL,
    [PublisherDisplayName]  NVARCHAR (255) NOT NULL,
    [InstallMode]			NVARCHAR(50) NOT NULL, 
    [Description]			NVARCHAR(2000) NULL, 
    CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED ([PackageFamilyName]) 
);


GO

CREATE UNIQUE INDEX [IX_Application_PackageFamilyName] ON [dbo].[Application] ([PackageFamilyName])
