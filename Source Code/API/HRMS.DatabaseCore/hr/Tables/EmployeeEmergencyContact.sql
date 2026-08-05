CREATE TABLE [hr].[EmployeeEmergencyContact] (
    [EmployeeEmergencyContactID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]                   BIGINT         NOT NULL,
    [EmployeeID]                 BIGINT         NOT NULL,
    [ContactName]                NVARCHAR (200) NOT NULL,
    [Relationship]               NVARCHAR (50)  NOT NULL,
    [MobileNumber]               NVARCHAR (50)  NOT NULL,
    [Email]                      NVARCHAR (200) NULL,
    [CreatedBy]                  BIGINT         NOT NULL,
    [CreatedDate]                DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]                 BIGINT         NULL,
    [ModifiedDate]               DATETIME2 (7)  NULL,
    [DeletedBy]                  BIGINT         NULL,
    [DeletedDate]                DATETIME2 (7)  NULL,
    [IsDeleted]                  BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]                 ROWVERSION     NOT NULL,
    CONSTRAINT [PK_EmployeeEmergencyContact] PRIMARY KEY CLUSTERED ([EmployeeEmergencyContactID] ASC),
    CONSTRAINT [FK_EmployeeEmergencyContact_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeEmergencyContact_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_EmergencyContact_Employee]
    ON [hr].[EmployeeEmergencyContact]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));

