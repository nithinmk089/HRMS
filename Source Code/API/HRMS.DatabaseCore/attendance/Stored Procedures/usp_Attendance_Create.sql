
CREATE PROCEDURE attendance.usp_Attendance_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @ShiftID BIGINT,
    @AttendanceDate DATE,
    @ClockInTime DATETIME2 = NULL,
    @AttendanceStatus NVARCHAR(50),
    @CreatedBy BIGINT,
    @AttendanceID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO attendance.Attendance (
            TenantID, EmployeeID, ShiftID, AttendanceDate, ClockInTime, 
            AttendanceStatus, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @ShiftID, @AttendanceDate, @ClockInTime, 
            @AttendanceStatus, @CreatedBy, GETUTCDATE(), 0
        );
        SET @AttendanceID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_Attendance_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;