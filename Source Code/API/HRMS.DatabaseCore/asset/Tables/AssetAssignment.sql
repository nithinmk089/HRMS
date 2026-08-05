CREATE TABLE [asset].[AssetAssignment] (
    [AssetAssignmentID]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT        NOT NULL,
    [AssetID]            BIGINT        NOT NULL,
    [EmployeeID]         BIGINT        NOT NULL,
    [AssignedDate]       DATE          NOT NULL,
    [ExpectedReturnDate] DATE          NULL,
    [ReturnedDate]       DATE          NULL,
    [AssignmentStatus]   NVARCHAR (50) DEFAULT ('Active') NOT NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [CreatedDate]        DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT        NULL,
    [ModifiedDate]       DATETIME2 (7) NULL,
    [DeletedBy]          BIGINT        NULL,
    [DeletedDate]        DATETIME2 (7) NULL,
    [IsDeleted]          BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION    NOT NULL,
    CONSTRAINT [PK_AssetAssignment] PRIMARY KEY CLUSTERED ([AssetAssignmentID] ASC),
    CONSTRAINT [FK_AssetAssignment_Asset] FOREIGN KEY ([AssetID]) REFERENCES [asset].[AssetMaster] ([AssetID]),
    CONSTRAINT [FK_AssetAssignment_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_AssetAssignment_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_AssetAssignment_Asset]
    ON [asset].[AssetAssignment]([AssetID] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_AssetAssignment_Employee]
    ON [asset].[AssetAssignment]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));

