
CREATE FUNCTION hr.fn_GetCertificationExpiryDays
(
    @ExpiryDate DATE
)
RETURNS INT
AS
BEGIN
    DECLARE @Days INT;
    IF @ExpiryDate IS NULL RETURN NULL;
    SET @Days = DATEDIFF(day, CAST(GETUTCDATE() AS DATE), @ExpiryDate);
    RETURN @Days;
END;