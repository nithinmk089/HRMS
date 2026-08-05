
CREATE FUNCTION onboarding.fn_GetPendingTaskCount
(
    @EmployeeID BIGINT,
    @TenantID BIGINT
)
RETURNS INT
AS
BEGIN
    DECLARE @Count INT = 0;

    SELECT @Count = COUNT(*)
    FROM onboarding.OnboardingTaskAssignment
    WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND TaskStatus <> 'Completed' AND IsDeleted = 0;

    RETURN @Count;
END