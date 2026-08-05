
-- asset.usp_AssetReturn_Verify.sql
CREATE PROCEDURE asset.usp_AssetReturn_Verify
    @AssetReturnID BIGINT,
    @TenantID BIGINT,
    @ReturnCondition NVARCHAR(500) = NULL,
    @VerifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE asset.AssetReturn
        SET ReturnStatus = 'Verified',
            ReturnCondition = COALESCE(@ReturnCondition, ReturnCondition),
            ModifiedBy = @VerifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE AssetReturnID = @AssetReturnID AND TenantID = @TenantID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;