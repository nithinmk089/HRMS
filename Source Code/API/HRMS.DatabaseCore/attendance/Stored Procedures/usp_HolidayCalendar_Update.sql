
CREATE PROCEDURE attendance.usp_HolidayCalendar_Update
    @HolidayCalendarID BIGINT,
    @TenantID BIGINT,
    @CalendarCode NVARCHAR(50),
    @CalendarName NVARCHAR(100),
    @CountryCode NVARCHAR(10),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE attendance.HolidayCalendar
        SET CalendarCode = @CalendarCode,
            CalendarName = @CalendarName,
            CountryCode = @CountryCode,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE HolidayCalendarID = @HolidayCalendarID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_HolidayCalendar_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;