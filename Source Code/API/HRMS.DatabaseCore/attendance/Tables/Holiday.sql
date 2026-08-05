CREATE TABLE [attendance].[Holiday] (
    [HolidayID]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]          BIGINT         NOT NULL,
    [HolidayCalendarID] BIGINT         NOT NULL,
    [HolidayDate]       DATE           NOT NULL,
    [HolidayName]       NVARCHAR (100) NOT NULL,
    [HolidayType]       NVARCHAR (50)  NOT NULL,
    [CreatedBy]         BIGINT         NOT NULL,
    [CreatedDate]       DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        BIGINT         NULL,
    [ModifiedDate]      DATETIME2 (7)  NULL,
    [DeletedBy]         BIGINT         NULL,
    [DeletedDate]       DATETIME2 (7)  NULL,
    [IsDeleted]         BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Holiday] PRIMARY KEY CLUSTERED ([HolidayID] ASC),
    CONSTRAINT [FK_Holiday_Calendar] FOREIGN KEY ([HolidayCalendarID]) REFERENCES [attendance].[HolidayCalendar] ([HolidayCalendarID]),
    CONSTRAINT [FK_Holiday_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Holiday_Date]
    ON [attendance].[Holiday]([TenantID] ASC, [HolidayDate] ASC) WHERE ([IsDeleted]=(0));

