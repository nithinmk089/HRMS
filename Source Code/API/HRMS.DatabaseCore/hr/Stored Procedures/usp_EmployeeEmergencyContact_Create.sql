
CREATE PROCEDURE hr.usp_EmployeeEmergencyContact_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @ContactName NVARCHAR(200),
    @Relationship NVARCHAR(50),
    @MobileNumber NVARCHAR(50),
    @Email NVARCHAR(200) = NULL,
    @CreatedBy BIGINT,
    @EmployeeEmergencyContactID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO hr.EmployeeEmergencyContact (
            TenantID, EmployeeID, ContactName, Relationship, MobileNumber, Email, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @ContactName, @Relationship, @MobileNumber, @Email, @CreatedBy, GETUTCDATE(), 0
        );

        SET @EmployeeEmergencyContactID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeEmergencyContact_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;