
CREATE PROCEDURE offboarding.usp_Report_ExitStatus
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM offboarding.vw_ExitStatus
    WHERE TenantID = @TenantID;
END