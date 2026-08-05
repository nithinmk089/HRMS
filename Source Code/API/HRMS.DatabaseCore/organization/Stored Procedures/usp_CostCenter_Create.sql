
-- STORED PROCEDURES - COST CENTER
CREATE   PROCEDURE organization.usp_CostCenter_Create
    @TenantID BIGINT,
    @CostCenterCode VARCHAR(50),
    @CostCenterName VARCHAR(200),
    @CreatedBy BIGINT,
    @CostCenterID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO organization.CostCenter (TenantID, CostCenterCode, CostCenterName, CreatedBy)
        VALUES (@TenantID, @CostCenterCode, @CostCenterName, @CreatedBy);
        SET @CostCenterID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;