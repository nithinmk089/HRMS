CREATE TABLE [leave].[LeaveApproval] (
    [LeaveApprovalID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]        BIGINT         NOT NULL,
    [LeaveRequestID]  BIGINT         NOT NULL,
    [ApproverID]      BIGINT         NOT NULL,
    [ApprovalDate]    DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ApprovalRemarks] NVARCHAR (500) NULL,
    [CreatedBy]       BIGINT         NOT NULL,
    [CreatedDate]     DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]      BIGINT         NULL,
    [ModifiedDate]    DATETIME2 (7)  NULL,
    [DeletedBy]       BIGINT         NULL,
    [DeletedDate]     DATETIME2 (7)  NULL,
    [IsDeleted]       BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]      ROWVERSION     NOT NULL,
    CONSTRAINT [PK_LeaveApproval] PRIMARY KEY CLUSTERED ([LeaveApprovalID] ASC),
    CONSTRAINT [FK_LeaveApproval_Approver] FOREIGN KEY ([ApproverID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_LeaveApproval_Request] FOREIGN KEY ([LeaveRequestID]) REFERENCES [leave].[LeaveRequest] ([LeaveRequestID]),
    CONSTRAINT [FK_LeaveApproval_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

