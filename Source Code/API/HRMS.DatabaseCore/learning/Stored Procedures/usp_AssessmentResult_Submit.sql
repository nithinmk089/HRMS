
-- learning.usp_AssessmentResult_Submit.sql
CREATE PROCEDURE learning.usp_AssessmentResult_Submit
    @TenantID BIGINT, @AssessmentID BIGINT, @EmployeeID BIGINT,
    @Score DECIMAL(5,2), @CreatedBy BIGINT, @AssessmentResultID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @PassPct DECIMAL(5,2);
    SELECT @PassPct = PassPercentage FROM learning.Assessment WHERE AssessmentID = @AssessmentID AND TenantID = @TenantID;
    DECLARE @Status NVARCHAR(50) = CASE WHEN @Score >= @PassPct THEN 'Pass' ELSE 'Fail' END;
    INSERT INTO learning.AssessmentResult (TenantID, AssessmentID, EmployeeID, Score, ResultStatus, CreatedBy)
    VALUES (@TenantID, @AssessmentID, @EmployeeID, @Score, @Status, @CreatedBy);
    SET @AssessmentResultID = SCOPE_IDENTITY();
END