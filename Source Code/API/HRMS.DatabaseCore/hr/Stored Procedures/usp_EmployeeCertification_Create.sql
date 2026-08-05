
CREATE PROCEDURE hr.usp_EmployeeCertification_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @CertificationName NVARCHAR(200),
    @CertificationAuthority NVARCHAR(200),
    @IssueDate DATE,
    @ExpiryDate DATE = NULL,
    @CertificateNumber NVARCHAR(100) = NULL,
    @CreatedBy BIGINT,
    @EmployeeCertificationID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO hr.EmployeeCertification (
            TenantID, EmployeeID, CertificationName, CertificationAuthority, IssueDate, ExpiryDate, CertificateNumber, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @CertificationName, @CertificationAuthority, @IssueDate, @ExpiryDate, @CertificateNumber, @CreatedBy, GETUTCDATE(), 0
        );

        SET @EmployeeCertificationID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeCertification_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;