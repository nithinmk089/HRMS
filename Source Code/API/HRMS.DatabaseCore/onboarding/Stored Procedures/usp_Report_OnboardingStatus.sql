
CREATE PROCEDURE onboarding.usp_Report_OnboardingStatus
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM onboarding.vw_OnboardingStatus
    WHERE TenantID = @TenantID;
END