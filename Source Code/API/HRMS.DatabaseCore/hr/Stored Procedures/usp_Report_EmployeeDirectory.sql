
CREATE PROCEDURE hr.usp_Report_EmployeeDirectory
    @TenantID BIGINT,
    @DepartmentID BIGINT = NULL,
    @LocationID BIGINT = NULL,
    @Status NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        e.EmployeeID,
        e.EmployeeCode,
        e.EmployeeNumber,
        e.FirstName + ' ' + e.LastName AS FullName,
        e.PersonalEmail,
        e.MobileNumber,
        d.DepartmentName,
        dg.DesignationName,
        l.LocationName,
        ee.JoiningDate,
        e.[Status]
    FROM hr.Employee e
    INNER JOIN hr.EmployeeEmployment ee ON e.EmployeeID = ee.EmployeeID AND ee.IsDeleted = 0
    LEFT JOIN organization.Department d ON ee.DepartmentID = d.DepartmentID
    LEFT JOIN organization.Designation dg ON ee.DesignationID = dg.DesignationID
    LEFT JOIN organization.Location l ON ee.LocationID = l.LocationID
    WHERE e.TenantID = @TenantID AND e.IsDeleted = 0
      AND (@DepartmentID IS NULL OR ee.DepartmentID = @DepartmentID)
      AND (@LocationID IS NULL OR ee.LocationID = @LocationID)
      AND (@Status IS NULL OR e.[Status] = @Status);
END;