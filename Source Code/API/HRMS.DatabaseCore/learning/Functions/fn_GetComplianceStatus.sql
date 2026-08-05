

-- learning.fn_GetComplianceStatus.sql
CREATE FUNCTION learning.fn_GetComplianceStatus
(
    @EmployeeID BIGINT,
    @TenantID BIGINT
)
RETURNS NVARCHAR(50)
AS
BEGIN
    DECLARE @Status NVARCHAR(50) = 'Non-Compliant';
    DECLARE @Total INT, @Acked INT;
    SELECT @Total = COUNT(*) FROM learning.ComplianceTraining WHERE TenantID = @TenantID AND MandatoryFlag = 1 AND IsDeleted = 0;
    SELECT @Acked = COUNT(*) FROM learning.ComplianceAcknowledgement WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0;
    IF @Total > 0 AND @Acked >= @Total SET @Status = 'Compliant';
    RETURN @Status;
END