
CREATE   VIEW system.vw_SystemConfiguration
AS
SELECT 
    c.ConfigurationID, c.TenantID, t.TenantName, c.ConfigurationKey, c.ConfigurationValue, c.DataType
FROM system.Configuration c
INNER JOIN security.Tenant t ON t.TenantID = c.TenantID
WHERE c.IsDeleted = 0;