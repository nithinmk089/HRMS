
CREATE   PROCEDURE system.usp_Configuration_Search
    @TenantID BIGINT,
    @SearchText VARCHAR(250) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM system.Configuration
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR ConfigurationKey LIKE '%' + @SearchText + '%');

    SELECT ConfigurationID, TenantID, ConfigurationKey, ConfigurationValue, DataType, VersionNo
    FROM system.Configuration
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR ConfigurationKey LIKE '%' + @SearchText + '%')
    ORDER BY ConfigurationKey
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;