
CREATE PROCEDURE leave.usp_LeavePolicy_Update
    @LeavePolicyID BIGINT,
    @TenantID BIGINT,
    @PolicyName NVARCHAR(100),
    @LeaveTypeID BIGINT,
    @EffectiveFrom DATE,
    @EffectiveTo DATE = NULL,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE leave.LeavePolicy
        SET PolicyName = @PolicyName,
            LeaveTypeID = @LeaveTypeID,
            EffectiveFrom = @EffectiveFrom,
            EffectiveTo = @EffectiveTo,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE LeavePolicyID = @LeavePolicyID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_LeavePolicy_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;