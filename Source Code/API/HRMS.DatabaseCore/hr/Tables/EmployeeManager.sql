CREATE TABLE [hr].[EmployeeManager] (
    [EmployeeManagerID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]          BIGINT        NOT NULL,
    [EmployeeID]        BIGINT        NOT NULL,
    [ManagerID]         BIGINT        NOT NULL,
    [EffectiveFrom]     DATE          NOT NULL,
    [EffectiveTo]       DATE          NULL,
    [CreatedBy]         BIGINT        NOT NULL,
    [CreatedDate]       DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        BIGINT        NULL,
    [ModifiedDate]      DATETIME2 (7) NULL,
    [DeletedBy]         BIGINT        NULL,
    [DeletedDate]       DATETIME2 (7) NULL,
    [IsDeleted]         BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]        ROWVERSION    NOT NULL,
    CONSTRAINT [PK_EmployeeManager] PRIMARY KEY CLUSTERED ([EmployeeManagerID] ASC),
    CONSTRAINT [FK_EmployeeManager_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeManager_Manager] FOREIGN KEY ([ManagerID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeManager_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Manager_Manager]
    ON [hr].[EmployeeManager]([ManagerID] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Manager_Employee]
    ON [hr].[EmployeeManager]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));

