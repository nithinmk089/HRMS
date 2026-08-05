CREATE PROCEDURE security.usp_User_Update
    @UserID BIGINT,
    @TenantID BIGINT,
    @EmployeeID BIGINT = NULL,
    @Email VARCHAR(200),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM security.[User] WHERE UserID = @UserID AND IsDeleted = 0)
        BEGIN
            RAISERROR('User account not found.', 16, 1);
            RETURN;
        END

        IF EXISTS (
            SELECT 1 
            FROM security.[User] 
            WHERE Email = @Email 
              AND UserID <> @UserID 
              AND IsDeleted = 0
        )
        BEGIN
            RAISERROR('Email address ''%s'' is already in use by another active user account.', 16, 1, @Email);
            RETURN;
        END

        BEGIN TRANSACTION;
        UPDATE security.[User]
        SET TenantID = @TenantID,
            EmployeeID = @EmployeeID,
            Email = @Email,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE(),
            VersionNo = VersionNo + 1
        WHERE UserID = @UserID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;