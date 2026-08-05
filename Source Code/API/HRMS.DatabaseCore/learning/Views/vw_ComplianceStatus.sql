
-- learning.vw_ComplianceStatus.sql
CREATE VIEW learning.vw_ComplianceStatus
AS
SELECT ca.ComplianceAcknowledgementID, ca.TenantID, ca.EmployeeID, ca.AcknowledgedDate,
       ct.ComplianceType, ct.MandatoryFlag
FROM learning.ComplianceAcknowledgement ca
INNER JOIN learning.ComplianceTraining ct ON ca.ComplianceTrainingID = ct.ComplianceTrainingID
WHERE ca.IsDeleted = 0;