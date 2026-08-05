
CREATE PROCEDURE leave.usp_LeaveRequest_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @LeaveTypeID BIGINT,
    @FromDate DATE,
    @ToDate DATE,
    @TotalDays DECIMAL(5,2),
    @Reason NVARCHAR(500),
    @Status NVARCHAR(50) = 'Pending',
    @CreatedBy BIGINT,
    @LeaveRequestID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO leave.LeaveRequest (
            TenantID, EmployeeID, LeaveTypeID, FromDate, ToDate, TotalDays, Reason, [Status], CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @LeaveTypeID, @FromDate, @ToDate, @TotalDays, @Reason, @Status, @CreatedBy, GETUTCDATE(), 0
        );
        SET @LeaveRequestID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_LeaveRequest_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;