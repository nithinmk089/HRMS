
CREATE   PROCEDURE security.usp_User_BulkExport
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT UserID, UserName, Email, IsLocked, LastLoginDate
    FROM security.[User]
    WHERE TenantID = @TenantID AND IsDeleted = 0
    FOR JSON PATH;
END;