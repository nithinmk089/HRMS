CREATE TABLE [attendance].[HolidayCalendar] (
    [HolidayCalendarID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]          BIGINT         NOT NULL,
    [CalendarCode]      NVARCHAR (50)  NOT NULL,
    [CalendarName]      NVARCHAR (100) NOT NULL,
    [CountryCode]       NVARCHAR (10)  NOT NULL,
    [CreatedBy]         BIGINT         NOT NULL,
    [CreatedDate]       DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        BIGINT         NULL,
    [ModifiedDate]      DATETIME2 (7)  NULL,
    [DeletedBy]         BIGINT         NULL,
    [DeletedDate]       DATETIME2 (7)  NULL,
    [IsDeleted]         BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL,
    CONSTRAINT [PK_HolidayCalendar] PRIMARY KEY CLUSTERED ([HolidayCalendarID] ASC),
    CONSTRAINT [FK_HolidayCalendar_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_HolidayCalendar_CalendarCode]
    ON [attendance].[HolidayCalendar]([TenantID] ASC, [CalendarCode] ASC) WHERE ([IsDeleted]=(0));

