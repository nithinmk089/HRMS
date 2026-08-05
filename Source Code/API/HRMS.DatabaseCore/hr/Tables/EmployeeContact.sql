CREATE TABLE [hr].[EmployeeContact] (
    [EmployeeContactID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]          BIGINT         NOT NULL,
    [EmployeeID]        BIGINT         NOT NULL,
    [ContactType]       NVARCHAR (50)  NOT NULL,
    [ContactValue]      NVARCHAR (200) NOT NULL,
    [CreatedBy]         BIGINT         NOT NULL,
    [CreatedDate]       DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        BIGINT         NULL,
    [ModifiedDate]      DATETIME2 (7)  NULL,
    [DeletedBy]         BIGINT         NULL,
    [DeletedDate]       DATETIME2 (7)  NULL,
    [IsDeleted]         BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL,
    CONSTRAINT [PK_EmployeeContact] PRIMARY KEY CLUSTERED ([EmployeeContactID] ASC),
    CONSTRAINT [FK_EmployeeContact_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeContact_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Contact_Employee]
    ON [hr].[EmployeeContact]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));

