
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
    g.PerformanceCycleID,
    g.GoalTitle,
    g.Weightage,
    g.TargetValue,
    g.AchievementValue,
    performance.fn_CalculateGoalAchievement(g.GoalID, g.TenantID) AS AchievementPercentage
FROM performance.Goal g
WHERE g.IsDeleted = 0;