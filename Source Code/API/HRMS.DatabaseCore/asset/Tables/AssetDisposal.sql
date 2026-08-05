CREATE TABLE [asset].[AssetDisposal] (
    [AssetDisposalID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]        BIGINT          NOT NULL,
    [AssetID]         BIGINT          NOT NULL,
    [DisposalDate]    DATE            NOT NULL,
    [DisposalMethod]  NVARCHAR (100)  NOT NULL,
    [DisposalValue]   DECIMAL (18, 2) NOT NULL,
    [DisposalStatus]  NVARCHAR (50)   DEFAULT ('Pending') NOT NULL,
    [CreatedBy]       BIGINT          NOT NULL,
    [CreatedDate]     DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]      BIGINT          NULL,
    [ModifiedDate]    DATETIME2 (7)   NULL,
    [DeletedBy]       BIGINT          NULL,
    [DeletedDate]     DATETIME2 (7)   NULL,
    [IsDeleted]       BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]      ROWVERSION      NOT NULL,
    CONSTRAINT [PK_AssetDisposal] PRIMARY KEY CLUSTERED ([AssetDisposalID] ASC),
    CONSTRAINT [FK_AssetDisposal_Asset] FOREIGN KEY ([AssetID]) REFERENCES [asset].[AssetMaster] ([AssetID]),
    CONSTRAINT [FK_AssetDisposal_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_AssetDisposal_Asset]
    ON [asset].[AssetDisposal]([AssetID] ASC) WHERE ([IsDeleted]=(0));

