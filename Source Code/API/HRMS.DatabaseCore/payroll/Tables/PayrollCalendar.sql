CREATE TABLE [payroll].[PayrollCalendar] (
    [PayrollCalendarID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]          BIGINT         NOT NULL,
    [CalendarCode]      NVARCHAR (50)  NOT NULL,
    [CalendarName]      NVARCHAR (100) NOT NULL,
    [FinancialYear]     NVARCHAR (20)  NOT NULL,
    [IsActive]          BIT            DEFAULT ((1)) NOT NULL,
    [CreatedBy]         BIGINT         NOT NULL,
    [CreatedDate]       DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        BIGINT         NULL,
    [ModifiedDate]      DATETIME2 (7)  NULL,
    [DeletedBy]         BIGINT         NULL,
    [DeletedDate]       DATETIME2 (7)  NULL,
    [IsDeleted]         BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL,
    CONSTRAINT [PK_PayrollCalendar] PRIMARY KEY CLUSTERED ([PayrollCalendarID] ASC),
    CONSTRAINT [FK_PayrollCalendar_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

