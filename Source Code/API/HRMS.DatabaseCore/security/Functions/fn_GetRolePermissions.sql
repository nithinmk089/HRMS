
CREATE   FUNCTION security.fn_GetRolePermissions
(
    @RoleID BIGINT
)
RETURNS TABLE
AS
RETURN
(
    SELECT DISTINCT PermissionCode, PermissionName, ModuleCode
    FROM security.vw_RolePermissions
    WHERE RoleID = @RoleID
);