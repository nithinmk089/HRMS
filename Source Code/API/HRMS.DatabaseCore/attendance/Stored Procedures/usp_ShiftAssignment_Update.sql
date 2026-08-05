
CREATE PROCEDURE attendance.usp_ShiftAssignment_Update
    @ShiftAssignmentID BIGINT,
    @TenantID BIGINT,
    @ShiftID BIGINT,
    @EffectiveFrom DATE,
    @EffectiveTo DATE = NULL,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE attendance.ShiftAssignment
        SET ShiftID = @ShiftID,
            EffectiveFrom = @EffectiveFrom,
            EffectiveTo = @EffectiveTo,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE ShiftAssignmentID = @ShiftAssignmentID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_ShiftAssignment_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;