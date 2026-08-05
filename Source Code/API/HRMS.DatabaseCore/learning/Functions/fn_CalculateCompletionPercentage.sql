
-- learning.fn_CalculateCompletionPercentage.sql
CREATE FUNCTION learning.fn_CalculateCompletionPercentage
(
    @EnrollmentID BIGINT,
    @TenantID BIGINT
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @Pct DECIMAL(5,2) = 0;
    SELECT TOP 1 @Pct = CompletionPercentage
    FROM learning.LearningProgress
    WHERE EnrollmentID = @EnrollmentID AND TenantID = @TenantID AND IsDeleted = 0
    ORDER BY LastAccessedDate DESC;
    RETURN ISNULL(@Pct, 0);
END