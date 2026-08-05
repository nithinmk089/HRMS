
CREATE VIEW security.vw_TenantSummary
AS
SELECT 
    t.TenantID, t.TenantCode, t.TenantName, t.[Status], t.EffectiveFrom, t.EffectiveTo,
    (SELECT COUNT(*) FROM security.Company c WHERE c.TenantID = t.TenantID AND c.IsDeleted = 0) AS CompanyCount,
    (SELECT COUNT(*) FROM security.[User] u WHERE u.TenantID = t.TenantID AND u.IsDeleted = 0) AS UserCount
FROM security.Tenant t
WHERE t.IsDeleted = 0;