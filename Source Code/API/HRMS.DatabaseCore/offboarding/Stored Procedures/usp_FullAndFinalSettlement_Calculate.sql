
CREATE PROCEDURE offboarding.usp_FullAndFinalSettlement_Calculate
    @TenantID                 BIGINT,
    @EmployeeID               BIGINT,
    @SettlementAmount         DECIMAL(18,2),
    @CreatedBy                BIGINT,
    @FullAndFinalSettlementID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO offboarding.FullAndFinalSettlement (
            TenantID, EmployeeID, SettlementAmount, SettlementDate, SettlementStatus, CreatedBy
        )
        VALUES (
            @TenantID, @EmployeeID, @SettlementAmount, CAST(GETUTCDATE() AS DATE), 'Pending', @CreatedBy
        );

        SET @FullAndFinalSettlementID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END