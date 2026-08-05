
CREATE   PROCEDURE organization.usp_Location_Search
    @TenantID BIGINT,
    @SearchText VARCHAR(250) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM organization.Location
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR LocationName LIKE '%' + @SearchText + '%' OR LocationCode LIKE '%' + @SearchText + '%');

    SELECT LocationID, TenantID, LocationCode, LocationName, CountryCode, StateCode, City, VersionNo
    FROM organization.Location
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR LocationName LIKE '%' + @SearchText + '%' OR LocationCode LIKE '%' + @SearchText + '%')
    ORDER BY LocationName
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;