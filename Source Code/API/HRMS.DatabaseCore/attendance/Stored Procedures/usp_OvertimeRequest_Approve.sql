
CREATE PROCEDURE attendance.usp_OvertimeRequest_Approve
    @OvertimeRequestID BIGINT,
    @TenantID BIGINT,
    @ApproverID BIGINT,
    @Remarks NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE attendance.OvertimeRequest
        SET [Status] = 'Approved',
            ModifiedBy = @ApproverID,
            ModifiedDate = GETUTCDATE()
        WHERE OvertimeRequestID = @OvertimeRequestID AND TenantID = @TenantID AND IsDeleted = 0;

        INSERT INTO attendance.OvertimeApproval (
            TenantID, OvertimeRequestID, ApproverID, ApprovalDate, Remarks, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @OvertimeRequestID, @ApproverID, GETUTCDATE(), @Remarks, @ApproverID, GETUTCDATE(), 0
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_OvertimeRequest_Approve', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;