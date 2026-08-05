
-- STORED PROCEDURES - USER
CREATE   PROCEDURE security.usp_User_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT = NULL,
    @UserName VARCHAR(100),
    @Email VARCHAR(200),
    @PasswordHash VARCHAR(500),
    @PasswordSalt VARCHAR(500),
    @CreatedBy BIGINT,
    @UserID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO security.[User] (TenantID, EmployeeID, UserName, Email, PasswordHash, PasswordSalt, CreatedBy)
        VALUES (@TenantID, @EmployeeID, @UserName, @Email, @PasswordHash, @PasswordSalt, @CreatedBy);
        SET @UserID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;