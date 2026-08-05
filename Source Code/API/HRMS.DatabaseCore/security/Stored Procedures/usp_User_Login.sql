
CREATE   PROCEDURE security.usp_User_Login
    @Email VARCHAR(200),
    @IPAddress VARCHAR(50),
    @BrowserInfo VARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @UserID BIGINT;
    DECLARE @TenantID BIGINT;
    DECLARE @IsLocked BIT;
    DECLARE @PasswordHash VARCHAR(500);
    DECLARE @PasswordSalt VARCHAR(500);

    -- Find user
    SELECT 
        @UserID = UserID,
        @TenantID = TenantID,
        @IsLocked = IsLocked,
        @PasswordHash = PasswordHash,
        @PasswordSalt = PasswordSalt
    FROM security.[User]
    WHERE (Email = @Email OR UserName = @Email) AND IsDeleted = 0;

    IF @UserID IS NULL
    BEGIN
        SELECT 0 AS Success, 'Invalid email/username or password.' AS [Message];
        RETURN;
    END

    IF @IsLocked = 1
    BEGIN
        SELECT 0 AS Success, 'User account is locked.' AS [Message];
        RETURN;
    END

    -- Return user row (Success = 1)
    SELECT 
        1 AS Success,
        'Login successful.' AS [Message],
        u.UserID,
        u.Email,
        u.UserName,
        'Admin' AS FirstName,
        'Operator' AS LastName,
        1 AS OrganizationID,
        u.TenantID,
        u.PasswordHash,
        u.PasswordSalt
    FROM security.[User] u
    WHERE u.UserID = @UserID;

    -- Return roles
    SELECT r.RoleCode
    FROM security.UserRole ur
    INNER JOIN security.Role r ON ur.RoleID = r.RoleID
    WHERE ur.UserID = @UserID AND ur.IsDeleted = 0 AND r.IsDeleted = 0;

    -- Return permissions
    SELECT p.PermissionCode
    FROM security.UserPermission up
    INNER JOIN security.Permission p ON up.PermissionID = p.PermissionID
    WHERE up.UserID = @UserID AND up.IsAllowed = 1 AND up.IsDeleted = 0 AND p.IsDeleted = 0
    UNION
    SELECT p.PermissionCode
    FROM security.UserRole ur
    INNER JOIN security.RolePermission rp ON ur.RoleID = rp.RoleID
    INNER JOIN security.Permission p ON rp.PermissionID = p.PermissionID
    WHERE ur.UserID = @UserID AND ur.IsDeleted = 0 AND rp.IsDeleted = 0 AND p.IsDeleted = 0;

    -- Return companies
    SELECT CompanyID, CompanyCode, CompanyName, CAST(1 AS BIT) AS IsDefault
    FROM security.Company
    WHERE TenantID = @TenantID AND IsDeleted = 0;

    -- Log login success to AuditLog
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    VALUES (@TenantID, 'User', @UserID, 'LOGIN', NULL, '{"IP":"' + @IPAddress + '","Browser":"' + @BrowserInfo + '"}', @UserID);

    -- Update LastLoginDate
    UPDATE security.[User]
    SET LastLoginDate = GETUTCDATE()
    WHERE UserID = @UserID;
END;