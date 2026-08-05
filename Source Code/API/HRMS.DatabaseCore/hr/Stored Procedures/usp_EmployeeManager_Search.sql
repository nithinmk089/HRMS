
CREATE PROCEDURE hr.usp_EmployeeManager_Search
    @TenantID BIGINT,
    @EmployeeID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * 
    FROM hr.vw_EmployeeHierarchy
    WHERE TenantID = @TenantID AND EmployeeID = @EmployeeID;
END;