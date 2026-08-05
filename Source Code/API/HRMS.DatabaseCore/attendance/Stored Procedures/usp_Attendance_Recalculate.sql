
CREATE PROCEDURE attendance.usp_Attendance_Recalculate
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @AttendanceDate DATE,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        -- Fetch shift detail
        DECLARE @ShiftID BIGINT, @ClockIn DATETIME2, @ClockOut DATETIME2;
        SELECT @ShiftID = ShiftID, @ClockIn = ClockInTime, @ClockOut = ClockOutTime
        FROM attendance.Attendance
        WHERE TenantID = @TenantID AND EmployeeID = @EmployeeID AND AttendanceDate = @AttendanceDate AND IsDeleted = 0;

        IF @ShiftID IS NOT NULL
        BEGIN
            DECLARE @StartTime TIME, @EndTime TIME, @GraceIn INT, @GraceOut INT;
            SELECT @StartTime = StartTime, @EndTime = EndTime, @GraceIn = GraceInMinutes, @GraceOut = GraceOutMinutes
            FROM attendance.Shift WHERE ShiftID = @ShiftID;

            DECLARE @WorkingMin INT = attendance.fn_CalculateWorkingHours(@ClockIn, @ClockOut);
            DECLARE @Status NVARCHAR(50) = attendance.fn_CalculateAttendanceStatus(@ClockIn, @ClockOut, @StartTime, @EndTime, @GraceIn, @GraceOut);

            UPDATE attendance.Attendance
            SET WorkingMinutes = @WorkingMin,
                AttendanceStatus = @Status,
                ModifiedBy = @ModifiedBy,
                ModifiedDate = GETUTCDATE()
            WHERE TenantID = @TenantID AND EmployeeID = @EmployeeID AND AttendanceDate = @AttendanceDate AND IsDeleted = 0;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_Attendance_Recalculate', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;