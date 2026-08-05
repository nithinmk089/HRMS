
CREATE PROCEDURE attendance.usp_ShiftAssignment_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @ShiftID BIGINT,
    @EffectiveFrom DATE,
    @EffectiveTo DATE = NULL,
    @CreatedBy BIGINT,
    @ShiftAssignmentID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO attendance.ShiftAssignment (
            TenantID, EmployeeID, ShiftID, EffectiveFrom, EffectiveTo, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @ShiftID, @EffectiveFrom, @EffectiveTo, @CreatedBy, GETUTCDATE(), 0
        );
        SET @ShiftAssignmentID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_ShiftAssignment_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;