
-- ==========================================
-- 4. STORED PROCEDURES
-- ==========================================

-- performance.usp_GoalProgress_Create.sql
CREATE PROCEDURE performance.usp_GoalProgress_Create
    @TenantID BIGINT,
    @GoalID BIGINT,
    @ProgressPercentage DECIMAL(5,2),
    @Remarks NVARCHAR(500) = NULL,
    @CreatedBy BIGINT,
    @GoalProgressID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRANSACTION;
    
    INSERT INTO performance.GoalProgress (TenantID, GoalID, ProgressPercentage, Remarks, CreatedBy)
    VALUES (@TenantID, @GoalID, @ProgressPercentage, @Remarks, @CreatedBy);
    SET @GoalProgressID = SCOPE_IDENTITY();
    
    -- Update Goal table achievement
    UPDATE performance.Goal
    SET AchievementValue = (TargetValue * @ProgressPercentage / 100.0)
    WHERE GoalID = @GoalID AND TenantID = @TenantID;
    
    COMMIT TRANSACTION;
END