CREATE TABLE [performance].[CompetencyFramework] (
    [CompetencyFrameworkID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]              BIGINT          NOT NULL,
    [FrameworkName]         NVARCHAR (100)  NOT NULL,
    [Description]           NVARCHAR (1000) NULL,
    [CreatedBy]             BIGINT          NOT NULL,
    [CreatedDate]           DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]            BIGINT          NULL,
    [ModifiedDate]          DATETIME2 (7)   NULL,
    [DeletedBy]             BIGINT          NULL,
    [DeletedDate]           DATETIME2 (7)   NULL,
    [IsDeleted]             BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]            ROWVERSION      NOT NULL,
    CONSTRAINT [PK_CompetencyFramework] PRIMARY KEY CLUSTERED ([CompetencyFrameworkID] ASC),
    CONSTRAINT [FK_CompetencyFramework_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

