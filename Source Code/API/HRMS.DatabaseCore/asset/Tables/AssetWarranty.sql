CREATE TABLE [asset].[AssetWarranty] (
    [AssetWarrantyID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]          BIGINT         NOT NULL,
    [AssetID]           BIGINT         NOT NULL,
    [WarrantyStartDate] DATE           NOT NULL,
    [WarrantyEndDate]   DATE           NOT NULL,
    [WarrantyProvider]  NVARCHAR (150) NOT NULL,
    [CreatedBy]         BIGINT         NOT NULL,
    [CreatedDate]       DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        BIGINT         NULL,
    [ModifiedDate]      DATETIME2 (7)  NULL,
    [DeletedBy]         BIGINT         NULL,
    [DeletedDate]       DATETIME2 (7)  NULL,
    [IsDeleted]         BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL,
    CONSTRAINT [PK_AssetWarranty] PRIMARY KEY CLUSTERED ([AssetWarrantyID] ASC),
    CONSTRAINT [FK_AssetWarranty_Asset] FOREIGN KEY ([AssetID]) REFERENCES [asset].[AssetMaster] ([AssetID]),
    CONSTRAINT [FK_AssetWarranty_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Warranty_Asset]
    ON [asset].[AssetWarranty]([AssetID] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Warranty_EndDate]
    ON [asset].[AssetWarranty]([WarrantyEndDate] ASC) WHERE ([IsDeleted]=(0));

