
CREATE   PROCEDURE security.usp_User_Search
    @TenantID BIGINT,
    @SearchText VARCHAR(250) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM security.[User]
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR UserName LIKE '%' + @SearchText + '%' OR Email LIKE '%' + @SearchText + '%');

    SELECT UserID, TenantID, EmployeeID, UserName, Email, IsLocked, LastLoginDate, VersionNo
    FROM security.[User]
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR UserName LIKE '%' + @SearchText + '%' OR Email LIKE '%' + @SearchText + '%')
    ORDER BY UserName
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;