
-- payroll.usp_PayrollCalendar_Delete.sql
CREATE PROCEDURE payroll.usp_PayrollCalendar_Delete
    @PayrollCalendarID BIGINT,
    @TenantID BIGINT,
    @DeletedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    UPDATE payroll.PayrollCalendar
    SET IsDeleted = 1,
        DeletedBy = @DeletedBy,
        DeletedDate = GETUTCDATE()
    WHERE PayrollCalendarID = @PayrollCalendarID AND TenantID = @TenantID;
END