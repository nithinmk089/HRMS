
-- STORED PROCEDURES - ROLE
CREATE   PROCEDURE security.usp_Role_Create
    @TenantID BIGINT,
    @RoleCode VARCHAR(50),
    @RoleName VARCHAR(200),
    @Description VARCHAR(500) = NULL,
    @CreatedBy BIGINT,
    @RoleID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO security.[Role] (TenantID, RoleCode, RoleName, [Description], CreatedBy)
        VALUES (@TenantID, @RoleCode, @RoleName, @Description, @CreatedBy);
        SET @RoleID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;