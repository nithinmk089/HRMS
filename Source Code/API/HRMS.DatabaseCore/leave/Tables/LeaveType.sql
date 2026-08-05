CREATE TABLE [leave].[LeaveType] (
    [LeaveTypeID]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]       BIGINT         NOT NULL,
    [LeaveCode]      NVARCHAR (50)  NOT NULL,
    [LeaveName]      NVARCHAR (100) NOT NULL,
    [IsPaid]         BIT            DEFAULT ((1)) NOT NULL,
    [IsAccrualBased] BIT            DEFAULT ((1)) NOT NULL,
    [CreatedBy]      BIGINT         NOT NULL,
    [CreatedDate]    DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]     BIGINT         NULL,
    [ModifiedDate]   DATETIME2 (7)  NULL,
    [DeletedBy]      BIGINT         NULL,
    [DeletedDate]    DATETIME2 (7)  NULL,
    [IsDeleted]      BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]     ROWVERSION     NOT NULL,
    CONSTRAINT [PK_LeaveType] PRIMARY KEY CLUSTERED ([LeaveTypeID] ASC),
    CONSTRAINT [FK_LeaveType_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_LeaveType_LeaveCode]
    ON [leave].[LeaveType]([TenantID] ASC, [LeaveCode] ASC) WHERE ([IsDeleted]=(0));

