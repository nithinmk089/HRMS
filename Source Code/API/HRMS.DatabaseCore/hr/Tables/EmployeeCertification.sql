CREATE TABLE [hr].[EmployeeCertification] (
    [EmployeeCertificationID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]                BIGINT         NOT NULL,
    [EmployeeID]              BIGINT         NOT NULL,
    [CertificationName]       NVARCHAR (200) NOT NULL,
    [CertificationAuthority]  NVARCHAR (200) NOT NULL,
    [IssueDate]               DATE           NOT NULL,
    [ExpiryDate]              DATE           NULL,
    [CertificateNumber]       NVARCHAR (100) NULL,
    [CreatedBy]               BIGINT         NOT NULL,
    [CreatedDate]             DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]              BIGINT         NULL,
    [ModifiedDate]            DATETIME2 (7)  NULL,
    [DeletedBy]               BIGINT         NULL,
    [DeletedDate]             DATETIME2 (7)  NULL,
    [IsDeleted]               BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]              ROWVERSION     NOT NULL,
    CONSTRAINT [PK_EmployeeCertification] PRIMARY KEY CLUSTERED ([EmployeeCertificationID] ASC),
    CONSTRAINT [FK_EmployeeCertification_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeCertification_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Certification_Expiry]
    ON [hr].[EmployeeCertification]([ExpiryDate] ASC) WHERE ([IsDeleted]=(0) AND [ExpiryDate] IS NOT NULL);


GO
CREATE NONCLUSTERED INDEX [IX_Certification_Employee]
    ON [hr].[EmployeeCertification]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));

