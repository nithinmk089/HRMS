
CREATE   PROCEDURE security.usp_Company_Update
    @CompanyID BIGINT,
    @TenantID BIGINT,
    @CompanyName VARCHAR(200),
    @LegalName VARCHAR(300) = NULL,
    @TaxNumber VARCHAR(100) = NULL,
    @Email VARCHAR(200) = NULL,
    @Phone VARCHAR(50) = NULL,
    @Website VARCHAR(200) = NULL,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE security.Company
        SET CompanyName = @CompanyName,
            LegalName = @LegalName,
            TaxNumber = @TaxNumber,
            Email = @Email,
            Phone = @Phone,
            Website = @Website,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE(),
            VersionNo = VersionNo + 1
        WHERE CompanyID = @CompanyID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;