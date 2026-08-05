
-- performance.usp_SelfAssessment_Create.sql
CREATE PROCEDURE performance.usp_SelfAssessment_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @PerformanceCycleID BIGINT,
    @CreatedBy BIGINT,
    @SelfAssessmentID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO performance.SelfAssessment (TenantID, EmployeeID, PerformanceCycleID, AssessmentStatus, CreatedBy)
    VALUES (@TenantID, @EmployeeID, @PerformanceCycleID, 'Pending', @CreatedBy);
    SET @SelfAssessmentID = SCOPE_IDENTITY();
END