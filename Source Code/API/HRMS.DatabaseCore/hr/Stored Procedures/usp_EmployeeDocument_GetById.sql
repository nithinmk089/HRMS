
CREATE PROCEDURE hr.usp_EmployeeDocument_GetById
    @EmployeeDocumentID BIGINT,
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * 
    FROM hr.vw_EmployeeDocuments
    WHERE EmployeeDocumentID = @EmployeeDocumentID AND TenantID = @TenantID;
END;