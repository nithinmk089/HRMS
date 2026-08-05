
CREATE PROCEDURE onboarding.usp_DocumentSubmission_Search
    @TenantID   BIGINT,
    @EmployeeID BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        EmployeeDocumentSubmissionID,
        TenantID,
        EmployeeID,
        DocumentType,
        FileName,
        FilePath,
        MimeType,
        UploadedDate
    FROM onboarding.EmployeeDocumentSubmission
    WHERE TenantID = @TenantID
      AND (@EmployeeID IS NULL OR EmployeeID = @EmployeeID)
      AND IsDeleted = 0;
END