CREATE TABLE [offboarding].[FullAndFinalSettlement] (
    [FullAndFinalSettlementID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]                 BIGINT          NOT NULL,
    [EmployeeID]               BIGINT          NOT NULL,
    [SettlementAmount]         DECIMAL (18, 2) DEFAULT ((0.00)) NOT NULL,
    [SettlementDate]           DATE            NULL,
    [SettlementStatus]         NVARCHAR (50)   DEFAULT ('Pending') NOT NULL,
    [CreatedBy]                BIGINT          NOT NULL,
    [CreatedDate]              DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]               BIGINT          NULL,
    [ModifiedDate]             DATETIME2 (7)   NULL,
    [DeletedBy]                BIGINT          NULL,
    [DeletedDate]              DATETIME2 (7)   NULL,
    [IsDeleted]                BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]               ROWVERSION      NOT NULL,
    CONSTRAINT [PK_FullAndFinalSettlement] PRIMARY KEY CLUSTERED ([FullAndFinalSettlementID] ASC),
    CONSTRAINT [FK_FullAndFinalSettlement_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_FullAndFinalSettlement_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

