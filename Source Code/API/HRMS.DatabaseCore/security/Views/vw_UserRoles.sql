
-- 3. VIEWS
CREATE VIEW security.vw_UserRoles
AS
SELECT ur.UserRoleID, ur.TenantID, ur.UserID, ur.RoleID, u.UserName, r.RoleCode, r.RoleName
FROM security.UserRole ur
INNER JOIN security.[User] u ON u.UserID = ur.UserID AND u.IsDeleted = 0
INNER JOIN security.[Role] r ON r.RoleID = ur.RoleID AND r.IsDeleted = 0
WHERE ur.IsDeleted = 0;