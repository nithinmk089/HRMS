
CREATE PROCEDURE hr.usp_EmployeeEmergencyContact_Delete
    @EmployeeEmergencyContactID BIGINT,
    @TenantID BIGINT,
    @DeletedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE hr.EmployeeEmergencyContact
        SET IsDeleted = 1,
            DeletedBy = @DeletedBy,
            DeletedDate = GETUTCDATE()
        WHERE EmployeeEmergencyContactID = @EmployeeEmergencyContactID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeEmergencyContact_Delete', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;