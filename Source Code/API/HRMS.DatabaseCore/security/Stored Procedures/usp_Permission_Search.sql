
CREATE   PROCEDURE security.usp_Permission_Search
    @TenantID BIGINT,
    @SearchText VARCHAR(250) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM security.[Permission]
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR PermissionName LIKE '%' + @SearchText + '%' OR PermissionCode LIKE '%' + @SearchText + '%' OR ModuleCode LIKE '%' + @SearchText + '%');

    SELECT PermissionID, TenantID, PermissionCode, PermissionName, ModuleCode, VersionNo
    FROM security.[Permission]
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR PermissionName LIKE '%' + @SearchText + '%' OR PermissionCode LIKE '%' + @SearchText + '%' OR ModuleCode LIKE '%' + @SearchText + '%')
    ORDER BY PermissionName
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;