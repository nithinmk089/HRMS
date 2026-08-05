
CREATE PROCEDURE attendance.usp_AttendanceRegularization_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @RequestedDate DATE,
    @Reason NVARCHAR(500),
    @CreatedBy BIGINT,
    @AttendanceRegularizationID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO attendance.AttendanceRegularization (
            TenantID, EmployeeID, RequestedDate, Reason, [Status], CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @RequestedDate, @Reason, 'Pending', @CreatedBy, GETUTCDATE(), 0
        );
        SET @AttendanceRegularizationID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_AttendanceRegularization_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;