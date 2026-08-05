
CREATE PROCEDURE onboarding.usp_Report_PendingTasks
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM onboarding.vw_PendingTasks
    WHERE TenantID = @TenantID;
END