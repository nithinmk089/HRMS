
CREATE   FUNCTION system.fn_GetTenantConfiguration
(
    @TenantID BIGINT
)
RETURNS TABLE
AS
RETURN
(
    SELECT ConfigurationKey, ConfigurationValue, DataType
    FROM system.Configuration
    WHERE TenantID = @TenantID AND IsDeleted = 0
);