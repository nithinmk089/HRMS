
CREATE PROCEDURE leave.usp_LeaveBalance_Adjust
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @LeaveTypeID BIGINT,
    @AdjustmentAmount DECIMAL(5,2),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE leave.LeaveBalance
        SET AvailableBalance = AvailableBalance + @AdjustmentAmount,
            AccruedBalance = AccruedBalance + @AdjustmentAmount,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE TenantID = @TenantID AND EmployeeID = @EmployeeID AND LeaveTypeID = @LeaveTypeID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_LeaveBalance_Adjust', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;