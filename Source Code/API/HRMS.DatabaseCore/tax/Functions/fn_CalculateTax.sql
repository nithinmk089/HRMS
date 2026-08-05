
-- tax.fn_CalculateTax.sql
CREATE FUNCTION tax.fn_CalculateTax
(
    @TaxableIncome DECIMAL(18,2),
    @TaxRegimeID BIGINT,
    @TenantID BIGINT
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @Tax DECIMAL(18,2) = 0;
    DECLARE @RemainingIncome DECIMAL(18,2) = @TaxableIncome;
    
    DECLARE SlabCursor CURSOR FOR
    SELECT IncomeFrom, IncomeTo, TaxRate
    FROM tax.TaxSlab
    WHERE TaxRegimeID = @TaxRegimeID AND TenantID = @TenantID AND IsDeleted = 0
    ORDER BY IncomeFrom;
    
    DECLARE @From DECIMAL(18,2), @To DECIMAL(18,2), @Rate DECIMAL(5,2);
    
    OPEN SlabCursor;
    FETCH NEXT FROM SlabCursor INTO @From, @To, @Rate;
    
    WHILE @@FETCH_STATUS = 0 AND @RemainingIncome > 0
    BEGIN
        DECLARE @Range DECIMAL(18,2) = 0;
        IF @To IS NULL
            SET @Range = @RemainingIncome;
        ELSE
            SET @Range = @To - @From;
            
        IF @RemainingIncome > @Range
        BEGIN
            SET @Tax = @Tax + (@Range * @Rate / 100.0);
            SET @RemainingIncome = @RemainingIncome - @Range;
        END
        ELSE
        BEGIN
            SET @Tax = @Tax + (@RemainingIncome * @Rate / 100.0);
            SET @RemainingIncome = 0;
        END
        
        FETCH NEXT FROM SlabCursor INTO @From, @To, @Rate;
    END
    
    CLOSE SlabCursor;
    DEALLOCATE SlabCursor;
    
    RETURN @Tax;
END