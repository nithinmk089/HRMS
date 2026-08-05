CREATE TABLE [asset].[AssetDepreciation] (
    [AssetDepreciationID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]            BIGINT          NOT NULL,
    [AssetID]             BIGINT          NOT NULL,
    [DepreciationMethod]  NVARCHAR (50)   NOT NULL,
    [DepreciationRate]    DECIMAL (5, 2)  NOT NULL,
    [BookValue]           DECIMAL (18, 2) NOT NULL,
    [CreatedBy]           BIGINT          NOT NULL,
    [CreatedDate]         DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]          BIGINT          NULL,
    [ModifiedDate]        DATETIME2 (7)   NULL,
    [DeletedBy]           BIGINT          NULL,
    [DeletedDate]         DATETIME2 (7)   NULL,
    [IsDeleted]           BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]          ROWVERSION      NOT NULL,
    CONSTRAINT [PK_AssetDepreciation] PRIMARY KEY CLUSTERED ([AssetDepreciationID] ASC),
    CONSTRAINT [FK_AssetDepreciation_Asset] FOREIGN KEY ([AssetID]) REFERENCES [asset].[AssetMaster] ([AssetID]),
    CONSTRAINT [FK_AssetDepreciation_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_AssetDepreciation_Asset]
    ON [asset].[AssetDepreciation]([AssetID] ASC) WHERE ([IsDeleted]=(0));

