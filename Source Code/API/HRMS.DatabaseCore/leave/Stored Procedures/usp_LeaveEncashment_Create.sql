
CREATE PROCEDURE leave.usp_LeaveEncashment_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @LeaveTypeID BIGINT,
    @EncashedDays DECIMAL(5,2),
    @Amount DECIMAL(18,2),
    @CreatedBy BIGINT,
    @LeaveEncashmentID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO leave.LeaveEncashment (
            TenantID, EmployeeID, LeaveTypeID, EncashedDays, Amount, [Status], CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @LeaveTypeID, @EncashedDays, @Amount, 'Pending', @CreatedBy, GETUTCDATE(), 0
        );
        SET @LeaveEncashmentID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_LeaveEncashment_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;