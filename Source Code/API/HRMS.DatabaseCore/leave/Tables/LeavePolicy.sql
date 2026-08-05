CREATE TABLE [leave].[LeavePolicy] (
    [LeavePolicyID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]      BIGINT         NOT NULL,
    [PolicyName]    NVARCHAR (100) NOT NULL,
    [LeaveTypeID]   BIGINT         NOT NULL,
    [EffectiveFrom] DATE           NOT NULL,
    [EffectiveTo]   DATE           NULL,
    [CreatedBy]     BIGINT         NOT NULL,
    [CreatedDate]   DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]    BIGINT         NULL,
    [ModifiedDate]  DATETIME2 (7)  NULL,
    [DeletedBy]     BIGINT         NULL,
    [DeletedDate]   DATETIME2 (7)  NULL,
    [IsDeleted]     BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]    ROWVERSION     NOT NULL,
    CONSTRAINT [PK_LeavePolicy] PRIMARY KEY CLUSTERED ([LeavePolicyID] ASC),
    CONSTRAINT [FK_LeavePolicy_LeaveType] FOREIGN KEY ([LeaveTypeID]) REFERENCES [leave].[LeaveType] ([LeaveTypeID]),
    CONSTRAINT [FK_LeavePolicy_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

