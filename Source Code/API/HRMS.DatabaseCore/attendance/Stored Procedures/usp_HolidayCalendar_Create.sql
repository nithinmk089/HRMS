
CREATE PROCEDURE attendance.usp_HolidayCalendar_Create
    @TenantID BIGINT,
    @CalendarCode NVARCHAR(50),
    @CalendarName NVARCHAR(100),
    @CountryCode NVARCHAR(10),
    @CreatedBy BIGINT,
    @HolidayCalendarID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO attendance.HolidayCalendar (
            TenantID, CalendarCode, CalendarName, CountryCode, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @CalendarCode, @CalendarName, @CountryCode, @CreatedBy, GETUTCDATE(), 0
        );
        SET @HolidayCalendarID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_HolidayCalendar_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;