
CREATE   PROCEDURE security.usp_Role_AssignToUser
    @TenantID BIGINT,
    @UserID BIGINT,
    @RoleID BIGINT,
    @CreatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS(SELECT 1 FROM security.UserRole WHERE UserID = @UserID AND RoleID = @RoleID AND IsDeleted = 0)
        BEGIN
            INSERT INTO security.UserRole (TenantID, UserID, RoleID, CreatedBy)
            VALUES (@TenantID, @UserID, @RoleID, @CreatedBy);
        END
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;