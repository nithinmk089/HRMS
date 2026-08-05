
CREATE   PROCEDURE security.usp_Permission_AssignToUser
    @TenantID BIGINT,
    @UserID BIGINT,
    @PermissionID BIGINT,
    @IsAllowed BIT,
    @CreatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF EXISTS(SELECT 1 FROM security.UserPermission WHERE UserID = @UserID AND PermissionID = @PermissionID AND TenantID = @TenantID AND IsDeleted = 0)
        BEGIN
            UPDATE security.UserPermission
            SET IsAllowed = @IsAllowed, ModifiedBy = @CreatedBy, ModifiedDate = GETUTCDATE(), VersionNo = VersionNo + 1
            WHERE UserID = @UserID AND PermissionID = @PermissionID AND TenantID = @TenantID AND IsDeleted = 0;
        END
        ELSE
        BEGIN
            INSERT INTO security.UserPermission (TenantID, UserID, PermissionID, IsAllowed, CreatedBy)
            VALUES (@TenantID, @UserID, @PermissionID, @IsAllowed, @CreatedBy);
        END
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;