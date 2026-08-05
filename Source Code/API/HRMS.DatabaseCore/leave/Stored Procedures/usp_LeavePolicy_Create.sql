
CREATE PROCEDURE leave.usp_LeavePolicy_Create
    @TenantID BIGINT,
    @PolicyName NVARCHAR(100),
    @LeaveTypeID BIGINT,
    @EffectiveFrom DATE,
    @EffectiveTo DATE = NULL,
    @CreatedBy BIGINT,
    @LeavePolicyID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO leave.LeavePolicy (
            TenantID, PolicyName, LeaveTypeID, EffectiveFrom, EffectiveTo, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @PolicyName, @LeaveTypeID, @EffectiveFrom, @EffectiveTo, @CreatedBy, GETUTCDATE(), 0
        );
        SET @LeavePolicyID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_LeavePolicy_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;