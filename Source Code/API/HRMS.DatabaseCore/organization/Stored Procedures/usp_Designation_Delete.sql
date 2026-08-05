
CREATE   PROCEDURE organization.usp_Designation_Delete
    @DesignationID BIGINT,
    @TenantID BIGINT,
    @DeletedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE organization.Designation
        SET IsDeleted = 1, DeletedBy = @DeletedBy, DeletedDate = GETUTCDATE()
        WHERE DesignationID = @DesignationID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;