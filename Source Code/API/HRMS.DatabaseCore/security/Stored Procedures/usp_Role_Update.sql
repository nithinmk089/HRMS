
CREATE   PROCEDURE security.usp_Role_Update
    @RoleID BIGINT,
    @TenantID BIGINT,
    @RoleName VARCHAR(200),
    @Description VARCHAR(500) = NULL,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE security.[Role]
        SET RoleName = @RoleName,
            [Description] = @Description,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE(),
            VersionNo = VersionNo + 1
        WHERE RoleID = @RoleID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;