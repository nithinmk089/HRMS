
-- learning.usp_ComplianceAcknowledgement_Record.sql
CREATE PROCEDURE learning.usp_ComplianceAcknowledgement_Record
    @TenantID BIGINT, @EmployeeID BIGINT, @ComplianceTrainingID BIGINT,
    @CreatedBy BIGINT, @ComplianceAcknowledgementID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO learning.ComplianceAcknowledgement (TenantID, EmployeeID, ComplianceTrainingID, CreatedBy)
    VALUES (@TenantID, @EmployeeID, @ComplianceTrainingID, @CreatedBy);
    SET @ComplianceAcknowledgementID = SCOPE_IDENTITY();
END