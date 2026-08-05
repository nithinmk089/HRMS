CREATE TABLE [leave].[LeaveAccrualRule] (
    [LeaveAccrualRuleID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT         NOT NULL,
    [LeavePolicyID]      BIGINT         NOT NULL,
    [AccrualFrequency]   NVARCHAR (50)  NOT NULL,
    [AccrualAmount]      DECIMAL (5, 2) NOT NULL,
    [CreatedBy]          BIGINT         NOT NULL,
    [CreatedDate]        DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT         NULL,
    [ModifiedDate]       DATETIME2 (7)  NULL,
    [DeletedBy]          BIGINT         NULL,
    [DeletedDate]        DATETIME2 (7)  NULL,
    [IsDeleted]          BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION     NOT NULL,
    CONSTRAINT [PK_LeaveAccrualRule] PRIMARY KEY CLUSTERED ([LeaveAccrualRuleID] ASC),
    CONSTRAINT [FK_LeaveAccrualRule_Policy] FOREIGN KEY ([LeavePolicyID]) REFERENCES [leave].[LeavePolicy] ([LeavePolicyID]),
    CONSTRAINT [FK_LeaveAccrualRule_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

