CREATE TABLE [asset].[AssetRepair] (
    [AssetRepairID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]      BIGINT          NOT NULL,
    [AssetID]       BIGINT          NOT NULL,
    [RepairDate]    DATE            NOT NULL,
    [RepairReason]  NVARCHAR (500)  NULL,
    [RepairCost]    DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [RepairStatus]  NVARCHAR (50)   DEFAULT ('Pending') NOT NULL,
    [CreatedBy]     BIGINT          NOT NULL,
    [CreatedDate]   DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]    BIGINT          NULL,
    [ModifiedDate]  DATETIME2 (7)   NULL,
    [DeletedBy]     BIGINT          NULL,
    [DeletedDate]   DATETIME2 (7)   NULL,
    [IsDeleted]     BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]    ROWVERSION      NOT NULL,
    CONSTRAINT [PK_AssetRepair] PRIMARY KEY CLUSTERED ([AssetRepairID] ASC),
    CONSTRAINT [FK_AssetRepair_Asset] FOREIGN KEY ([AssetID]) REFERENCES [asset].[AssetMaster] ([AssetID]),
    CONSTRAINT [FK_AssetRepair_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_AssetRepair_Asset]
    ON [asset].[AssetRepair]([AssetID] ASC) WHERE ([IsDeleted]=(0));

