
CREATE PROCEDURE attendance.usp_Holiday_Update
    @HolidayID BIGINT,
    @TenantID BIGINT,
    @HolidayCalendarID BIGINT,
    @HolidayDate DATE,
    @HolidayName NVARCHAR(100),
    @HolidayType NVARCHAR(50),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE attendance.Holiday
        SET HolidayCalendarID = @HolidayCalendarID,
            HolidayDate = @HolidayDate,
            HolidayName = @HolidayName,
            HolidayType = @HolidayType,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE HolidayID = @HolidayID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_Holiday_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;