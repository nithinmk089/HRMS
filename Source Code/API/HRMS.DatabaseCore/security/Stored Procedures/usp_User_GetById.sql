
CREATE   PROCEDURE security.usp_User_GetById
    @UserID BIGINT,
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT UserID, TenantID, EmployeeID, UserName, Email, PasswordHash, PasswordSalt, IsLocked, LastLoginDate, VersionNo
    FROM security.[User]
    WHERE UserID = @UserID AND TenantID = @TenantID AND IsDeleted = 0;
END;