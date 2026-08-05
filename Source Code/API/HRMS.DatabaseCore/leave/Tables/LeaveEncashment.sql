CREATE TABLE [leave].[LeaveEncashment] (
    [LeaveEncashmentID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]          BIGINT          NOT NULL,
    [EmployeeID]        BIGINT          NOT NULL,
    [LeaveTypeID]       BIGINT          NOT NULL,
    [EncashedDays]      DECIMAL (5, 2)  NOT NULL,
    [Amount]            DECIMAL (18, 2) NOT NULL,
    [Status]            NVARCHAR (50)   DEFAULT ('Pending') NOT NULL,
    [CreatedBy]         BIGINT          NOT NULL,
    [CreatedDate]       DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        BIGINT          NULL,
    [ModifiedDate]      DATETIME2 (7)   NULL,
    [DeletedBy]         BIGINT          NULL,
    [DeletedDate]       DATETIME2 (7)   NULL,
    [IsDeleted]         BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]        ROWVERSION      NOT NULL,
    CONSTRAINT [PK_LeaveEncashment] PRIMARY KEY CLUSTERED ([LeaveEncashmentID] ASC),
    CONSTRAINT [FK_LeaveEncashment_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_LeaveEncashment_LeaveType] FOREIGN KEY ([LeaveTypeID]) REFERENCES [leave].[LeaveType] ([LeaveTypeID]),
    CONSTRAINT [FK_LeaveEncashment_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

