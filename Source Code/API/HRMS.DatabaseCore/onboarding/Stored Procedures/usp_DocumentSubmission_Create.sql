
CREATE PROCEDURE onboarding.usp_DocumentSubmission_Create
    @TenantID                     BIGINT,
    @EmployeeID                   BIGINT,
    @DocumentType                 NVARCHAR(100),
    @FileName                     NVARCHAR(255),
    @FilePath                     NVARCHAR(500),
    @MimeType                     NVARCHAR(100),
    @CreatedBy                    BIGINT,
    @EmployeeDocumentSubmissionID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO onboarding.EmployeeDocumentSubmission (
            TenantID, EmployeeID, DocumentType, FileName, FilePath, MimeType, UploadedDate, CreatedBy
        )
        VALUES (
            @TenantID, @EmployeeID, @DocumentType, @FileName, @FilePath, @MimeType, GETUTCDATE(), @CreatedBy
        );

        SET @EmployeeDocumentSubmissionID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END