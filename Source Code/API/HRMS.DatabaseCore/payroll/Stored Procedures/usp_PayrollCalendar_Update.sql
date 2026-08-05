
-- payroll.usp_PayrollCalendar_Update.sql
CREATE PROCEDURE payroll.usp_PayrollCalendar_Update
    @PayrollCalendarID BIGINT,
    @TenantID BIGINT,
    @CalendarName NVARCHAR(100),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    UPDATE payroll.PayrollCalendar
    SET CalendarName = @CalendarName,
        ModifiedBy = @ModifiedBy,
        ModifiedDate = GETUTCDATE()
    WHERE PayrollCalendarID = @PayrollCalendarID AND TenantID = @TenantID AND IsDeleted = 0;
END