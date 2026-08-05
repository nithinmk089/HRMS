
CREATE   PROCEDURE organization.usp_CostCenter_Search
    @TenantID BIGINT,
    @SearchText VARCHAR(250) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM organization.CostCenter
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR CostCenterName LIKE '%' + @SearchText + '%' OR CostCenterCode LIKE '%' + @SearchText + '%');

    SELECT CostCenterID, TenantID, CostCenterCode, CostCenterName, VersionNo
    FROM organization.CostCenter
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR CostCenterName LIKE '%' + @SearchText + '%' OR CostCenterCode LIKE '%' + @SearchText + '%')
    ORDER BY CostCenterName
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;