CREATE TABLE [asset].[AssetReturn] (
    [AssetReturnID]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]          BIGINT         NOT NULL,
    [AssetAssignmentID] BIGINT         NOT NULL,
    [ReturnDate]        DATE           NOT NULL,
    [ReturnCondition]   NVARCHAR (500) NULL,
    [ReturnStatus]      NVARCHAR (50)  DEFAULT ('Pending') NOT NULL,
    [CreatedBy]         BIGINT         NOT NULL,
    [CreatedDate]       DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        BIGINT         NULL,
    [ModifiedDate]      DATETIME2 (7)  NULL,
    [DeletedBy]         BIGINT         NULL,
    [DeletedDate]       DATETIME2 (7)  NULL,
    [IsDeleted]         BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL,
    CONSTRAINT [PK_AssetReturn] PRIMARY KEY CLUSTERED ([AssetReturnID] ASC),
    CONSTRAINT [FK_AssetReturn_Assignment] FOREIGN KEY ([AssetAssignmentID]) REFERENCES [asset].[AssetAssignment] ([AssetAssignmentID]),
    CONSTRAINT [FK_AssetReturn_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_AssetReturn_Assignment]
    ON [asset].[AssetReturn]([AssetAssignmentID] ASC) WHERE ([IsDeleted]=(0));

