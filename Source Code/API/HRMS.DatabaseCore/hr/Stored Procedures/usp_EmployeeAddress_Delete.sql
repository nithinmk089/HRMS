
CREATE PROCEDURE hr.usp_EmployeeAddress_Delete
    @EmployeeAddressID BIGINT,
    @TenantID BIGINT,
    @DeletedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE hr.EmployeeAddress
        SET IsDeleted = 1,
            DeletedBy = @DeletedBy,
            DeletedDate = GETUTCDATE()
        WHERE EmployeeAddressID = @EmployeeAddressID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeAddress_Delete', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;