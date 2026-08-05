CREATE TABLE [asset].[AssetMaintenance] (
    [AssetMaintenanceID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT          NOT NULL,
    [AssetID]            BIGINT          NOT NULL,
    [MaintenanceDate]    DATE            NOT NULL,
    [MaintenanceType]    NVARCHAR (100)  NOT NULL,
    [VendorName]         NVARCHAR (150)  NULL,
    [Cost]               DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [MaintenanceStatus]  NVARCHAR (50)   DEFAULT ('Scheduled') NOT NULL,
    [CreatedBy]          BIGINT          NOT NULL,
    [CreatedDate]        DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT          NULL,
    [ModifiedDate]       DATETIME2 (7)   NULL,
    [DeletedBy]          BIGINT          NULL,
    [DeletedDate]        DATETIME2 (7)   NULL,
    [IsDeleted]          BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION      NOT NULL,
    CONSTRAINT [PK_AssetMaintenance] PRIMARY KEY CLUSTERED ([AssetMaintenanceID] ASC),
    CONSTRAINT [FK_AssetMaintenance_Asset] FOREIGN KEY ([AssetID]) REFERENCES [asset].[AssetMaster] ([AssetID]),
    CONSTRAINT [FK_AssetMaintenance_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_AssetMaintenance_Asset]
    ON [asset].[AssetMaintenance]([AssetID] ASC) WHERE ([IsDeleted]=(0));

