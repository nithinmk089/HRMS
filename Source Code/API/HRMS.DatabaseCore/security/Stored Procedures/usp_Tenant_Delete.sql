
CREATE   PROCEDURE security.usp_Tenant_Delete
    @TenantID BIGINT,
    @DeletedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE security.Tenant
        SET IsDeleted = 1,
            DeletedBy = @DeletedBy,
            DeletedDate = GETUTCDATE(),
            [Status] = 'Inactive'
        WHERE TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace)
        VALUES ('security.usp_Tenant_Delete', ERROR_MESSAGE(), CONVERT(VARCHAR, ERROR_STATE()));
        THROW;
    END CATCH
END;