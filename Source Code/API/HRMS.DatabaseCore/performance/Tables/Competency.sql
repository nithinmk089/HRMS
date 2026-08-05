CREATE TABLE [performance].[Competency] (
    [CompetencyID]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]              BIGINT         NOT NULL,
    [CompetencyFrameworkID] BIGINT         NOT NULL,
    [CompetencyName]        NVARCHAR (100) NOT NULL,
    [CompetencyLevel]       NVARCHAR (50)  NOT NULL,
    [CreatedBy]             BIGINT         NOT NULL,
    [CreatedDate]           DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]            BIGINT         NULL,
    [ModifiedDate]          DATETIME2 (7)  NULL,
    [DeletedBy]             BIGINT         NULL,
    [DeletedDate]           DATETIME2 (7)  NULL,
    [IsDeleted]             BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]            ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Competency] PRIMARY KEY CLUSTERED ([CompetencyID] ASC),
    CONSTRAINT [FK_Competency_Framework] FOREIGN KEY ([CompetencyFrameworkID]) REFERENCES [performance].[CompetencyFramework] ([CompetencyFrameworkID]),
    CONSTRAINT [FK_Competency_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

