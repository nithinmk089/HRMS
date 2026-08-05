
CREATE PROCEDURE hr.usp_EmployeeEmergencyContact_Update
    @EmployeeEmergencyContactID BIGINT,
    @TenantID BIGINT,
    @ContactName NVARCHAR(200),
    @Relationship NVARCHAR(50),
    @MobileNumber NVARCHAR(50),
    @Email NVARCHAR(200) = NULL,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE hr.EmployeeEmergencyContact
        SET ContactName = @ContactName,
            Relationship = @Relationship,
            MobileNumber = @MobileNumber,
            Email = @Email,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE EmployeeEmergencyContactID = @EmployeeEmergencyContactID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeEmergencyContact_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;