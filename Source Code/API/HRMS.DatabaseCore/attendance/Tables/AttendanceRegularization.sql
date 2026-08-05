CREATE TABLE [attendance].[AttendanceRegularization] (
    [AttendanceRegularizationID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]                   BIGINT         NOT NULL,
    [EmployeeID]                 BIGINT         NOT NULL,
    [RequestedDate]              DATE           NOT NULL,
    [Reason]                     NVARCHAR (500) NOT NULL,
    [Status]                     NVARCHAR (50)  DEFAULT ('Pending') NOT NULL,
    [CreatedBy]                  BIGINT         NOT NULL,
    [CreatedDate]                DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]                 BIGINT         NULL,
    [ModifiedDate]               DATETIME2 (7)  NULL,
    [DeletedBy]                  BIGINT         NULL,
    [DeletedDate]                DATETIME2 (7)  NULL,
    [IsDeleted]                  BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]                 ROWVERSION     NOT NULL,
    CONSTRAINT [PK_AttendanceRegularization] PRIMARY KEY CLUSTERED ([AttendanceRegularizationID] ASC),
    CONSTRAINT [FK_AttendanceRegularization_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_AttendanceRegularization_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_AttendanceRegularization_Employee]
    ON [attendance].[AttendanceRegularization]([TenantID] ASC, [EmployeeID] ASC) WHERE ([IsDeleted]=(0));

