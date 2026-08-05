
CREATE PROCEDURE hr.usp_EmployeeStatusHistory_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @Status NVARCHAR(50),
    @EffectiveDate DATE,
    @Reason NVARCHAR(500) = NULL,
    @CreatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO hr.EmployeeStatusHistory (
            TenantID, EmployeeID, [Status], EffectiveDate, Reason, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @Status, @EffectiveDate, @Reason, @CreatedBy, GETUTCDATE(), 0
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeStatusHistory_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;