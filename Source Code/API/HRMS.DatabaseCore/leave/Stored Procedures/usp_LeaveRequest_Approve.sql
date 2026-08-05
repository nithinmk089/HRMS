
CREATE PROCEDURE leave.usp_LeaveRequest_Approve
    @LeaveRequestID BIGINT,
    @TenantID BIGINT,
    @ApproverID BIGINT,
    @Remarks NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Get request details
        DECLARE @EmployeeID BIGINT, @LeaveTypeID BIGINT, @TotalDays DECIMAL(5,2);
        SELECT @EmployeeID = EmployeeID, @LeaveTypeID = LeaveTypeID, @TotalDays = TotalDays
        FROM leave.LeaveRequest
        WHERE LeaveRequestID = @LeaveRequestID AND TenantID = @TenantID AND IsDeleted = 0;

        UPDATE leave.LeaveRequest
        SET [Status] = 'Approved',
            ModifiedBy = @ApproverID,
            ModifiedDate = GETUTCDATE()
        WHERE LeaveRequestID = @LeaveRequestID AND TenantID = @TenantID AND IsDeleted = 0;

        INSERT INTO leave.LeaveApproval (
            TenantID, LeaveRequestID, ApproverID, ApprovalDate, ApprovalRemarks, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @LeaveRequestID, @ApproverID, GETUTCDATE(), @Remarks, @ApproverID, GETUTCDATE(), 0
        );

        -- Adjust leave balances
        UPDATE leave.LeaveBalance
        SET ConsumedBalance = ConsumedBalance + @TotalDays,
            AvailableBalance = AvailableBalance - @TotalDays,
            ModifiedBy = @ApproverID,
            ModifiedDate = GETUTCDATE()
        WHERE TenantID = @TenantID AND EmployeeID = @EmployeeID AND LeaveTypeID = @LeaveTypeID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_LeaveRequest_Approve', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;