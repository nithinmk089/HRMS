
CREATE   PROCEDURE security.usp_Permission_Update
    @PermissionID BIGINT,
    @TenantID BIGINT,
    @PermissionName VARCHAR(200),
    @ModuleCode VARCHAR(100),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE security.[Permission]
        SET PermissionName = @PermissionName,
            ModuleCode = @ModuleCode,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE(),
            VersionNo = VersionNo + 1
        WHERE PermissionID = @PermissionID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;