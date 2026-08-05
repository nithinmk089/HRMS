
CREATE FUNCTION attendance.fn_CalculateLateMinutes
(
    @ClockInTime DATETIME2,
    @ShiftStartTime TIME,
    @GraceInMinutes INT
)
RETURNS INT
AS
BEGIN
    IF @ClockInTime IS NULL OR @ShiftStartTime IS NULL RETURN 0;
    
    DECLARE @ClockInTimeOnly TIME = CAST(@ClockInTime AS TIME);
    DECLARE @Diff INT = DATEDIFF(minute, @ShiftStartTime, @ClockInTimeOnly);
    
    IF @Diff > @GraceInMinutes
        RETURN @Diff;
        
    RETURN 0;
END;