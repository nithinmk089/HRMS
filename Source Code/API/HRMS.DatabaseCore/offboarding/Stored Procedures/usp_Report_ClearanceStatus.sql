
CREATE PROCEDURE offboarding.usp_Report_ClearanceStatus
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM offboarding.vw_ClearanceStatus
    WHERE TenantID = @TenantID;
END