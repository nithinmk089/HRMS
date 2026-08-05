
CREATE FUNCTION hr.fn_GetEmployeeTenure
(
    @JoiningDate DATE
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @Tenure DECIMAL(5,2);
    IF @JoiningDate IS NULL RETURN NULL;
    SET @Tenure = CAST(DATEDIFF(day, @JoiningDate, GETUTCDATE()) AS DECIMAL(5,2)) / 365.25;
    RETURN @Tenure;
END;