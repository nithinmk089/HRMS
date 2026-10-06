
-- ==========================================
-- 3. VIEWS
-- ==========================================

-- performance.vw_GoalProgress.sql
CREATE VIEW performance.vw_GoalProgress
AS
SELECT 
    g.GoalID,
    g.TenantID,
    g.EmployeeID,
    CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName,
    g.PerformanceCycleID,
    g.GoalTitle,
    g.GoalDescription,
    g.Weightage,
    g.TargetValue,
    g.AchievementValue,
    performance.fn_CalculateGoalAchievement(g.GoalID, g.TenantID) AS AchievementPercentage,
    g.GoalStatus
FROM performance.Goal g
LEFT JOIN hr.Employee e ON g.EmployeeID = e.EmployeeID
WHERE g.IsDeleted = 0;