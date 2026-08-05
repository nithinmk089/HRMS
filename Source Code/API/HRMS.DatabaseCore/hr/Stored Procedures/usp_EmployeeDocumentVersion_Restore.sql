
CREATE PROCEDURE hr.usp_EmployeeDocumentVersion_Restore
    @EmployeeDocumentID BIGINT,
    @TenantID BIGINT,
    @VersionNumber INT,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @FileName NVARCHAR(250), @FilePath NVARCHAR(500), @MimeType NVARCHAR(100);

        SELECT @FileName = [FileName], @FilePath = FilePath, @MimeType = MimeType
        FROM hr.EmployeeDocumentVersion
        WHERE EmployeeDocumentID = @EmployeeDocumentID AND TenantID = @TenantID AND VersionNumber = @VersionNumber AND IsDeleted = 0;

        IF @FileName IS NOT NULL
        BEGIN
            UPDATE hr.EmployeeDocument
            SET [FileName] = @FileName,
                FilePath = @FilePath,
                MimeType = @MimeType,
                VersionNumber = @VersionNumber,
                ModifiedBy = @ModifiedBy,
                ModifiedDate = GETUTCDATE()
            WHERE EmployeeDocumentID = @EmployeeDocumentID AND TenantID = @TenantID;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeDocumentVersion_Restore', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;