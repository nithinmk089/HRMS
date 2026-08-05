
CREATE FUNCTION attendance.fn_CalculateAttendanceStatus
(
    @ClockInTime DATETIME2,
    @ClockOutTime DATETIME2,
    @ShiftStartTime TIME,
    @ShiftEndTime TIME,
    @GraceInMinutes INT,
    @GraceOutMinutes INT
)
RETURNS NVARCHAR(50)
AS
BEGIN
    IF @ClockInTime IS NULL RETURN 'Absent';
    
    IF @ClockOutTime IS NULL RETURN 'ClockedIn';
    
    DECLARE @ClockInTimeOnly TIME = CAST(@ClockInTime AS TIME);
    DECLARE @ClockOutTimeOnly TIME = CAST(@ClockOutTime AS TIME);
    
    DECLARE @Late BIT = 0;
    DECLARE @EarlyOut BIT = 0;
    
    IF DATEDIFF(minute, @ShiftStartTime, @ClockInTimeOnly) > @GraceInMinutes
        SET @Late = 1;
        
    IF DATEDIFF(minute, @ClockOutTimeOnly, @ShiftEndTime) > @GraceOutMinutes
        SET @EarlyOut = 1;
        
    IF @Late = 1 AND @EarlyOut = 1
        RETURN 'Late & EarlyOut';
    IF @Late = 1
        RETURN 'Late';
    IF @EarlyOut = 1
        RETURN 'EarlyOut';
        
    RETURN 'Present';
END;