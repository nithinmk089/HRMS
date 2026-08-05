
CREATE VIEW hr.vw_EmployeePromotionHistory
AS
SELECT 
    p.EmployeePromotionID,
    p.TenantID,
    p.EmployeeID,
    e.FirstName + ' ' + e.LastName AS EmployeeFullName,
    e.EmployeeCode,
    p.OldDesignationID,
    od.DesignationName AS OldDesignationName,
    p.NewDesignationID,
    nd.DesignationName AS NewDesignationName,
    p.OldGrade,
    p.NewGrade,
    p.EffectiveDate,
    p.Reason,
    p.[Status],
    p.CreatedDate
FROM hr.EmployeePromotion p
INNER JOIN hr.Employee e ON p.EmployeeID = e.EmployeeID
INNER JOIN organization.Designation od ON p.OldDesignationID = od.DesignationID
INNER JOIN organization.Designation nd ON p.NewDesignationID = nd.DesignationID
WHERE p.IsDeleted = 0;