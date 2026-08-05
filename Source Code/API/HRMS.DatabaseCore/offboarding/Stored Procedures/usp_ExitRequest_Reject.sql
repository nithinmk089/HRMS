
CREATE PROCEDURE offboarding.usp_ExitRequest_Reject
    @ExitRequestID BIGINT,
    @TenantID      BIGINT,
    @ApproverID    BIGINT,
    @Remarks       NVARCHAR(500),
    @ExitApprovalID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE offboarding.ExitRequest
        SET Status = 'Rejected',
            ModifiedBy = @ApproverID,
            ModifiedDate = GETUTCDATE()
        WHERE ExitRequestID = @ExitRequestID AND TenantID = @TenantID AND IsDeleted = 0;

        INSERT INTO offboarding.ExitApproval (
            TenantID, ExitRequestID, ApproverID, ApprovalStatus, ApprovalDate, Remarks, CreatedBy
        )
        VALUES (
            @TenantID, @ExitRequestID, @ApproverID, 'Rejected', GETUTCDATE(), @Remarks, @ApproverID
        );

        SET @ExitApprovalID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END