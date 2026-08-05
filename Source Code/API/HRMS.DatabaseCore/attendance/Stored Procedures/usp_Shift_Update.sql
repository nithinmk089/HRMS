
CREATE PROCEDURE attendance.usp_Shift_Update
    @ShiftID BIGINT,
    @TenantID BIGINT,
    @ShiftCode NVARCHAR(50),
    @ShiftName NVARCHAR(100),
    @ShiftType NVARCHAR(50),
    @StartTime TIME,
    @EndTime TIME,
    @GraceInMinutes INT,
    @GraceOutMinutes INT,
    @IsFlexible BIT,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE attendance.Shift
        SET ShiftCode = @ShiftCode,
            ShiftName = @ShiftName,
            ShiftType = @ShiftType,
            StartTime = @StartTime,
            EndTime = @EndTime,
            GraceInMinutes = @GraceInMinutes,
            GraceOutMinutes = @GraceOutMinutes,
            IsFlexible = @IsFlexible,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE ShiftID = @ShiftID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_Shift_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;