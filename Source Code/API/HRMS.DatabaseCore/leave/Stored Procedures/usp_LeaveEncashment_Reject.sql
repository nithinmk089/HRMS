
CREATE PROCEDURE leave.usp_LeaveEncashment_Reject
    @LeaveEncashmentID BIGINT,
    @TenantID BIGINT,
    @ApproverID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE leave.LeaveEncashment
        SET [Status] = 'Rejected',
            ModifiedBy = @ApproverID,
            ModifiedDate = GETUTCDATE()
        WHERE LeaveEncashmentID = @LeaveEncashmentID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_LeaveEncashment_Reject', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;