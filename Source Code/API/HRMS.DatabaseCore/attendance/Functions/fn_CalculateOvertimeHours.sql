
CREATE FUNCTION attendance.fn_CalculateOvertimeHours
(
    @ClockInTime DATETIME2,
    @ClockOutTime DATETIME2,
    @ShiftStartTime TIME,
    @ShiftEndTime TIME
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    IF @ClockInTime IS NULL OR @ClockOutTime IS NULL OR @ShiftStartTime IS NULL OR @ShiftEndTime IS NULL RETURN 0;
    
    DECLARE @WorkingMinutes INT = DATEDIFF(minute, @ClockInTime, @ClockOutTime);
    DECLARE @ShiftMinutes INT = DATEDIFF(minute, @ShiftStartTime, @ShiftEndTime);
    
    IF @ShiftEndTime < @ShiftStartTime
        SET @ShiftMinutes = DATEDIFF(minute, @ShiftStartTime, CAST('23:59:59' AS TIME)) + DATEDIFF(minute, CAST('00:00:00' AS TIME), @ShiftEndTime) + 1;
        
    IF @WorkingMinutes > @ShiftMinutes
        RETURN CAST((@WorkingMinutes - @ShiftMinutes) AS DECIMAL(5,2)) / 60.0;
        
    RETURN 0;
END;