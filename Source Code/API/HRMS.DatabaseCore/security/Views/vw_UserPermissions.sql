
CREATE VIEW security.vw_UserPermissions
AS
SELECT DISTINCT ur.TenantID, ur.UserID, u.UserName, p.PermissionID, p.PermissionCode, p.PermissionName, p.ModuleCode, 'Role' AS GrantedVia
FROM security.UserRole ur
INNER JOIN security.[User] u ON u.UserID = ur.UserID AND u.IsDeleted = 0
INNER JOIN security.RolePermission rp ON rp.RoleID = ur.RoleID AND rp.IsDeleted = 0
INNER JOIN security.[Permission] p ON p.PermissionID = rp.PermissionID AND p.IsDeleted = 0
WHERE ur.IsDeleted = 0
UNION
SELECT up.TenantID, up.UserID, u.UserName, p.PermissionID, p.PermissionCode, p.PermissionName, p.ModuleCode, 'Direct' AS GrantedVia
FROM security.UserPermission up
INNER JOIN security.[User] u ON u.UserID = up.UserID AND u.IsDeleted = 0
INNER JOIN security.[Permission] p ON p.PermissionID = up.PermissionID AND p.IsDeleted = 0
WHERE up.IsDeleted = 0 AND up.IsAllowed = 1;