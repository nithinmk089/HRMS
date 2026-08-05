
CREATE PROCEDURE leave.usp_LeaveRequest_Reject
    @LeaveRequestID BIGINT,
    @TenantID BIGINT,
    @ApproverID BIGINT,
    @Remarks NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE leave.LeaveRequest
        SET [Status] = 'Rejected',
            ModifiedBy = @ApproverID,
            ModifiedDate = GETUTCDATE()
        WHERE LeaveRequestID = @LeaveRequestID AND TenantID = @TenantID AND IsDeleted = 0;

        INSERT INTO leave.LeaveApproval (
            TenantID, LeaveRequestID, ApproverID, ApprovalDate, ApprovalRemarks, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @LeaveRequestID, @ApproverID, GETUTCDATE(), @Remarks, @ApproverID, GETUTCDATE(), 0
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_LeaveRequest_Reject', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;