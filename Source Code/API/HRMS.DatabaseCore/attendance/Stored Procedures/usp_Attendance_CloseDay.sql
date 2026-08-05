
CREATE PROCEDURE attendance.usp_Attendance_CloseDay
    @TenantID BIGINT,
    @CloseDate DATE,
    @CreatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 1;
END;