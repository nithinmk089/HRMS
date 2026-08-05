
CREATE PROCEDURE hr.usp_EmployeeEmployment_Search
    @TenantID BIGINT,
    @EmployeeID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * 
    FROM hr.vw_EmployeeEmployment
    WHERE TenantID = @TenantID AND EmployeeID = @EmployeeID;
END;