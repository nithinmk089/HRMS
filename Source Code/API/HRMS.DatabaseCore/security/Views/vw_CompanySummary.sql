
CREATE VIEW security.vw_CompanySummary
AS
SELECT 
    c.CompanyID, c.TenantID, t.TenantName, c.CompanyCode, c.CompanyName, c.LegalName, c.TaxNumber, c.Email, c.Phone, c.Website
FROM security.Company c
INNER JOIN security.Tenant t ON t.TenantID = c.TenantID
WHERE c.IsDeleted = 0;