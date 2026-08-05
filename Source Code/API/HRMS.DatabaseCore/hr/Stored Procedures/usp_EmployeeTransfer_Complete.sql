
CREATE PROCEDURE hr.usp_EmployeeTransfer_Complete
    @EmployeeTransferID BIGINT,
    @TenantID BIGINT,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @EmployeeID BIGINT, @ToDeptID BIGINT, @ToLocID BIGINT;

        SELECT @EmployeeID = EmployeeID, @ToDeptID = ToDepartmentID, @ToLocID = ToLocationID
        FROM hr.EmployeeTransfer
        WHERE EmployeeTransferID = @EmployeeTransferID AND TenantID = @TenantID AND [Status] = 'Approved' AND IsDeleted = 0;

        IF @EmployeeID IS NOT NULL
        BEGIN
            UPDATE hr.EmployeeTransfer
            SET [Status] = 'Completed',
                ModifiedBy = @ModifiedBy,
                ModifiedDate = GETUTCDATE()
            WHERE EmployeeTransferID = @EmployeeTransferID;

            UPDATE hr.EmployeeEmployment
            SET DepartmentID = @ToDeptID,
                LocationID = @ToLocID,
                ModifiedBy = @ModifiedBy,
                ModifiedDate = GETUTCDATE()
            WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeTransfer_Complete', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;