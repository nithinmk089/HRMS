
CREATE PROCEDURE attendance.usp_AttendanceRegularization_Reject
    @AttendanceRegularizationID BIGINT,
    @TenantID BIGINT,
    @ApproverID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE attendance.AttendanceRegularization
        SET [Status] = 'Rejected',
            ModifiedBy = @ApproverID,
            ModifiedDate = GETUTCDATE()
        WHERE AttendanceRegularizationID = @AttendanceRegularizationID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_AttendanceRegularization_Reject', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;