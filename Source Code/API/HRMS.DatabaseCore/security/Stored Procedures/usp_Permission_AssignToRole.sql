
CREATE   PROCEDURE security.usp_Permission_AssignToRole
    @TenantID BIGINT,
    @RoleID BIGINT,
    @PermissionID BIGINT,
    @CreatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS(SELECT 1 FROM security.RolePermission WHERE RoleID = @RoleID AND PermissionID = @PermissionID AND IsDeleted = 0)
        BEGIN
            INSERT INTO security.RolePermission (TenantID, RoleID, PermissionID, CreatedBy)
            VALUES (@TenantID, @RoleID, @PermissionID, @CreatedBy);
        END
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;