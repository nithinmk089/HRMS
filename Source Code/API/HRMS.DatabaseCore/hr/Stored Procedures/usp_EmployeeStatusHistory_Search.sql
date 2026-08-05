
CREATE PROCEDURE hr.usp_EmployeeStatusHistory_Search
    @TenantID BIGINT,
    @EmployeeID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * 
    FROM hr.EmployeeStatusHistory
    WHERE TenantID = @TenantID AND EmployeeID = @EmployeeID AND IsDeleted = 0
    ORDER BY EffectiveDate DESC, EmployeeStatusHistoryID DESC;
END;