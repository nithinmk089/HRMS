CREATE TABLE [onboarding].[EmployeeESignature] (
    [EmployeeESignatureID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]             BIGINT         NOT NULL,
    [EmployeeID]           BIGINT         NOT NULL,
    [SignatureFilePath]    NVARCHAR (500) NULL,
    [SignatureHash]        NVARCHAR (256) NOT NULL,
    [SignatureDate]        DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]            BIGINT         NOT NULL,
    [CreatedDate]          DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]           BIGINT         NULL,
    [ModifiedDate]         DATETIME2 (7)  NULL,
    [DeletedBy]            BIGINT         NULL,
    [DeletedDate]          DATETIME2 (7)  NULL,
    [IsDeleted]            BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]           ROWVERSION     NOT NULL,
    CONSTRAINT [PK_EmployeeESignature] PRIMARY KEY CLUSTERED ([EmployeeESignatureID] ASC),
    CONSTRAINT [FK_EmployeeESignature_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeESignature_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

