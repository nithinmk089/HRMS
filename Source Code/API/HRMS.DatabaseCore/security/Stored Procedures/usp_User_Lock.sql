
CREATE   PROCEDURE security.usp_User_Lock
    @UserID BIGINT,
    @TenantID BIGINT,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE security.[User]
        SET IsLocked = 1, ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE(), VersionNo = VersionNo + 1
        WHERE UserID = @UserID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;