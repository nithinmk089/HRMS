CREATE TABLE [payroll].[ArrearProcessing] (
    [ArrearProcessingID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT          NOT NULL,
    [EmployeeID]         BIGINT          NOT NULL,
    [ArrearAmount]       DECIMAL (18, 2) NOT NULL,
    [EffectiveMonth]     DATE            NOT NULL,
    [Status]             NVARCHAR (50)   DEFAULT ('Pending') NOT NULL,
    [CreatedBy]          BIGINT          NOT NULL,
    [CreatedDate]        DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT          NULL,
    [ModifiedDate]       DATETIME2 (7)   NULL,
    [DeletedBy]          BIGINT          NULL,
    [DeletedDate]        DATETIME2 (7)   NULL,
    [IsDeleted]          BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION      NOT NULL,
    CONSTRAINT [PK_ArrearProcessing] PRIMARY KEY CLUSTERED ([ArrearProcessingID] ASC),
    CONSTRAINT [FK_ArrearProcessing_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_ArrearProcessing_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

