CREATE TABLE [learning].[ComplianceAcknowledgement] (
    [ComplianceAcknowledgementID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]                    BIGINT        NOT NULL,
    [EmployeeID]                  BIGINT        NOT NULL,
    [ComplianceTrainingID]        BIGINT        NOT NULL,
    [AcknowledgedDate]            DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]                   BIGINT        NOT NULL,
    [CreatedDate]                 DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]                  BIGINT        NULL,
    [ModifiedDate]                DATETIME2 (7) NULL,
    [DeletedBy]                   BIGINT        NULL,
    [DeletedDate]                 DATETIME2 (7) NULL,
    [IsDeleted]                   BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]                  ROWVERSION    NOT NULL,
    CONSTRAINT [PK_ComplianceAcknowledgement] PRIMARY KEY CLUSTERED ([ComplianceAcknowledgementID] ASC),
    CONSTRAINT [FK_ComplianceAck_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_ComplianceAck_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID]),
    CONSTRAINT [FK_ComplianceAck_Training] FOREIGN KEY ([ComplianceTrainingID]) REFERENCES [learning].[ComplianceTraining] ([ComplianceTrainingID])
);

