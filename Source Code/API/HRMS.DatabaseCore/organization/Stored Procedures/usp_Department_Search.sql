
CREATE   PROCEDURE organization.usp_Department_Search
    @TenantID BIGINT,
    @BusinessUnitID BIGINT = NULL,
    @SearchText VARCHAR(250) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM organization.Department
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@BusinessUnitID IS NULL OR BusinessUnitID = @BusinessUnitID)
      AND (@SearchText IS NULL OR DepartmentName LIKE '%' + @SearchText + '%' OR DepartmentCode LIKE '%' + @SearchText + '%');

    SELECT DepartmentID, TenantID, BusinessUnitID, DepartmentCode, DepartmentName, ParentDepartmentID, VersionNo
    FROM organization.Department
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@BusinessUnitID IS NULL OR BusinessUnitID = @BusinessUnitID)
      AND (@SearchText IS NULL OR DepartmentName LIKE '%' + @SearchText + '%' OR DepartmentCode LIKE '%' + @SearchText + '%')
    ORDER BY DepartmentName
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;