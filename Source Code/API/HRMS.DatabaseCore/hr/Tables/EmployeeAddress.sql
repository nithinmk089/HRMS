CREATE TABLE [hr].[EmployeeAddress] (
    [EmployeeAddressID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]          BIGINT         NOT NULL,
    [EmployeeID]        BIGINT         NOT NULL,
    [AddressType]       NVARCHAR (50)  NOT NULL,
    [AddressLine1]      NVARCHAR (250) NOT NULL,
    [AddressLine2]      NVARCHAR (250) NULL,
    [City]              NVARCHAR (100) NOT NULL,
    [State]             NVARCHAR (100) NULL,
    [Country]           NVARCHAR (100) NOT NULL,
    [ZipCode]           NVARCHAR (20)  NULL,
    [CreatedBy]         BIGINT         NOT NULL,
    [CreatedDate]       DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        BIGINT         NULL,
    [ModifiedDate]      DATETIME2 (7)  NULL,
    [DeletedBy]         BIGINT         NULL,
    [DeletedDate]       DATETIME2 (7)  NULL,
    [IsDeleted]         BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL,
    CONSTRAINT [PK_EmployeeAddress] PRIMARY KEY CLUSTERED ([EmployeeAddressID] ASC),
    CONSTRAINT [FK_EmployeeAddress_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeAddress_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Address_Employee]
    ON [hr].[EmployeeAddress]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));

