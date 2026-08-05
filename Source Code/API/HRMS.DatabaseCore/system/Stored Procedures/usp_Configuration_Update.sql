
CREATE   PROCEDURE system.usp_Configuration_Update
    @ConfigurationID BIGINT,
    @TenantID BIGINT,
    @ConfigurationValue VARCHAR(MAX),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE system.Configuration
        SET ConfigurationValue = @ConfigurationValue,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE(),
            VersionNo = VersionNo + 1
        WHERE ConfigurationID = @ConfigurationID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;