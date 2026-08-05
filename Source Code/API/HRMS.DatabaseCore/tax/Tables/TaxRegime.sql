CREATE TABLE [tax].[TaxRegime] (
    [TaxRegimeID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]      BIGINT         NOT NULL,
    [RegimeName]    NVARCHAR (100) NOT NULL,
    [EffectiveFrom] DATE           NOT NULL,
    [EffectiveTo]   DATE           NULL,
    [CreatedBy]     BIGINT         NOT NULL,
    [CreatedDate]   DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]    BIGINT         NULL,
    [ModifiedDate]  DATETIME2 (7)  NULL,
    [DeletedBy]     BIGINT         NULL,
    [DeletedDate]   DATETIME2 (7)  NULL,
    [IsDeleted]     BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]    ROWVERSION     NOT NULL,
    CONSTRAINT [PK_TaxRegime] PRIMARY KEY CLUSTERED ([TaxRegimeID] ASC),
    CONSTRAINT [FK_TaxRegime_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

