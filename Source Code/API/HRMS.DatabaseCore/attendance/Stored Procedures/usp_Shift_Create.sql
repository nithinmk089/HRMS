
CREATE PROCEDURE attendance.usp_Shift_Create
    @TenantID BIGINT,
    @ShiftCode NVARCHAR(50),
    @ShiftName NVARCHAR(100),
    @ShiftType NVARCHAR(50),
    @StartTime TIME,
    @EndTime TIME,
    @GraceInMinutes INT = 0,
    @GraceOutMinutes INT = 0,
    @IsFlexible BIT = 0,
    @CreatedBy BIGINT,
    @ShiftID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO attendance.Shift (
            TenantID, ShiftCode, ShiftName, ShiftType, StartTime, EndTime, 
            GraceInMinutes, GraceOutMinutes, IsFlexible, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @ShiftCode, @ShiftName, @ShiftType, @StartTime, @EndTime, 
            @GraceInMinutes, @GraceOutMinutes, @IsFlexible, @CreatedBy, GETUTCDATE(), 0
        );
        SET @ShiftID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_Shift_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;