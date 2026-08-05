
CREATE   PROCEDURE organization.usp_CostCenter_Update
    @CostCenterID BIGINT,
    @TenantID BIGINT,
    @CostCenterName VARCHAR(200),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE organization.CostCenter
        SET CostCenterName = @CostCenterName,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE(),
            VersionNo = VersionNo + 1
        WHERE CostCenterID = @CostCenterID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;