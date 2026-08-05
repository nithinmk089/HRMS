
CREATE PROCEDURE onboarding.usp_ESignature_Create
    @TenantID             BIGINT,
    @EmployeeID           BIGINT,
    @SignatureFilePath    NVARCHAR(500),
    @SignatureHash        NVARCHAR(256),
    @CreatedBy            BIGINT,
    @EmployeeESignatureID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO onboarding.EmployeeESignature (
            TenantID, EmployeeID, SignatureFilePath, SignatureHash, SignatureDate, CreatedBy
        )
        VALUES (
            @TenantID, @EmployeeID, @SignatureFilePath, @SignatureHash, GETUTCDATE(), @CreatedBy
        );

        SET @EmployeeESignatureID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END