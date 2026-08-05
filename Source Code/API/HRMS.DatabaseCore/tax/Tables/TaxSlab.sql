CREATE TABLE [tax].[TaxSlab] (
    [TaxSlabID]    BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]     BIGINT          NOT NULL,
    [TaxRegimeID]  BIGINT          NOT NULL,
    [IncomeFrom]   DECIMAL (18, 2) NOT NULL,
    [IncomeTo]     DECIMAL (18, 2) NULL,
    [TaxRate]      DECIMAL (5, 2)  NOT NULL,
    [CreatedBy]    BIGINT          NOT NULL,
    [CreatedDate]  DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]   BIGINT          NULL,
    [ModifiedDate] DATETIME2 (7)   NULL,
    [DeletedBy]    BIGINT          NULL,
    [DeletedDate]  DATETIME2 (7)   NULL,
    [IsDeleted]    BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]   ROWVERSION      NOT NULL,
    CONSTRAINT [PK_TaxSlab] PRIMARY KEY CLUSTERED ([TaxSlabID] ASC),
    CONSTRAINT [FK_TaxSlab_Regime] FOREIGN KEY ([TaxRegimeID]) REFERENCES [tax].[TaxRegime] ([TaxRegimeID]),
    CONSTRAINT [FK_TaxSlab_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_TaxSlab_Regime]
    ON [tax].[TaxSlab]([TaxRegimeID] ASC) WHERE ([IsDeleted]=(0));

