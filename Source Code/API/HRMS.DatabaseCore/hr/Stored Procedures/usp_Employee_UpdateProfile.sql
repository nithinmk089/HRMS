
CREATE PROCEDURE hr.usp_Employee_UpdateProfile
    @EmployeeID BIGINT,
    @TenantID BIGINT,
    @PreferredName NVARCHAR(100) = NULL,
    @MaritalStatus NVARCHAR(50) = NULL,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE hr.Employee
        SET PreferredName = @PreferredName,
            MaritalStatus = @MaritalStatus,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_Employee_UpdateProfile', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;