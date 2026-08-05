
CREATE PROCEDURE leave.usp_LeaveType_Search
    @TenantID BIGINT,
    @SearchTerm NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM leave.LeaveType
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchTerm IS NULL OR LeaveCode LIKE '%' + @SearchTerm + '%' OR LeaveName LIKE '%' + @SearchTerm + '%');
END;