CREATE TABLE [onboarding].[PolicyAcceptance] (
    [PolicyAcceptanceID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT        NOT NULL,
    [EmployeeID]         BIGINT        NOT NULL,
    [PolicyID]           BIGINT        NOT NULL,
    [AcceptedDate]       DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [AcceptanceVersion]  NVARCHAR (50) NOT NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [CreatedDate]        DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT        NULL,
    [ModifiedDate]       DATETIME2 (7) NULL,
    [DeletedBy]          BIGINT        NULL,
    [DeletedDate]        DATETIME2 (7) NULL,
    [IsDeleted]          BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION    NOT NULL,
    CONSTRAINT [PK_PolicyAcceptance] PRIMARY KEY CLUSTERED ([PolicyAcceptanceID] ASC),
    CONSTRAINT [FK_PolicyAcceptance_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_PolicyAcceptance_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

