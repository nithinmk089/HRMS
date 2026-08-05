
CREATE VIEW onboarding.vw_DocumentVerificationStatus
AS
SELECT 
    ds.EmployeeDocumentSubmissionID,
    ds.TenantID,
    ds.EmployeeID,
    e.FirstName + ' ' + e.LastName AS EmployeeName,
    ds.DocumentType,
    ds.FileName,
    ds.UploadedDate,
    ISNULL(dv.VerificationStatus, 'Pending') AS VerificationStatus,
    dv.VerificationRemarks,
    dv.VerifiedDate
FROM onboarding.EmployeeDocumentSubmission ds
INNER JOIN hr.Employee e ON ds.EmployeeID = e.EmployeeID
LEFT JOIN onboarding.EmployeeDocumentVerification dv ON ds.EmployeeDocumentSubmissionID = dv.EmployeeDocumentSubmissionID
WHERE ds.IsDeleted = 0;