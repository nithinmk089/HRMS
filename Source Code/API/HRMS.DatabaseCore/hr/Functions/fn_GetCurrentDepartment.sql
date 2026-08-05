
CREATE FUNCTION hr.fn_GetCurrentDepartment
(
    @EmployeeID BIGINT
)
RETURNS BIGINT
AS
BEGIN
    DECLARE @DepartmentID BIGINT;
    SELECT @DepartmentID = DepartmentID
    FROM hr.EmployeeEmployment
    WHERE EmployeeID = @EmployeeID AND IsDeleted = 0 AND EmploymentStatus = 'Active';
    RETURN @DepartmentID;
END;