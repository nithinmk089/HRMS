CREATE TABLE [performance].[CheckInMeeting] (
    [CheckInMeetingID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]         BIGINT          NOT NULL,
    [EmployeeID]       BIGINT          NOT NULL,
    [ManagerID]        BIGINT          NOT NULL,
    [MeetingDate]      DATETIME2 (7)   NOT NULL,
    [Notes]            NVARCHAR (2000) NULL,
    [CreatedBy]        BIGINT          NOT NULL,
    [CreatedDate]      DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]       BIGINT          NULL,
    [ModifiedDate]     DATETIME2 (7)   NULL,
    [DeletedBy]        BIGINT          NULL,
    [DeletedDate]      DATETIME2 (7)   NULL,
    [IsDeleted]        BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]       ROWVERSION      NOT NULL,
    CONSTRAINT [PK_CheckInMeeting] PRIMARY KEY CLUSTERED ([CheckInMeetingID] ASC),
    CONSTRAINT [FK_CheckInMeeting_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_CheckInMeeting_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

