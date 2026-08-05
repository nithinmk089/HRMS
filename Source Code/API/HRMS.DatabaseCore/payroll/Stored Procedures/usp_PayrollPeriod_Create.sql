
-- payroll.usp_PayrollPeriod_Create.sql
CREATE PROCEDURE payroll.usp_PayrollPeriod_Create
    @TenantID BIGINT,
    @PayrollCalendarID BIGINT,
    @PeriodCode NVARCHAR(50),
    @PeriodStartDate DATE,
    @PeriodEndDate DATE,
    @CreatedBy BIGINT,
    @PayrollPeriodID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO payroll.PayrollPeriod (TenantID, PayrollCalendarID, PeriodCode, PeriodStartDate, PeriodEndDate, ProcessingStatus, CreatedBy)
    VALUES (@TenantID, @PayrollCalendarID, @PeriodCode, @PeriodStartDate, @PeriodEndDate, 'Open', @CreatedBy);
    
    SET @PayrollPeriodID = SCOPE_IDENTITY();
END