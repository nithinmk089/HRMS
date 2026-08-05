
CREATE PROCEDURE hr.usp_Employee_BulkExport
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * 
    FROM hr.vw_EmployeeDirectory
    WHERE TenantID = @TenantID;
END;