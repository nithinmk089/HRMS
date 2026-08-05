
-- asset.usp_AssetAudit_Create.sql
CREATE PROCEDURE asset.usp_AssetAudit_Create
    @TenantID BIGINT,
    @AssetID BIGINT,
    @AuditDate DATE,
    @AuditorID BIGINT,
    @CreatedBy BIGINT,
    @AssetAuditID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO asset.AssetAudit (TenantID, AssetID, AuditDate, AuditorID, AuditStatus, CreatedBy)
        VALUES (@TenantID, @AssetID, @AuditDate, @AuditorID, 'Scheduled', @CreatedBy);
        SET @AssetAuditID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;