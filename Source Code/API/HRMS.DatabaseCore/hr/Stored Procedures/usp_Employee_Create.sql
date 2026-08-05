
CREATE PROCEDURE hr.usp_Employee_Create
    @TenantID BIGINT,
    @EmployeeCode NVARCHAR(50),
    @EmployeeNumber NVARCHAR(50),
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
    @Status NVARCHAR(50) = 'Active',
    @CreatedBy BIGINT,
    @EmployeeID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO hr.Employee (
            TenantID, EmployeeCode, EmployeeNumber, FirstName, MiddleName, LastName, 
            PreferredName, Gender, DateOfBirth, MaritalStatus, Nationality, PersonalEmail, 
            MobileNumber, [Status], CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeCode, @EmployeeNumber, @FirstName, @MiddleName, @LastName, 
            @PreferredName, @Gender, @DateOfBirth, @MaritalStatus, @Nationality, @PersonalEmail, 
            @MobileNumber, @Status, @CreatedBy, GETUTCDATE(), 0
        );

        SET @EmployeeID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_Employee_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;