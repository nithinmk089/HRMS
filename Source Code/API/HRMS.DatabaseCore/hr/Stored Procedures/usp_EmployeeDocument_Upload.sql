
CREATE PROCEDURE hr.usp_EmployeeDocument_Upload
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @DocumentType NVARCHAR(50),
    @FileName NVARCHAR(250),
    @FilePath NVARCHAR(500),
    @MimeType NVARCHAR(100),
    @CreatedBy BIGINT,
    @EmployeeDocumentID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO hr.EmployeeDocument (
            TenantID, EmployeeID, DocumentType, [FileName], FilePath, MimeType, VersionNumber, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @DocumentType, @FileName, @FilePath, @MimeType, 1, @CreatedBy, GETUTCDATE(), 0
        );

        SET @EmployeeDocumentID = SCOPE_IDENTITY();

        INSERT INTO hr.EmployeeDocumentVersion (
            TenantID, EmployeeDocumentID, VersionNumber, [FileName], FilePath, MimeType, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeDocumentID, 1, @FileName, @FilePath, @MimeType, @CreatedBy, GETUTCDATE(), 0
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeDocument_Upload', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;