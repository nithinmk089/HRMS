
-- ==========================================
-- 3. FUNCTIONS
-- ==========================================

CREATE FUNCTION onboarding.fn_GetCompletionPercentage
(
    @EmployeeID BIGINT,
    @TenantID BIGINT
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @Total INT = 0;
    DECLARE @Completed INT = 0;
    DECLARE @Percentage DECIMAL(5,2) = 0.00;

    SELECT @Total = COUNT(*), 
           @Completed = SUM(CASE WHEN TaskStatus = 'Completed' THEN 1 ELSE 0 END)
    FROM onboarding.OnboardingTaskAssignment
    WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0;

    IF @Total > 0
    BEGIN
        SET @Percentage = CAST(@Completed AS DECIMAL(5,2)) / CAST(@Total AS DECIMAL(5,2)) * 100.00;
    END

    RETURN @Percentage;
END