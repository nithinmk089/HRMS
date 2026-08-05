
CREATE PROCEDURE leave.usp_LeaveRequest_Update
    @LeaveRequestID BIGINT,
    @TenantID BIGINT,
    @LeaveTypeID BIGINT,
    @FromDate DATE,
    @ToDate DATE,
    @TotalDays DECIMAL(5,2),
    @Reason NVARCHAR(500),
    @Status NVARCHAR(50),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE leave.LeaveRequest
        SET LeaveTypeID = @LeaveTypeID,
            FromDate = @FromDate,
            ToDate = @ToDate,
            TotalDays = @TotalDays,
            Reason = @Reason,
            [Status] = @Status,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE LeaveRequestID = @LeaveRequestID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_LeaveRequest_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;