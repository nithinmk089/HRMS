
CREATE FUNCTION hr.fn_GetCertificationStatus
(
    @ExpiryDate DATE
)
RETURNS NVARCHAR(50)
AS
BEGIN
    DECLARE @Status NVARCHAR(50);
    IF @ExpiryDate IS NULL
        SET @Status = 'Active';
    ELSE IF @ExpiryDate < CAST(GETUTCDATE() AS DATE)
        SET @Status = 'Expired';
    ELSE IF DATEDIFF(day, CAST(GETUTCDATE() AS DATE), @ExpiryDate) <= 30
        SET @Status = 'Expiring Soon';
    ELSE
        SET @Status = 'Active';
    RETURN @Status;
END;