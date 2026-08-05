
-- asset.usp_Report_AssetDisposal.sql
CREATE PROCEDURE asset.usp_Report_AssetDisposal
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ad.AssetDisposalID, am.AssetCode, am.AssetName, ad.DisposalDate, ad.DisposalMethod, ad.DisposalValue, ad.DisposalStatus
    FROM asset.AssetDisposal ad
    JOIN asset.AssetMaster am ON ad.AssetID = am.AssetID
    WHERE ad.TenantID = @TenantID AND ad.IsDeleted = 0;
END;