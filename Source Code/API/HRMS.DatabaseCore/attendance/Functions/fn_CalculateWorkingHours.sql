
-- === 3. FUNCTIONS ===
CREATE FUNCTION attendance.fn_CalculateWorkingHours
(
    @ClockInTime DATETIME2,
    @ClockOutTime DATETIME2
)
RETURNS INT
AS
BEGIN
    IF @ClockInTime IS NULL OR @ClockOutTime IS NULL RETURN NULL;
    RETURN DATEDIFF(minute, @ClockInTime, @ClockOutTime);
END;