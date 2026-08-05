
-- asset.fn_CalculateDepreciation.sql
CREATE FUNCTION asset.fn_CalculateDepreciation (@AssetID BIGINT, @AsOfDate DATE)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @Cost DECIMAL(18,2);
    DECLARE @PurchaseDate DATE;
    DECLARE @Rate DECIMAL(5,2);
    DECLARE @Depreciated DECIMAL(18,2) = 0;

    SELECT @Cost = PurchaseCost, @PurchaseDate = PurchaseDate
    FROM asset.AssetMaster
    WHERE AssetID = @AssetID AND IsDeleted = 0;

    SET @Rate = 10.0;

    IF @Cost IS NOT NULL AND @PurchaseDate IS NOT NULL
    BEGIN
        DECLARE @Years INT = DATEDIFF(year, @PurchaseDate, @AsOfDate);
        IF @Years > 0
        BEGIN
            SET @Depreciated = (@Cost * (@Rate / 100.0)) * @Years;
            IF @Depreciated > @Cost SET @Depreciated = @Cost;
        END
    END
    RETURN @Depreciated;
END;