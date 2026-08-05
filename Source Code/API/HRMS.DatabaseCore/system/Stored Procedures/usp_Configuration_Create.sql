
-- STORED PROCEDURES - SYSTEM CONFIGURATION
CREATE   PROCEDURE system.usp_Configuration_Create
    @TenantID BIGINT,
    @ConfigurationKey VARCHAR(200),
    @ConfigurationValue VARCHAR(MAX),
    @DataType VARCHAR(50),
    @CreatedBy BIGINT,
    @ConfigurationID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO system.Configuration (TenantID, ConfigurationKey, ConfigurationValue, DataType, CreatedBy)
        VALUES (@TenantID, @ConfigurationKey, @ConfigurationValue, @DataType, @CreatedBy);
        SET @ConfigurationID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;