
CREATE VIEW security.vw_RolePermissions
AS
SELECT rp.RolePermissionID, rp.TenantID, rp.RoleID, rp.PermissionID, r.RoleCode, r.RoleName, p.PermissionCode, p.PermissionName, p.ModuleCode
FROM security.RolePermission rp
INNER JOIN security.[Role] r ON r.RoleID = rp.RoleID AND r.IsDeleted = 0
INNER JOIN security.[Permission] p ON p.PermissionID = rp.PermissionID AND p.IsDeleted = 0
WHERE rp.IsDeleted = 0;