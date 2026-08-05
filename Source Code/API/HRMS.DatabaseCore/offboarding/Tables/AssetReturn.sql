CREATE TABLE [offboarding].[AssetReturn] (
    [AssetReturnID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]        BIGINT         NOT NULL,
    [EmployeeID]      BIGINT         NOT NULL,
    [AssetID]         BIGINT         NOT NULL,
    [ReturnDate]      DATE           NULL,
    [ReturnCondition] NVARCHAR (100) NULL,
    [CreatedBy]       BIGINT         NOT NULL,
    [CreatedDate]     DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]      BIGINT         NULL,
    [ModifiedDate]    DATETIME2 (7)  NULL,
    [DeletedBy]       BIGINT         NULL,
    [DeletedDate]     DATETIME2 (7)  NULL,
    [IsDeleted]       BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]      ROWVERSION     NOT NULL,
    CONSTRAINT [PK_AssetReturn] PRIMARY KEY CLUSTERED ([AssetReturnID] ASC),
    CONSTRAINT [FK_AssetReturn_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_AssetReturn_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

