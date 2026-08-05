CREATE TABLE [performance].[AppraisalSection] (
    [AppraisalSectionID]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]            BIGINT         NOT NULL,
    [AppraisalTemplateID] BIGINT         NOT NULL,
    [SectionName]         NVARCHAR (100) NOT NULL,
    [Weightage]           DECIMAL (5, 2) NOT NULL,
    [SequenceNo]          INT            NOT NULL,
    [CreatedBy]           BIGINT         NOT NULL,
    [CreatedDate]         DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]          BIGINT         NULL,
    [ModifiedDate]        DATETIME2 (7)  NULL,
    [DeletedBy]           BIGINT         NULL,
    [DeletedDate]         DATETIME2 (7)  NULL,
    [IsDeleted]           BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]          ROWVERSION     NOT NULL,
    CONSTRAINT [PK_AppraisalSection] PRIMARY KEY CLUSTERED ([AppraisalSectionID] ASC),
    CONSTRAINT [FK_AppraisalSection_Template] FOREIGN KEY ([AppraisalTemplateID]) REFERENCES [performance].[AppraisalTemplate] ([AppraisalTemplateID]),
    CONSTRAINT [FK_AppraisalSection_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

