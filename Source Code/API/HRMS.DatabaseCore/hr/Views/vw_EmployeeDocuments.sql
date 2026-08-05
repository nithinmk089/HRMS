
CREATE VIEW hr.vw_EmployeeDocuments
AS
SELECT 
    d.EmployeeDocumentID,
    d.TenantID,
    d.EmployeeID,
    e.FirstName + ' ' + e.LastName AS EmployeeFullName,
    e.EmployeeCode,
    d.DocumentType,
    d.[FileName],
    d.FilePath,
    d.MimeType,
    d.VersionNumber,
    d.CreatedDate,
    d.CreatedBy
FROM hr.EmployeeDocument d
INNER JOIN hr.Employee e ON d.EmployeeID = e.EmployeeID
WHERE d.IsDeleted = 0;