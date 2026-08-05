
CREATE PROCEDURE hr.usp_EmployeeCertification_Update
    @EmployeeCertificationID BIGINT,
    @TenantID BIGINT,
    @CertificationName NVARCHAR(200),
    @CertificationAuthority NVARCHAR(200),
    @IssueDate DATE,
    @ExpiryDate DATE = NULL,
    @CertificateNumber NVARCHAR(100) = NULL,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE hr.EmployeeCertification
        SET CertificationName = @CertificationName,
            CertificationAuthority = @CertificationAuthority,
            IssueDate = @IssueDate,
            ExpiryDate = @ExpiryDate,
            CertificateNumber = @CertificateNumber,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE EmployeeCertificationID = @EmployeeCertificationID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeCertification_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;