
-- payroll.usp_PayrollCalendar_Create.sql
CREATE PROCEDURE payroll.usp_PayrollCalendar_Create
    @TenantID BIGINT,
    @CalendarCode NVARCHAR(50),
    @CalendarName NVARCHAR(100),
    @FinancialYear NVARCHAR(20),
    @CreatedBy BIGINT,
    @PayrollCalendarID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    INSERT INTO payroll.PayrollCalendar (TenantID, CalendarCode, CalendarName, FinancialYear, IsActive, CreatedBy)
    VALUES (@TenantID, @CalendarCode, @CalendarName, @FinancialYear, 1, @CreatedBy);
    
    SET @PayrollCalendarID = SCOPE_IDENTITY();
END