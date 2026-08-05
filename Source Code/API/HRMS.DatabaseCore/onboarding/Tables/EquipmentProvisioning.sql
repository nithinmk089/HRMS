CREATE TABLE [onboarding].[EquipmentProvisioning] (
    [EquipmentProvisioningID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]                BIGINT        NOT NULL,
    [EmployeeID]              BIGINT        NOT NULL,
    [AssetID]                 BIGINT        NOT NULL,
    [ProvisionDate]           DATE          NOT NULL,
    [ReturnRequired]          BIT           DEFAULT ((1)) NOT NULL,
    [CreatedBy]               BIGINT        NOT NULL,
    [CreatedDate]             DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]              BIGINT        NULL,
    [ModifiedDate]            DATETIME2 (7) NULL,
    [DeletedBy]               BIGINT        NULL,
    [DeletedDate]             DATETIME2 (7) NULL,
    [IsDeleted]               BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]              ROWVERSION    NOT NULL,
    CONSTRAINT [PK_EquipmentProvisioning] PRIMARY KEY CLUSTERED ([EquipmentProvisioningID] ASC),
    CONSTRAINT [FK_EquipmentProvisioning_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EquipmentProvisioning_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

