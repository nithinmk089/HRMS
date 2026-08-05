
CREATE   PROCEDURE security.usp_Company_Delete
    @CompanyID BIGINT,
    @TenantID BIGINT,
    @DeletedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE security.Company
        SET IsDeleted = 1, DeletedBy = @DeletedBy, DeletedDate = GETUTCDATE()
        WHERE CompanyID = @CompanyID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;