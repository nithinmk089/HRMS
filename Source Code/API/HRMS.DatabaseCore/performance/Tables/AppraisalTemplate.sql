CREATE TABLE [performance].[AppraisalTemplate] (
    [AppraisalTemplateID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]            BIGINT         NOT NULL,
    [TemplateCode]        NVARCHAR (50)  NOT NULL,
    [TemplateName]        NVARCHAR (100) NOT NULL,
    [EffectiveFrom]       DATE           NOT NULL,
    [EffectiveTo]         DATE           NULL,
    [CreatedBy]           BIGINT         NOT NULL,
    [CreatedDate]         DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]          BIGINT         NULL,
    [ModifiedDate]        DATETIME2 (7)  NULL,
    [DeletedBy]           BIGINT         NULL,
    [DeletedDate]         DATETIME2 (7)  NULL,
    [IsDeleted]           BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]          ROWVERSION     NOT NULL,
    CONSTRAINT [PK_AppraisalTemplate] PRIMARY KEY CLUSTERED ([AppraisalTemplateID] ASC),
    CONSTRAINT [FK_AppraisalTemplate_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

