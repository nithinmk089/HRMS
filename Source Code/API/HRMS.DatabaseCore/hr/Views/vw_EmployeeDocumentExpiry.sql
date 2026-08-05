
CREATE VIEW hr.vw_EmployeeDocumentExpiry
AS
SELECT 
    c.EmployeeCertificationID AS DocumentID,
    c.TenantID,
    c.EmployeeID,
    e.FirstName + ' ' + e.LastName AS EmployeeFullName,
    e.EmployeeCode,
    'Certification' AS DocumentCategory,
    c.CertificationName AS DocumentName,
    c.CertificateNumber AS DocumentNumber,
    c.ExpiryDate,
    DATEDIFF(day, GETUTCDATE(), c.ExpiryDate) AS DaysToExpiry
FROM hr.EmployeeCertification c
INNER JOIN hr.Employee e ON c.EmployeeID = e.EmployeeID
WHERE c.IsDeleted = 0 AND c.ExpiryDate IS NOT NULL;