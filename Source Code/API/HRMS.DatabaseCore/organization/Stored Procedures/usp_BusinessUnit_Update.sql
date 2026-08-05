
CREATE   PROCEDURE organization.usp_BusinessUnit_Update
    @BusinessUnitID BIGINT,
    @TenantID BIGINT,
    @BusinessUnitName VARCHAR(200),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE organization.BusinessUnit
        SET BusinessUnitName = @BusinessUnitName,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE(),
            VersionNo = VersionNo + 1
        WHERE BusinessUnitID = @BusinessUnitID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;