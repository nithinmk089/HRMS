CREATE TABLE [learning].[CertificationRenewal] (
    [CertificationRenewalID]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]                BIGINT        NOT NULL,
    [LearningCertificationID] BIGINT        NOT NULL,
    [EmployeeID]              BIGINT        NOT NULL,
    [RenewalDate]             DATETIME2 (7) NOT NULL,
    [ExpiryDate]              DATETIME2 (7) NOT NULL,
    [CreatedBy]               BIGINT        NOT NULL,
    [CreatedDate]             DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]              BIGINT        NULL,
    [ModifiedDate]            DATETIME2 (7) NULL,
    [DeletedBy]               BIGINT        NULL,
    [DeletedDate]             DATETIME2 (7) NULL,
    [IsDeleted]               BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]              ROWVERSION    NOT NULL,
    CONSTRAINT [PK_CertificationRenewal] PRIMARY KEY CLUSTERED ([CertificationRenewalID] ASC),
    CONSTRAINT [FK_CertificationRenewal_Cert] FOREIGN KEY ([LearningCertificationID]) REFERENCES [learning].[LearningCertification] ([LearningCertificationID]),
    CONSTRAINT [FK_CertificationRenewal_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_CertificationRenewal_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

