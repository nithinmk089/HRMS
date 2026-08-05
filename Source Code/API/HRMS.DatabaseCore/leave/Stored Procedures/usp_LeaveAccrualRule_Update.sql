
CREATE PROCEDURE leave.usp_LeaveAccrualRule_Update
    @LeaveAccrualRuleID BIGINT,
    @TenantID BIGINT,
    @AccrualFrequency NVARCHAR(50),
    @AccrualAmount DECIMAL(5,2),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE leave.LeaveAccrualRule
        SET AccrualFrequency = @AccrualFrequency,
            AccrualAmount = @AccrualAmount,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE LeaveAccrualRuleID = @LeaveAccrualRuleID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_LeaveAccrualRule_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;