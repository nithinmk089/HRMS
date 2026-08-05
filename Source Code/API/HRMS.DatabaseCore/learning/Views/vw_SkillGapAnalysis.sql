
-- learning.vw_SkillGapAnalysis.sql
CREATE VIEW learning.vw_SkillGapAnalysis
AS
SELECT es.EmployeeSkillID, es.TenantID, es.EmployeeID, es.SkillLevel,
       s.SkillCode, s.SkillName, s.SkillCategory
FROM learning.EmployeeSkill es
INNER JOIN learning.Skill s ON es.SkillID = s.SkillID
WHERE es.IsDeleted = 0;