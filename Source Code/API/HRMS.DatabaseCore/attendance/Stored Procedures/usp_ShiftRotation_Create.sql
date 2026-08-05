
CREATE PROCEDURE attendance.usp_ShiftRotation_Create
    @TenantID BIGINT,
    @RotationName NVARCHAR(100),
    @RotationCycleDays INT,
    @CreatedBy BIGINT,
    @ShiftRotationID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO attendance.ShiftRotation (
            TenantID, RotationName, RotationCycleDays, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @RotationName, @RotationCycleDays, @CreatedBy, GETUTCDATE(), 0
        );
        SET @ShiftRotationID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_ShiftRotation_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;