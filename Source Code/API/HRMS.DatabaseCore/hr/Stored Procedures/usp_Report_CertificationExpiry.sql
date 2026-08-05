
CREATE PROCEDURE hr.usp_Report_CertificationExpiry
    @TenantID BIGINT,
    @DaysToExpiryThreshold INT = 30
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * 
    FROM hr.vw_EmployeeDocumentExpiry
    WHERE TenantID = @TenantID 
      AND DocumentCategory = 'Certification'
      AND DaysToExpiry <= @DaysToExpiryThreshold
    ORDER BY DaysToExpiry ASC;
END;