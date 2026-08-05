
CREATE PROCEDURE offboarding.usp_FullAndFinalSettlement_Close
    @FullAndFinalSettlementID BIGINT,
    @TenantID                 BIGINT,
    @ModifiedBy               BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE offboarding.FullAndFinalSettlement
        SET SettlementStatus = 'Closed',
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE FullAndFinalSettlementID = @FullAndFinalSettlementID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END