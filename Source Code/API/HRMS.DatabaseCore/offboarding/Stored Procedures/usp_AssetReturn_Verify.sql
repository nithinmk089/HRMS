
CREATE PROCEDURE offboarding.usp_AssetReturn_Verify
    @AssetReturnID   BIGINT,
    @TenantID        BIGINT,
    @ReturnCondition NVARCHAR(100),
    @ModifiedBy      BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE offboarding.AssetReturn
        SET ReturnCondition = @ReturnCondition,
            ReturnDate = CAST(GETUTCDATE() AS DATE),
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE AssetReturnID = @AssetReturnID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END