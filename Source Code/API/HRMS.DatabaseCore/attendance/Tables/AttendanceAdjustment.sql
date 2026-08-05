CREATE TABLE [attendance].[AttendanceAdjustment] (
    [AttendanceAdjustmentID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]               BIGINT         NOT NULL,
    [AttendanceID]           BIGINT         NOT NULL,
    [AdjustmentReason]       NVARCHAR (500) NOT NULL,
    [OriginalValue]          NVARCHAR (100) NULL,
    [NewValue]               NVARCHAR (100) NULL,
    [ApprovalStatus]         NVARCHAR (50)  DEFAULT ('Pending') NOT NULL,
    [CreatedBy]              BIGINT         NOT NULL,
    [CreatedDate]            DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]             BIGINT         NULL,
    [ModifiedDate]           DATETIME2 (7)  NULL,
    [DeletedBy]              BIGINT         NULL,
    [DeletedDate]            DATETIME2 (7)  NULL,
    [IsDeleted]              BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]             ROWVERSION     NOT NULL,
    CONSTRAINT [PK_AttendanceAdjustment] PRIMARY KEY CLUSTERED ([AttendanceAdjustmentID] ASC),
    CONSTRAINT [FK_AttendanceAdjustment_Attendance] FOREIGN KEY ([AttendanceID]) REFERENCES [attendance].[Attendance] ([AttendanceID]),
    CONSTRAINT [FK_AttendanceAdjustment_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_AttendanceAdjustment_Attendance]
    ON [attendance].[AttendanceAdjustment]([TenantID] ASC, [AttendanceID] ASC) WHERE ([IsDeleted]=(0));

