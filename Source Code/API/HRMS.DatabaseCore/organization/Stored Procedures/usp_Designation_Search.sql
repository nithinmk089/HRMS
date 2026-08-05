
CREATE   PROCEDURE organization.usp_Designation_Search
    @TenantID BIGINT,
    @SearchText VARCHAR(250) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM organization.Designation
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR DesignationName LIKE '%' + @SearchText + '%' OR DesignationCode LIKE '%' + @SearchText + '%');

    SELECT DesignationID, TenantID, DesignationCode, DesignationName, Grade, VersionNo
    FROM organization.Designation
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR DesignationName LIKE '%' + @SearchText + '%' OR DesignationCode LIKE '%' + @SearchText + '%')
    ORDER BY DesignationName
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;