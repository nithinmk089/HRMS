
-- asset.fn_GetAssetAge.sql
CREATE FUNCTION asset.fn_GetAssetAge (@AssetID BIGINT)
RETURNS INT
AS
BEGIN
    DECLARE @Age INT = 0;
    SELECT @Age = DATEDIFF(month, PurchaseDate, GETUTCDATE())
    FROM asset.AssetMaster
    WHERE AssetID = @AssetID AND IsDeleted = 0;
    RETURN COALESCE(@Age, 0);
END;