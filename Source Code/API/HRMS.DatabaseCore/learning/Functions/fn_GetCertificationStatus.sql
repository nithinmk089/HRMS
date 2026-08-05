
-- learning.fn_GetCertificationStatus.sql
CREATE FUNCTION learning.fn_GetCertificationStatus
(
    @EmployeeID BIGINT,
    @TenantID BIGINT
)
RETURNS NVARCHAR(50)
AS
BEGIN
    DECLARE @Status NVARCHAR(50) = 'None';
    IF EXISTS (SELECT 1 FROM learning.CertificationRenewal WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND ExpiryDate > GETUTCDATE() AND IsDeleted = 0)
        SET @Status = 'Active';
    ELSE IF EXISTS (SELECT 1 FROM learning.CertificationRenewal WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0)
        SET @Status = 'Expired';
    RETURN @Status;
END