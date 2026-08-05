
CREATE PROCEDURE hr.usp_Employee_Search
    @TenantID BIGINT,
    @SearchText NVARCHAR(100) = NULL,
    @Status NVARCHAR(50) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM hr.vw_EmployeeDirectory
    WHERE TenantID = @TenantID
      AND (@Status IS NULL OR [Status] = @Status)
      AND (@SearchText IS NULL 
           OR EmployeeCode LIKE '%' + @SearchText + '%' 
           OR FirstName LIKE '%' + @SearchText + '%' 
           OR LastName LIKE '%' + @SearchText + '%'
           OR PersonalEmail LIKE '%' + @SearchText + '%');

    SELECT *
    FROM hr.vw_EmployeeDirectory
    WHERE TenantID = @TenantID
      AND (@Status IS NULL OR [Status] = @Status)
      AND (@SearchText IS NULL 
           OR EmployeeCode LIKE '%' + @SearchText + '%' 
           OR FirstName LIKE '%' + @SearchText + '%' 
           OR LastName LIKE '%' + @SearchText + '%'
           OR PersonalEmail LIKE '%' + @SearchText + '%')
    ORDER BY EmployeeID DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;