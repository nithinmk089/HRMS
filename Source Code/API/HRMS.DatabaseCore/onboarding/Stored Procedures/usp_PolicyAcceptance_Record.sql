
CREATE PROCEDURE onboarding.usp_PolicyAcceptance_Record
    @TenantID           BIGINT,
    @EmployeeID         BIGINT,
    @PolicyID           BIGINT,
    @AcceptanceVersion  NVARCHAR(50),
    @CreatedBy          BIGINT,
    @PolicyAcceptanceID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO onboarding.PolicyAcceptance (
            TenantID, EmployeeID, PolicyID, AcceptedDate, AcceptanceVersion, CreatedBy
        )
        VALUES (
            @TenantID, @EmployeeID, @PolicyID, GETUTCDATE(), @AcceptanceVersion, @CreatedBy
        );

        SET @PolicyAcceptanceID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END