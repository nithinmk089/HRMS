
CREATE PROCEDURE onboarding.usp_EquipmentProvisioning_Create
    @TenantID                BIGINT,
    @EmployeeID              BIGINT,
    @AssetID                 BIGINT,
    @ProvisionDate           DATE,
    @ReturnRequired          BIT,
    @CreatedBy               BIGINT,
    @EquipmentProvisioningID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO onboarding.EquipmentProvisioning (
            TenantID, EmployeeID, AssetID, ProvisionDate, ReturnRequired, CreatedBy
        )
        VALUES (
            @TenantID, @EmployeeID, @AssetID, @ProvisionDate, @ReturnRequired, @CreatedBy
        );

        SET @EquipmentProvisioningID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END