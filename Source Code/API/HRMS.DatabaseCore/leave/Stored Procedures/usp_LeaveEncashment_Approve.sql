
CREATE PROCEDURE leave.usp_LeaveEncashment_Approve
    @LeaveEncashmentID BIGINT,
    @TenantID BIGINT,
    @ApproverID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @EmployeeID BIGINT, @LeaveTypeID BIGINT, @EncashedDays DECIMAL(5,2);
        SELECT @EmployeeID = EmployeeID, @LeaveTypeID = LeaveTypeID, @EncashedDays = EncashedDays
        FROM leave.LeaveEncashment
        WHERE LeaveEncashmentID = @LeaveEncashmentID AND TenantID = @TenantID AND IsDeleted = 0;

        UPDATE leave.LeaveEncashment
        SET [Status] = 'Approved',
            ModifiedBy = @ApproverID,
            ModifiedDate = GETUTCDATE()
        WHERE LeaveEncashmentID = @LeaveEncashmentID AND TenantID = @TenantID AND IsDeleted = 0;

        -- Deduct from balance
        UPDATE leave.LeaveBalance
        SET ConsumedBalance = ConsumedBalance + @EncashedDays,
            AvailableBalance = AvailableBalance - @EncashedDays,
            ModifiedBy = @ApproverID,
            ModifiedDate = GETUTCDATE()
        WHERE TenantID = @TenantID AND EmployeeID = @EmployeeID AND LeaveTypeID = @LeaveTypeID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_LeaveEncashment_Approve', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;