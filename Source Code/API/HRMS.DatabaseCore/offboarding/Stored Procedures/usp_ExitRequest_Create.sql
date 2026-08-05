
-- ==========================================
-- 7. OFFBOARDING STORED PROCEDURES
-- ==========================================

CREATE PROCEDURE offboarding.usp_ExitRequest_Create
    @TenantID        BIGINT,
    @EmployeeID      BIGINT,
    @ResignationDate DATE,
    @LastWorkingDate DATE,
    @ExitReason      NVARCHAR(500),
    @CreatedBy       BIGINT,
    @ExitRequestID   BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO offboarding.ExitRequest (
            TenantID, EmployeeID, ResignationDate, LastWorkingDate, ExitReason, Status, CreatedBy
        )
        VALUES (
            @TenantID, @EmployeeID, @ResignationDate, @LastWorkingDate, @ExitReason, 'Pending', @CreatedBy
        );

        SET @ExitRequestID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END