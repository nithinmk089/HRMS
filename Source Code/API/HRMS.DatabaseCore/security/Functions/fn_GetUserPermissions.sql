
-- 4. FUNCTIONS
CREATE   FUNCTION security.fn_GetUserPermissions
(
    @UserID BIGINT
)
RETURNS TABLE
AS
RETURN
(
    SELECT DISTINCT PermissionCode, PermissionName, ModuleCode
    FROM security.vw_UserPermissions
    WHERE UserID = @UserID
);