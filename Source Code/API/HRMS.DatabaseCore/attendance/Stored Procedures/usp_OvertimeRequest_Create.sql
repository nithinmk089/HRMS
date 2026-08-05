
CREATE PROCEDURE attendance.usp_OvertimeRequest_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @OvertimeDate DATE,
    @RequestedHours DECIMAL(5,2),
    @CreatedBy BIGINT,
    @OvertimeRequestID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO attendance.OvertimeRequest (
            TenantID, EmployeeID, OvertimeDate, RequestedHours, [Status], CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @OvertimeDate, @RequestedHours, 'Pending', @CreatedBy, GETUTCDATE(), 0
        );
        SET @OvertimeRequestID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_OvertimeRequest_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;