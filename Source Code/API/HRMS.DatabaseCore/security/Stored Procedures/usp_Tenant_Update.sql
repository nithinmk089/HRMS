
CREATE   PROCEDURE security.usp_Tenant_Update
    @TenantID BIGINT,
    @TenantName VARCHAR(200),
    @Status VARCHAR(20),
    @EffectiveFrom DATETIME,
    @EffectiveTo DATETIME = NULL,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE security.Tenant
        SET TenantName = @TenantName,
            [Status] = @Status,
            EffectiveFrom = @EffectiveFrom,
            EffectiveTo = @EffectiveTo,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE(),
            VersionNo = VersionNo + 1
        WHERE TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace)
        VALUES ('security.usp_Tenant_Update', ERROR_MESSAGE(), CONVERT(VARCHAR, ERROR_STATE()));
        THROW;
    END CATCH
END;