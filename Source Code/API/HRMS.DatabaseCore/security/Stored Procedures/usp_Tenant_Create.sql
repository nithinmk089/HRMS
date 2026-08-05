
-- 6. STORED PROCEDURES - TENANT
CREATE   PROCEDURE security.usp_Tenant_Create
    @TenantCode VARCHAR(50),
    @TenantName VARCHAR(200),
    @Status VARCHAR(20),
    @EffectiveFrom DATETIME,
    @EffectiveTo DATETIME = NULL,
    @CreatedBy BIGINT,
    @TenantID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO security.Tenant (TenantCode, TenantName, [Status], EffectiveFrom, EffectiveTo, CreatedBy)
        VALUES (@TenantCode, @TenantName, @Status, @EffectiveFrom, @EffectiveTo, @CreatedBy);
        SET @TenantID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace)
        VALUES ('security.usp_Tenant_Create', ERROR_MESSAGE(), CONVERT(VARCHAR, ERROR_STATE()));
        THROW;
    END CATCH
END;