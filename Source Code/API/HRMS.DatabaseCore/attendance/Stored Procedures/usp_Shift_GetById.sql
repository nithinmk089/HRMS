
CREATE PROCEDURE attendance.usp_Shift_GetById
    @ShiftID BIGINT,
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM attendance.Shift 
    WHERE ShiftID = @ShiftID AND TenantID = @TenantID AND IsDeleted = 0;
END;