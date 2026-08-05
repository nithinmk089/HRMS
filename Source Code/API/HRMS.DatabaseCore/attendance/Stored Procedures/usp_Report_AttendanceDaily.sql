
CREATE PROCEDURE attendance.usp_Report_AttendanceDaily
    @TenantID BIGINT,
    @AttendanceDate DATE,
    @DepartmentID BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT d.* 
    FROM attendance.vw_AttendanceDaily d
    LEFT JOIN hr.EmployeeEmployment ee ON d.EmployeeID = ee.EmployeeID AND ee.IsDeleted = 0
    WHERE d.TenantID = @TenantID 
      AND d.AttendanceDate = @AttendanceDate
      AND d.IsDeleted = 0
      AND (@DepartmentID IS NULL OR ee.DepartmentID = @DepartmentID);
END;