
CREATE   PROCEDURE security.usp_User_ChangePassword
    @UserID BIGINT,
    @TenantID BIGINT,
    @NewPasswordHash VARCHAR(500),
    @NewPasswordSalt VARCHAR(500),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE security.[User]
        SET PasswordHash = @NewPasswordHash,
            PasswordSalt = @NewPasswordSalt,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE(),
            VersionNo = VersionNo + 1
        WHERE UserID = @UserID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;