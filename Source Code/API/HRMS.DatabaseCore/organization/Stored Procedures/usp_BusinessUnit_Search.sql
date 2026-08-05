
CREATE   PROCEDURE organization.usp_BusinessUnit_Search
    @TenantID BIGINT,
    @CompanyID BIGINT = NULL,
    @SearchText VARCHAR(250) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM organization.BusinessUnit
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@CompanyID IS NULL OR CompanyID = @CompanyID)
      AND (@SearchText IS NULL OR BusinessUnitName LIKE '%' + @SearchText + '%' OR BusinessUnitCode LIKE '%' + @SearchText + '%');

    SELECT BusinessUnitID, TenantID, CompanyID, BusinessUnitCode, BusinessUnitName, VersionNo
    FROM organization.BusinessUnit
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@CompanyID IS NULL OR CompanyID = @CompanyID)
      AND (@SearchText IS NULL OR BusinessUnitName LIKE '%' + @SearchText + '%' OR BusinessUnitCode LIKE '%' + @SearchText + '%')
    ORDER BY BusinessUnitName
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;