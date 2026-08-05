
-- STORED PROCEDURES - COMPANY
CREATE   PROCEDURE security.usp_Company_Create
    @TenantID BIGINT,
    @CompanyCode VARCHAR(50),
    @CompanyName VARCHAR(200),
    @LegalName VARCHAR(300) = NULL,
    @TaxNumber VARCHAR(100) = NULL,
    @Email VARCHAR(200) = NULL,
    @Phone VARCHAR(50) = NULL,
    @Website VARCHAR(200) = NULL,
    @CreatedBy BIGINT,
    @CompanyID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO security.Company (TenantID, CompanyCode, CompanyName, LegalName, TaxNumber, Email, Phone, Website, CreatedBy)
        VALUES (@TenantID, @CompanyCode, @CompanyName, @LegalName, @TaxNumber, @Email, @Phone, @Website, @CreatedBy);
        SET @CompanyID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;