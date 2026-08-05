
CREATE PROCEDURE attendance.usp_AttendanceAdjustment_Create
    @TenantID BIGINT,
    @AttendanceID BIGINT,
    @AdjustmentReason NVARCHAR(500),
    @OriginalValue NVARCHAR(100),
    @NewValue NVARCHAR(100),
    @CreatedBy BIGINT,
    @AttendanceAdjustmentID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO attendance.AttendanceAdjustment (
            TenantID, AttendanceID, AdjustmentReason, OriginalValue, NewValue, ApprovalStatus, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @AttendanceID, @AdjustmentReason, @OriginalValue, @NewValue, 'Pending', @CreatedBy, GETUTCDATE(), 0
        );
        SET @AttendanceAdjustmentID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_AttendanceAdjustment_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;