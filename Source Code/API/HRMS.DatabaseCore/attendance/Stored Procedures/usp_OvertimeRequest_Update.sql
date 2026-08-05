
CREATE PROCEDURE attendance.usp_OvertimeRequest_Update
    @OvertimeRequestID BIGINT,
    @TenantID BIGINT,
    @RequestedHours DECIMAL(5,2),
    @Status NVARCHAR(50),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE attendance.OvertimeRequest
        SET RequestedHours = @RequestedHours,
            [Status] = @Status,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE OvertimeRequestID = @OvertimeRequestID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_OvertimeRequest_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;