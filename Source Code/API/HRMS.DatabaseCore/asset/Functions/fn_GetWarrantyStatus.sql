
-- ==========================================
-- 2. FUNCTIONS
-- ==========================================
-- asset.fn_GetWarrantyStatus.sql
CREATE FUNCTION asset.fn_GetWarrantyStatus (@AssetID BIGINT)
RETURNS NVARCHAR(50)
AS
BEGIN
    DECLARE @Status NVARCHAR(50) = 'No Warranty';
    SELECT TOP 1 @Status = CASE WHEN GETUTCDATE() BETWEEN WarrantyStartDate AND WarrantyEndDate THEN 'Active' ELSE 'Expired' END
    FROM asset.AssetWarranty
    WHERE AssetID = @AssetID AND IsDeleted = 0
    ORDER BY WarrantyEndDate DESC;
    RETURN @Status;
END;