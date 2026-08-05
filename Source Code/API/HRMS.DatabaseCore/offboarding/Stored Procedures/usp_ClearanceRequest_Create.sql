
CREATE PROCEDURE offboarding.usp_ClearanceRequest_Create
    @TenantID           BIGINT,
    @EmployeeID         BIGINT,
    @InitiatedDate      DATE,
    @CreatedBy          BIGINT,
    @ClearanceRequestID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO offboarding.ClearanceRequest (
            TenantID, EmployeeID, ClearanceStatus, InitiatedDate, CreatedBy
        )
        VALUES (
            @TenantID, @EmployeeID, 'Pending', @InitiatedDate, @CreatedBy
        );

        SET @ClearanceRequestID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END