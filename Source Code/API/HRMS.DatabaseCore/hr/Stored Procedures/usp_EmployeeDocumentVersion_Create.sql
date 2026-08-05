
CREATE PROCEDURE hr.usp_EmployeeDocumentVersion_Create
    @TenantID BIGINT,
    @EmployeeDocumentID BIGINT,
    @VersionNumber INT,
    @FileName NVARCHAR(250),
    @FilePath NVARCHAR(500),
    @MimeType NVARCHAR(100),
    @CreatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO hr.EmployeeDocumentVersion (
            TenantID, EmployeeDocumentID, VersionNumber, [FileName], FilePath, MimeType, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeDocumentID, @VersionNumber, @FileName, @FilePath, @MimeType, @CreatedBy, GETUTCDATE(), 0
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeDocumentVersion_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;