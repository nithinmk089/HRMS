
CREATE PROCEDURE attendance.usp_Attendance_Update
    @AttendanceID BIGINT,
    @TenantID BIGINT,
    @ClockOutTime DATETIME2,
    @WorkingMinutes INT,
    @AttendanceStatus NVARCHAR(50),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE attendance.Attendance
        SET ClockOutTime = @ClockOutTime,
            WorkingMinutes = @WorkingMinutes,
            AttendanceStatus = @AttendanceStatus,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE AttendanceID = @AttendanceID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_Attendance_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;