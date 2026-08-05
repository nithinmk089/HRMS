
-- asset.usp_AssetAudit_Complete.sql
CREATE PROCEDURE asset.usp_AssetAudit_Complete
    @AssetAuditID BIGINT,
    @TenantID BIGINT,
    @Findings NVARCHAR(1000) = NULL,
    @AuditStatus NVARCHAR(50),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE asset.AssetAudit
        SET Findings = @Findings,
            AuditStatus = @AuditStatus,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE AssetAuditID = @AssetAuditID AND TenantID = @TenantID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;