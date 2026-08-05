
-- payroll.usp_PayrollCalendar_Search.sql
CREATE PROCEDURE payroll.usp_PayrollCalendar_Search
    @TenantID BIGINT,
    @SearchText NVARCHAR(100) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT COUNT(*) FROM payroll.PayrollCalendar
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR CalendarName LIKE '%' + @SearchText + '%' OR CalendarCode LIKE '%' + @SearchText + '%');
      
    SELECT * FROM payroll.PayrollCalendar
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR CalendarName LIKE '%' + @SearchText + '%' OR CalendarCode LIKE '%' + @SearchText + '%')
    ORDER BY PayrollCalendarID DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
END