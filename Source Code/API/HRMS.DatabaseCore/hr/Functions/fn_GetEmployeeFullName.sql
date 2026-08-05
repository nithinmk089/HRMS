
CREATE FUNCTION hr.fn_GetEmployeeFullName
(
    @EmployeeID BIGINT
)
RETURNS NVARCHAR(300)
AS
BEGIN
    DECLARE @FullName NVARCHAR(300);
    SELECT @FullName = ISNULL(FirstName, '') + CASE WHEN MiddleName IS NOT NULL THEN ' ' + MiddleName ELSE '' END + ' ' + ISNULL(LastName, '')
    FROM hr.Employee
    WHERE EmployeeID = @EmployeeID;
    RETURN @FullName;
END;