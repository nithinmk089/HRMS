CREATE TABLE [asset].[AssetInventory] (
    [AssetInventoryID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]         BIGINT        NOT NULL,
    [AssetID]          BIGINT        NOT NULL,
    [LocationID]       BIGINT        NOT NULL,
    [Quantity]         INT           DEFAULT ((1)) NOT NULL,
    [InventoryStatus]  NVARCHAR (50) DEFAULT ('InStock') NOT NULL,
    [CreatedBy]        BIGINT        NOT NULL,
    [CreatedDate]      DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]       BIGINT        NULL,
    [ModifiedDate]     DATETIME2 (7) NULL,
    [DeletedBy]        BIGINT        NULL,
    [DeletedDate]      DATETIME2 (7) NULL,
    [IsDeleted]        BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]       ROWVERSION    NOT NULL,
    CONSTRAINT [PK_AssetInventory] PRIMARY KEY CLUSTERED ([AssetInventoryID] ASC),
    CONSTRAINT [FK_AssetInventory_Asset] FOREIGN KEY ([AssetID]) REFERENCES [asset].[AssetMaster] ([AssetID]),
    CONSTRAINT [FK_AssetInventory_Location] FOREIGN KEY ([LocationID]) REFERENCES [organization].[Location] ([LocationID]),
    CONSTRAINT [FK_AssetInventory_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_AssetInventory_Location]
    ON [asset].[AssetInventory]([LocationID] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_AssetInventory_Asset]
    ON [asset].[AssetInventory]([AssetID] ASC) WHERE ([IsDeleted]=(0));

