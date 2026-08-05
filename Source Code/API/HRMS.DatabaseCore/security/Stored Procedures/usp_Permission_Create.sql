
-- STORED PROCEDURES - PERMISSION
CREATE   PROCEDURE security.usp_Permission_Create
    @TenantID BIGINT,
    @PermissionCode VARCHAR(100),
    @PermissionName VARCHAR(200),
    @ModuleCode VARCHAR(100),
    @CreatedBy BIGINT,
    @PermissionID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO security.[Permission] (TenantID, PermissionCode, PermissionName, ModuleCode, CreatedBy)
        VALUES (@TenantID, @PermissionCode, @PermissionName, @ModuleCode, @CreatedBy);
        SET @PermissionID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;