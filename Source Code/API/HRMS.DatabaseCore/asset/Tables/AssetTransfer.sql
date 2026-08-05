CREATE TABLE [asset].[AssetTransfer] (
    [AssetTransferID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]        BIGINT         NOT NULL,
    [AssetID]         BIGINT         NOT NULL,
    [FromEmployeeID]  BIGINT         NULL,
    [ToEmployeeID]    BIGINT         NOT NULL,
    [TransferDate]    DATE           NOT NULL,
    [TransferReason]  NVARCHAR (500) NULL,
    [TransferStatus]  NVARCHAR (50)  DEFAULT ('Pending') NOT NULL,
    [CreatedBy]       BIGINT         NOT NULL,
    [CreatedDate]     DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]      BIGINT         NULL,
    [ModifiedDate]    DATETIME2 (7)  NULL,
    [DeletedBy]       BIGINT         NULL,
    [DeletedDate]     DATETIME2 (7)  NULL,
    [IsDeleted]       BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]      ROWVERSION     NOT NULL,
    CONSTRAINT [PK_AssetTransfer] PRIMARY KEY CLUSTERED ([AssetTransferID] ASC),
    CONSTRAINT [FK_AssetTransfer_Asset] FOREIGN KEY ([AssetID]) REFERENCES [asset].[AssetMaster] ([AssetID]),
    CONSTRAINT [FK_AssetTransfer_FromEmployee] FOREIGN KEY ([FromEmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_AssetTransfer_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID]),
    CONSTRAINT [FK_AssetTransfer_ToEmployee] FOREIGN KEY ([ToEmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID])
);


GO
CREATE NONCLUSTERED INDEX [IX_AssetTransfer_Asset]
    ON [asset].[AssetTransfer]([AssetID] ASC) WHERE ([IsDeleted]=(0));

