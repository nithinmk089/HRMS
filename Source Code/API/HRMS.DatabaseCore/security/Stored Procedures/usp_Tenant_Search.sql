
CREATE   PROCEDURE security.usp_Tenant_Search
    @SearchText VARCHAR(250) = NULL,
    @Status VARCHAR(20) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    -- Count total records
    SELECT COUNT(*) AS TotalCount
    FROM security.Tenant
    WHERE IsDeleted = 0
      AND (@SearchText IS NULL OR TenantName LIKE '%' + @SearchText + '%' OR TenantCode LIKE '%' + @SearchText + '%')
      AND (@Status IS NULL OR [Status] = @Status);

    -- Paged results
    SELECT TenantID, TenantCode, TenantName, [Status], EffectiveFrom, EffectiveTo, VersionNo
    FROM security.Tenant
    WHERE IsDeleted = 0
      AND (@SearchText IS NULL OR TenantName LIKE '%' + @SearchText + '%' OR TenantCode LIKE '%' + @SearchText + '%')
      AND (@Status IS NULL OR [Status] = @Status)
    ORDER BY TenantName
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;