
CREATE PROCEDURE offboarding.usp_Report_FullAndFinalSummary
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM offboarding.vw_SettlementSummary
    WHERE TenantID = @TenantID;
END