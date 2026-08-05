CREATE TABLE [attendance].[OvertimeApproval] (
    [OvertimeApprovalID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT         NOT NULL,
    [OvertimeRequestID]  BIGINT         NOT NULL,
    [ApproverID]         BIGINT         NOT NULL,
    [ApprovalDate]       DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [Remarks]            NVARCHAR (500) NULL,
    [CreatedBy]          BIGINT         NOT NULL,
    [CreatedDate]        DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT         NULL,
    [ModifiedDate]       DATETIME2 (7)  NULL,
    [DeletedBy]          BIGINT         NULL,
    [DeletedDate]        DATETIME2 (7)  NULL,
    [IsDeleted]          BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION     NOT NULL,
    CONSTRAINT [PK_OvertimeApproval] PRIMARY KEY CLUSTERED ([OvertimeApprovalID] ASC),
    CONSTRAINT [FK_OvertimeApproval_Approver] FOREIGN KEY ([ApproverID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_OvertimeApproval_Request] FOREIGN KEY ([OvertimeRequestID]) REFERENCES [attendance].[OvertimeRequest] ([OvertimeRequestID]),
    CONSTRAINT [FK_OvertimeApproval_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

