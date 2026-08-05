CREATE TABLE [offboarding].[ClearanceTask] (
    [ClearanceTaskID]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT        NOT NULL,
    [ClearanceRequestID] BIGINT        NOT NULL,
    [DepartmentID]       BIGINT        NOT NULL,
    [AssignedTo]         BIGINT        NOT NULL,
    [Status]             NVARCHAR (50) DEFAULT ('Pending') NOT NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [CreatedDate]        DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT        NULL,
    [ModifiedDate]       DATETIME2 (7) NULL,
    [DeletedBy]          BIGINT        NULL,
    [DeletedDate]        DATETIME2 (7) NULL,
    [IsDeleted]          BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION    NOT NULL,
    CONSTRAINT [PK_ClearanceTask] PRIMARY KEY CLUSTERED ([ClearanceTaskID] ASC),
    CONSTRAINT [FK_ClearanceTask_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [organization].[Department] ([DepartmentID]),
    CONSTRAINT [FK_ClearanceTask_Request] FOREIGN KEY ([ClearanceRequestID]) REFERENCES [offboarding].[ClearanceRequest] ([ClearanceRequestID]),
    CONSTRAINT [FK_ClearanceTask_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

