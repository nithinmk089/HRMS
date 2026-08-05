
CREATE PROCEDURE hr.usp_Employee_Update
    @EmployeeID BIGINT,
    @TenantID BIGINT,
    @FirstName NVARCHAR(100),
    @MiddleName NVARCHAR(100) = NULL,
    @LastName NVARCHAR(100),
    @PreferredName NVARCHAR(100) = NULL,
    @Gender NVARCHAR(20) = NULL,
    @DateOfBirth DATE = NULL,
    @MaritalStatus NVARCHAR(50) = NULL,
    @Nationality NVARCHAR(100) = NULL,
    @PersonalEmail NVARCHAR(200) = NULL,
    @MobileNumber NVARCHAR(50) = NULL,
    @Status NVARCHAR(50),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE hr.Employee
        SET FirstName = @FirstName,
            MiddleName = @MiddleName,
            LastName = @LastName,
            PreferredName = @PreferredName,
            Gender = @Gender,
            DateOfBirth = @DateOfBirth,
            MaritalStatus = @MaritalStatus,
            Nationality = @Nationality,
            PersonalEmail = @PersonalEmail,
            MobileNumber = @MobileNumber,
            [Status] = @Status,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_Employee_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;