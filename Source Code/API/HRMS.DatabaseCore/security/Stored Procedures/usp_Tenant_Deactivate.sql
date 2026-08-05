
CREATE   PROCEDURE security.usp_Tenant_Deactivate
    @TenantID BIGINT,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE security.Tenant
        SET [Status] = 'Inactive', ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE(), VersionNo = VersionNo + 1
        WHERE TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;