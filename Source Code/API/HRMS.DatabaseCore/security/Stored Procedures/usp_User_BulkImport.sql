
CREATE   PROCEDURE security.usp_User_BulkImport
    @TenantID BIGINT,
    @CreatedBy BIGINT,
    @UsersJSON NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        INSERT INTO security.[User] (TenantID, UserName, Email, PasswordHash, PasswordSalt, CreatedBy)
        SELECT 
            @TenantID,
            JSON_VALUE(val.value, '$.userName'),
            JSON_VALUE(val.value, '$.email'),
            JSON_VALUE(val.value, '$.passwordHash'),
            JSON_VALUE(val.value, '$.passwordSalt'),
            @CreatedBy
        FROM OPENJSON(@UsersJSON) val;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;