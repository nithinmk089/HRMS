
-- asset.fn_GetBookValue.sql
CREATE FUNCTION asset.fn_GetBookValue (@AssetID BIGINT)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @Value DECIMAL(18,2);
    SELECT @Value = CurrentBookValue FROM asset.AssetMaster WHERE AssetID = @AssetID AND IsDeleted = 0;
    RETURN COALESCE(@Value, 0);
END;