
CREATE   PROCEDURE security.usp_Company_Search
    @TenantID BIGINT,
    @SearchText VARCHAR(250) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM security.Company
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR CompanyName LIKE '%' + @SearchText + '%' OR CompanyCode LIKE '%' + @SearchText + '%');

    SELECT CompanyID, TenantID, CompanyCode, CompanyName, LegalName, TaxNumber, Email, Phone, Website, VersionNo
    FROM security.Company
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR CompanyName LIKE '%' + @SearchText + '%' OR CompanyCode LIKE '%' + @SearchText + '%')
    ORDER BY CompanyName
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;