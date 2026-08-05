
CREATE VIEW hr.vw_EmployeeContactSummary
AS
SELECT 
    e.EmployeeID,
    e.TenantID,
    e.FirstName + ' ' + e.LastName AS EmployeeFullName,
    e.PersonalEmail,
    e.MobileNumber,
    (SELECT STRING_AGG(CONCAT(AddressType, ': ', AddressLine1, ', ', City, ', ', Country), ' | ') FROM hr.EmployeeAddress ea WHERE ea.EmployeeID = e.EmployeeID AND ea.IsDeleted = 0) AS AddressesSummary,
    (SELECT STRING_AGG(CONCAT(ContactType, ': ', ContactValue), ' | ') FROM hr.EmployeeContact ec WHERE ec.EmployeeID = e.EmployeeID AND ec.IsDeleted = 0) AS ContactsSummary,
    (SELECT STRING_AGG(CONCAT(ContactName, ' (', Relationship, '): ', MobileNumber), ' | ') FROM hr.EmployeeEmergencyContact eec WHERE eec.EmployeeID = e.EmployeeID AND eec.IsDeleted = 0) AS EmergencyContactsSummary
FROM hr.Employee e
WHERE e.IsDeleted = 0;