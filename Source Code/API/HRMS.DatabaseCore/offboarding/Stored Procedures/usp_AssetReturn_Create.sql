
CREATE PROCEDURE offboarding.usp_AssetReturn_Create
    @TenantID      BIGINT,
    @EmployeeID    BIGINT,
    @AssetID       BIGINT,
    @ReturnDate    DATE,
    @CreatedBy     BIGINT,
    @AssetReturnID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO offboarding.AssetReturn (
            TenantID, EmployeeID, AssetID, ReturnDate, ReturnCondition, CreatedBy
        )
        VALUES (
            @TenantID, @EmployeeID, @AssetID, @ReturnDate, 'Good', @CreatedBy
        );

        SET @AssetReturnID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END