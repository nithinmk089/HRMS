
CREATE PROCEDURE attendance.usp_Attendance_GetById
    @AttendanceID BIGINT,
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM attendance.vw_AttendanceDaily 
    WHERE AttendanceID = @AttendanceID AND TenantID = @TenantID AND IsDeleted = 0;
END;