
-- performance.fn_CalculateGoalAchievement.sql
CREATE FUNCTION performance.fn_CalculateGoalAchievement
(
    @GoalID BIGINT,
    @TenantID BIGINT
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @Percentage DECIMAL(5,2) = 0;
    SELECT TOP 1 @Percentage = ProgressPercentage 
    FROM performance.GoalProgress 
    WHERE GoalID = @GoalID AND TenantID = @TenantID AND IsDeleted = 0
    ORDER BY ProgressDate DESC;
    RETURN ISNULL(@Percentage, 0);
END