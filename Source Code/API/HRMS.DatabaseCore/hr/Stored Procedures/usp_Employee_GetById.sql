
CREATE PROCEDURE hr.usp_Employee_GetById
    @EmployeeID BIGINT,
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * 
    FROM hr.vw_EmployeeProfile
    WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID;
END;