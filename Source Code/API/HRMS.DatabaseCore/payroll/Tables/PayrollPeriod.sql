CREATE TABLE [payroll].[PayrollPeriod] (
    [PayrollPeriodID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]          BIGINT        NOT NULL,
    [PayrollCalendarID] BIGINT        NOT NULL,
    [PeriodCode]        NVARCHAR (50) NOT NULL,
    [PeriodStartDate]   DATE          NOT NULL,
    [PeriodEndDate]     DATE          NOT NULL,
    [ProcessingStatus]  NVARCHAR (50) DEFAULT ('Open') NOT NULL,
    [CreatedBy]         BIGINT        NOT NULL,
    [CreatedDate]       DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        BIGINT        NULL,
    [ModifiedDate]      DATETIME2 (7) NULL,
    [DeletedBy]         BIGINT        NULL,
    [DeletedDate]       DATETIME2 (7) NULL,
    [IsDeleted]         BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]        ROWVERSION    NOT NULL,
    CONSTRAINT [PK_PayrollPeriod] PRIMARY KEY CLUSTERED ([PayrollPeriodID] ASC),
    CONSTRAINT [FK_PayrollPeriod_Calendar] FOREIGN KEY ([PayrollCalendarID]) REFERENCES [payroll].[PayrollCalendar] ([PayrollCalendarID]),
    CONSTRAINT [FK_PayrollPeriod_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_PayrollPeriod_Calendar]
    ON [payroll].[PayrollPeriod]([PayrollCalendarID] ASC) WHERE ([IsDeleted]=(0));

