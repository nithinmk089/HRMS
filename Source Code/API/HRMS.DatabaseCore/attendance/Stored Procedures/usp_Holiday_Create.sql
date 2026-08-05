
CREATE PROCEDURE attendance.usp_Holiday_Create
    @TenantID BIGINT,
    @HolidayCalendarID BIGINT,
    @HolidayDate DATE,
    @HolidayName NVARCHAR(100),
    @HolidayType NVARCHAR(50),
    @CreatedBy BIGINT,
    @HolidayID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO attendance.Holiday (
            TenantID, HolidayCalendarID, HolidayDate, HolidayName, HolidayType, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @HolidayCalendarID, @HolidayDate, @HolidayName, @HolidayType, @CreatedBy, GETUTCDATE(), 0
        );
        SET @HolidayID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_Holiday_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;