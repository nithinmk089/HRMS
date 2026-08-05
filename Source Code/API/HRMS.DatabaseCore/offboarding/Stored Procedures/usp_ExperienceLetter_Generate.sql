
CREATE PROCEDURE offboarding.usp_ExperienceLetter_Generate
    @TenantID                  BIGINT,
    @EmployeeID                BIGINT,
    @CreatedBy                 BIGINT,
    @ExperienceLetterRequestID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO offboarding.ExperienceLetterRequest (
            TenantID, EmployeeID, RequestDate, GeneratedDate, Status, CreatedBy
        )
        VALUES (
            @TenantID, @EmployeeID, CAST(GETUTCDATE() AS DATE), CAST(GETUTCDATE() AS DATE), 'Generated', @CreatedBy
        );

        SET @ExperienceLetterRequestID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END