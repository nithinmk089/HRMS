CREATE TABLE [asset].[AssetMaster] (
    [AssetID]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]         BIGINT          NOT NULL,
    [AssetCode]        NVARCHAR (50)   NOT NULL,
    [AssetTag]         NVARCHAR (50)   NOT NULL,
    [AssetName]        NVARCHAR (150)  NOT NULL,
    [AssetCategoryID]  BIGINT          NOT NULL,
    [Manufacturer]     NVARCHAR (100)  NULL,
    [Model]            NVARCHAR (100)  NULL,
    [SerialNumber]     NVARCHAR (100)  NOT NULL,
    [PurchaseDate]     DATE            NOT NULL,
    [PurchaseCost]     DECIMAL (18, 2) NOT NULL,
    [CurrentBookValue] DECIMAL (18, 2) NOT NULL,
    [Status]           NVARCHAR (50)   DEFAULT ('Available') NOT NULL,
    [CreatedBy]        BIGINT          NOT NULL,
    [CreatedDate]      DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]       BIGINT          NULL,
    [ModifiedDate]     DATETIME2 (7)   NULL,
    [DeletedBy]        BIGINT          NULL,
    [DeletedDate]      DATETIME2 (7)   NULL,
    [IsDeleted]        BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]       ROWVERSION      NOT NULL,
    CONSTRAINT [PK_AssetMaster] PRIMARY KEY CLUSTERED ([AssetID] ASC),
    CONSTRAINT [FK_AssetMaster_Category] FOREIGN KEY ([AssetCategoryID]) REFERENCES [asset].[AssetCategory] ([AssetCategoryID]),
    CONSTRAINT [FK_AssetMaster_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Asset_Status]
    ON [asset].[AssetMaster]([TenantID] ASC, [Status] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Asset_SerialNumber]
    ON [asset].[AssetMaster]([TenantID] ASC, [SerialNumber] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Asset_Tag]
    ON [asset].[AssetMaster]([TenantID] ASC, [AssetTag] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Asset_Code]
    ON [asset].[AssetMaster]([TenantID] ASC, [AssetCode] ASC) WHERE ([IsDeleted]=(0));

