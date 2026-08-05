
CREATE PROCEDURE attendance.usp_AttendanceAdjustment_Reject
    @AttendanceAdjustmentID BIGINT,
    @TenantID BIGINT,
    @ApproverID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE attendance.AttendanceAdjustment
        SET ApprovalStatus = 'Rejected',
            ModifiedBy = @ApproverID,
            ModifiedDate = GETUTCDATE()
        WHERE AttendanceAdjustmentID = @AttendanceAdjustmentID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_AttendanceAdjustment_Reject', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;