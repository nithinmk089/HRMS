
CREATE PROCEDURE attendance.usp_Shift_Search
    @TenantID BIGINT,
    @SearchTerm NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM attendance.Shift
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchTerm IS NULL OR ShiftCode LIKE '%' + @SearchTerm + '%' OR ShiftName LIKE '%' + @SearchTerm + '%');
END;