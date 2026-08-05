
CREATE   PROCEDURE security.usp_Tenant_Summary_Report
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [Status], COUNT(*) AS TenantCount
    FROM security.Tenant
    WHERE IsDeleted = 0
    GROUP BY [Status];
END;