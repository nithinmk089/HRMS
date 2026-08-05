
CREATE PROCEDURE leave.usp_LeaveBalance_Recalculate
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @LeaveTypeID BIGINT,
    @AccruedBalance DECIMAL(5,2),
    @ConsumedBalance DECIMAL(5,2),
    @AvailableBalance DECIMAL(5,2),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF EXISTS (SELECT 1 FROM leave.LeaveBalance WHERE TenantID = @TenantID AND EmployeeID = @EmployeeID AND LeaveTypeID = @LeaveTypeID AND IsDeleted = 0)
        BEGIN
            UPDATE leave.LeaveBalance
            SET AccruedBalance = @AccruedBalance,
                ConsumedBalance = @ConsumedBalance,
                AvailableBalance = @AvailableBalance,
                ModifiedBy = @ModifiedBy,
                ModifiedDate = GETUTCDATE()
            WHERE TenantID = @TenantID AND EmployeeID = @EmployeeID AND LeaveTypeID = @LeaveTypeID AND IsDeleted = 0;
        END
        ELSE
        BEGIN
            INSERT INTO leave.LeaveBalance (
                TenantID, EmployeeID, LeaveTypeID, OpeningBalance, AccruedBalance, ConsumedBalance, AvailableBalance, CreatedBy, CreatedDate, IsDeleted
            )
            VALUES (
                @TenantID, @EmployeeID, @LeaveTypeID, 0, @AccruedBalance, @ConsumedBalance, @AvailableBalance, @ModifiedBy, GETUTCDATE(), 0
            );
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_LeaveBalance_Recalculate', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;