
CREATE PROCEDURE offboarding.usp_KnowledgeTransfer_Create
    @TenantID            BIGINT,
    @EmployeeID          BIGINT,
    @SuccessorEmployeeID BIGINT,
    @KTDate              DATE,
    @CreatedBy           BIGINT,
    @KnowledgeTransferID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO offboarding.KnowledgeTransfer (
            TenantID, EmployeeID, SuccessorEmployeeID, KTDate, KTStatus, CreatedBy
        )
        VALUES (
            @TenantID, @EmployeeID, @SuccessorEmployeeID, @KTDate, 'Pending', @CreatedBy
        );

        SET @KnowledgeTransferID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END