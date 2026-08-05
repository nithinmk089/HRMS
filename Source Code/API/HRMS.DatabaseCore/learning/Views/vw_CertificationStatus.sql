
-- learning.vw_CertificationStatus.sql
CREATE VIEW learning.vw_CertificationStatus
AS
SELECT cr.CertificationRenewalID, cr.TenantID, cr.EmployeeID, cr.RenewalDate, cr.ExpiryDate,
       lc.CertificationName, lc.ValidityMonths
FROM learning.CertificationRenewal cr
INNER JOIN learning.LearningCertification lc ON cr.LearningCertificationID = lc.LearningCertificationID
WHERE cr.IsDeleted = 0;