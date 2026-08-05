
CREATE   PROCEDURE security.usp_Role_Search
    @TenantID BIGINT,
    @SearchText VARCHAR(250) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM security.[Role]
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR RoleName LIKE '%' + @SearchText + '%' OR RoleCode LIKE '%' + @SearchText + '%');

    SELECT RoleID, TenantID, RoleCode, RoleName, [Description], VersionNo
    FROM security.[Role]
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR RoleName LIKE '%' + @SearchText + '%' OR RoleCode LIKE '%' + @SearchText + '%')
    ORDER BY RoleName
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;