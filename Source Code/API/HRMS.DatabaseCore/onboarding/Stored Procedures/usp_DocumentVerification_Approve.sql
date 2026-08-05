
CREATE PROCEDURE onboarding.usp_DocumentVerification_Approve
    @EmployeeDocumentSubmissionID BIGINT,
    @TenantID                     BIGINT,
    @VerificationRemarks          NVARCHAR(500),
    @VerifiedBy                   BIGINT,
    @EmployeeDocumentVerificationID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO onboarding.EmployeeDocumentVerification (
            TenantID, EmployeeDocumentSubmissionID, VerificationStatus, VerificationRemarks, VerifiedBy, VerifiedDate, CreatedBy
        )
        VALUES (
            @TenantID, @EmployeeDocumentSubmissionID, 'Approved', @VerificationRemarks, @VerifiedBy, GETUTCDATE(), @VerifiedBy
        );

        SET @EmployeeDocumentVerificationID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END