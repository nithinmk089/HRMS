
CREATE   PROCEDURE security.usp_Company_Summary_Report
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.CompanyCode, c.CompanyName,
        (SELECT COUNT(*) FROM organization.BusinessUnit bu WHERE bu.CompanyID = c.CompanyID AND bu.IsDeleted = 0) AS BusinessUnitsCount
    FROM security.Company c
    WHERE c.TenantID = @TenantID AND c.IsDeleted = 0;
END;