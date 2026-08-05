
-- asset.fn_GetCurrentHolder.sql
CREATE FUNCTION asset.fn_GetCurrentHolder (@AssetID BIGINT)
RETURNS NVARCHAR(200)
AS
BEGIN
    DECLARE @Holder NVARCHAR(200) = NULL;
    SELECT TOP 1 @Holder = COALESCE(e.FirstName + ' ' + e.LastName, '')
    FROM asset.AssetAssignment a
    JOIN hr.Employee e ON a.EmployeeID = e.EmployeeID
    WHERE a.AssetID = @AssetID AND a.AssignmentStatus = 'Active' AND a.IsDeleted = 0
    ORDER BY a.AssignedDate DESC;
    RETURN @Holder;
END;