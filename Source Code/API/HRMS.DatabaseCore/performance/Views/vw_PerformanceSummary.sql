
-- performance.vw_PerformanceSummary.sql
CREATE VIEW performance.vw_PerformanceSummary
AS
SELECT 
    pc.PerformanceCycleID,
    pc.TenantID,
    pc.CycleCode,
    pc.CycleName,
    pc.StartDate,
    pc.EndDate,
    pc.CycleStatus,
    COUNT(g.GoalID) AS TotalGoals,
    ISNULL(AVG(performance.fn_CalculateGoalAchievement(g.GoalID, g.TenantID)), 0) AS AvgGoalAchievement
FROM performance.PerformanceCycle pc
LEFT JOIN performance.Goal g ON pc.PerformanceCycleID = g.PerformanceCycleID AND g.IsDeleted = 0
WHERE pc.IsDeleted = 0
GROUP BY pc.PerformanceCycleID, pc.TenantID, pc.CycleCode, pc.CycleName, pc.StartDate, pc.EndDate, pc.CycleStatus;