
CREATE PROCEDURE hr.usp_EmployeeDocument_Update
    @EmployeeDocumentID BIGINT,
    @TenantID BIGINT,
    @FileName NVARCHAR(250),
    @FilePath NVARCHAR(500),
    @MimeType NVARCHAR(100),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @NewVersion INT;
        SELECT @NewVersion = VersionNumber + 1 
        FROM hr.EmployeeDocument 
        WHERE EmployeeDocumentID = @EmployeeDocumentID AND TenantID = @TenantID AND IsDeleted = 0;

        UPDATE hr.EmployeeDocument
        SET [FileName] = @FileName,
            FilePath = @FilePath,
            MimeType = @MimeType,
            VersionNumber = @NewVersion,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE EmployeeDocumentID = @EmployeeDocumentID AND TenantID = @TenantID AND IsDeleted = 0;

        INSERT INTO hr.EmployeeDocumentVersion (
            TenantID, EmployeeDocumentID, VersionNumber, [FileName], FilePath, MimeType, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeDocumentID, @NewVersion, @FileName, @FilePath, @MimeType, @ModifiedBy, GETUTCDATE(), 0
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeDocument_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;