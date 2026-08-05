
CREATE   FUNCTION organization.fn_GetDepartmentHierarchy
(
    @DepartmentID BIGINT
)
RETURNS TABLE
AS
RETURN
(
    WITH DeptCTE AS (
        SELECT DepartmentID, DepartmentCode, DepartmentName, ParentDepartmentID, 1 AS [Level]
        FROM organization.Department
        WHERE DepartmentID = @DepartmentID AND IsDeleted = 0
        UNION ALL
        SELECT d.DepartmentID, d.DepartmentCode, d.DepartmentName, d.ParentDepartmentID, cte.[Level] + 1
        FROM organization.Department d
        INNER JOIN DeptCTE cte ON d.ParentDepartmentID = cte.DepartmentID
        WHERE d.IsDeleted = 0
    )
    SELECT DepartmentID, DepartmentCode, DepartmentName, ParentDepartmentID, [Level]
    FROM DeptCTE
);