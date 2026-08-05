
CREATE   FUNCTION system.fn_GetConfigurationValue
(
    @TenantID BIGINT,
    @Key VARCHAR(200)
)
RETURNS VARCHAR(MAX)
AS
BEGIN
    DECLARE @Val VARCHAR(MAX);
    SELECT @Val = ConfigurationValue
    FROM system.Configuration
    WHERE TenantID = @TenantID AND ConfigurationKey = @Key AND IsDeleted = 0;
    RETURN @Val;
END;