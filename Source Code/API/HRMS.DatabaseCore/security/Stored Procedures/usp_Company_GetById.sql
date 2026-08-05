
CREATE   PROCEDURE security.usp_Company_GetById
    @CompanyID BIGINT,
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CompanyID, TenantID, CompanyCode, CompanyName, LegalName, TaxNumber, Email, Phone, Website, VersionNo
    FROM security.Company
    WHERE CompanyID = @CompanyID AND TenantID = @TenantID AND IsDeleted = 0;
END;