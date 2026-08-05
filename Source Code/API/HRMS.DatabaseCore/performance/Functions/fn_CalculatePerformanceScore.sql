
-- ==========================================
-- 2. FUNCTIONS
-- ==========================================
-- performance.fn_CalculatePerformanceScore.sql
CREATE FUNCTION performance.fn_CalculatePerformanceScore
(
    @EmployeeID BIGINT,
    @CycleID BIGINT,
    @TenantID BIGINT
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @Score DECIMAL(5,2) = 0;
    SELECT @Score = SUM((AchievementValue / NULLIF(TargetValue, 0)) * Weightage)
    FROM performance.Goal
    WHERE EmployeeID = @EmployeeID AND PerformanceCycleID = @CycleID AND TenantID = @TenantID AND IsDeleted = 0;
    RETURN ISNULL(@Score, 0);
END