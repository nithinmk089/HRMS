
CREATE PROCEDURE leave.usp_LeaveAccrualRule_Create
    @TenantID BIGINT,
    @LeavePolicyID BIGINT,
    @AccrualFrequency NVARCHAR(50),
    @AccrualAmount DECIMAL(5,2),
    @CreatedBy BIGINT,
    @LeaveAccrualRuleID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO leave.LeaveAccrualRule (
            TenantID, LeavePolicyID, AccrualFrequency, AccrualAmount, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @LeavePolicyID, @AccrualFrequency, @AccrualAmount, @CreatedBy, GETUTCDATE(), 0
        );
        SET @LeaveAccrualRuleID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_LeaveAccrualRule_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;