
CREATE PROCEDURE attendance.usp_ShiftRotation_Assign
    @TenantID BIGINT,
    @ShiftRotationID BIGINT,
    @EmployeeID BIGINT,
    @CreatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    -- Just a stub procedure as rotation assignment design is extensible
    SELECT 1;
END;