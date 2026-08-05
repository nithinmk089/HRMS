
CREATE FUNCTION hr.fn_GetCurrentDesignation
(
    @EmployeeID BIGINT
)
RETURNS BIGINT
AS
BEGIN
    DECLARE @DesignationID BIGINT;
    SELECT @DesignationID = DesignationID
    FROM hr.EmployeeEmployment
    WHERE EmployeeID = @EmployeeID AND IsDeleted = 0 AND EmploymentStatus = 'Active';
    RETURN @DesignationID;
END;