
-- performance.usp_Goal_Create.sql
CREATE PROCEDURE performance.usp_Goal_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @PerformanceCycleID BIGINT,
    @GoalTitle NVARCHAR(200),
    @GoalDescription NVARCHAR(1000) = NULL,
    @Weightage DECIMAL(5,2),
    @TargetValue DECIMAL(18,2),
    @CreatedBy BIGINT,
    @GoalID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO performance.Goal (TenantID, EmployeeID, PerformanceCycleID, GoalTitle, GoalDescription, Weightage, TargetValue, CreatedBy)
    VALUES (@TenantID, @EmployeeID, @PerformanceCycleID, @GoalTitle, @GoalDescription, @Weightage, @TargetValue, @CreatedBy);
    SET @GoalID = SCOPE_IDENTITY();
END