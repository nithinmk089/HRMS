
CREATE FUNCTION hr.fn_GetCurrentLocation
(
    @EmployeeID BIGINT
)
RETURNS BIGINT
AS
BEGIN
    DECLARE @LocationID BIGINT;
    SELECT @LocationID = LocationID
    FROM hr.EmployeeEmployment
    WHERE EmployeeID = @EmployeeID AND IsDeleted = 0 AND EmploymentStatus = 'Active';
    RETURN @LocationID;
END;