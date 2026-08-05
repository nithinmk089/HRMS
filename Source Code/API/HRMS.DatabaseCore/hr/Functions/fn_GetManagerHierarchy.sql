
CREATE FUNCTION hr.fn_GetManagerHierarchy
(
    @EmployeeID BIGINT
)
RETURNS TABLE
AS
RETURN
(
    WITH HierarchyCTE AS (
        SELECT 
            em.EmployeeID,
            em.ManagerID,
            1 AS [Level]
        FROM hr.EmployeeManager em
        WHERE em.EmployeeID = @EmployeeID AND em.IsDeleted = 0 AND (em.EffectiveTo IS NULL OR em.EffectiveTo >= CAST(GETUTCDATE() AS DATE))
        
        UNION ALL
        
        SELECT 
            em.EmployeeID,
            em.ManagerID,
            h.[Level] + 1
        FROM hr.EmployeeManager em
        INNER JOIN HierarchyCTE h ON em.EmployeeID = h.ManagerID
        WHERE em.IsDeleted = 0 AND (em.EffectiveTo IS NULL OR em.EffectiveTo >= CAST(GETUTCDATE() AS DATE))
    )
    SELECT 
        h.EmployeeID,
        (SELECT ISNULL(e.FirstName, '') + CASE WHEN e.MiddleName IS NOT NULL THEN ' ' + e.MiddleName ELSE '' END + ' ' + ISNULL(e.LastName, '') FROM hr.Employee e WHERE e.EmployeeID = h.EmployeeID) AS EmployeeName,
        h.ManagerID,
        (SELECT ISNULL(m.FirstName, '') + CASE WHEN m.MiddleName IS NOT NULL THEN ' ' + m.MiddleName ELSE '' END + ' ' + ISNULL(m.LastName, '') FROM hr.Employee m WHERE m.EmployeeID = h.ManagerID) AS ManagerName,
        h.[Level]
    FROM HierarchyCTE h
);