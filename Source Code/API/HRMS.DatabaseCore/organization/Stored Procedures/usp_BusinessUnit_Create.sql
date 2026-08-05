
-- STORED PROCEDURES - BUSINESS UNIT
CREATE   PROCEDURE organization.usp_BusinessUnit_Create
    @TenantID BIGINT,
    @CompanyID BIGINT,
    @BusinessUnitCode VARCHAR(50),
    @BusinessUnitName VARCHAR(200),
    @CreatedBy BIGINT,
    @BusinessUnitID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO organization.BusinessUnit (TenantID, CompanyID, BusinessUnitCode, BusinessUnitName, CreatedBy)
        VALUES (@TenantID, @CompanyID, @BusinessUnitCode, @BusinessUnitName, @CreatedBy);
        SET @BusinessUnitID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;