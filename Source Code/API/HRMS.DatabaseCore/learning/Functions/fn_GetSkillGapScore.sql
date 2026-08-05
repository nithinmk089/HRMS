
-- learning.fn_GetSkillGapScore.sql
CREATE FUNCTION learning.fn_GetSkillGapScore
(
    @EmployeeID BIGINT,
    @TenantID BIGINT
)
RETURNS INT
AS
BEGIN
    DECLARE @TotalSkills INT, @EmployeeSkills INT;
    SELECT @TotalSkills = COUNT(*) FROM learning.Skill WHERE TenantID = @TenantID AND IsDeleted = 0;
    SELECT @EmployeeSkills = COUNT(*) FROM learning.EmployeeSkill WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0;
    RETURN ISNULL(@TotalSkills - @EmployeeSkills, 0);
END