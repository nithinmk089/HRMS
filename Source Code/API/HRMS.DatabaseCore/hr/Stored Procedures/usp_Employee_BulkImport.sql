
CREATE PROCEDURE hr.usp_Employee_BulkImport
    @TenantID BIGINT,
    @CreatedBy BIGINT,
    @JsonData NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO hr.Employee (
            TenantID, EmployeeCode, EmployeeNumber, FirstName, MiddleName, LastName, 
            PersonalEmail, MobileNumber, [Status], CreatedBy, CreatedDate, IsDeleted
        )
        SELECT 
            @TenantID, j.EmployeeCode, j.EmployeeNumber, j.FirstName, j.MiddleName, j.LastName, 
            j.PersonalEmail, j.MobileNumber, 'Active', @CreatedBy, GETUTCDATE(), 0
        FROM OPENJSON(@JsonData)
        WITH (
            EmployeeCode NVARCHAR(50),
            EmployeeNumber NVARCHAR(50),
            FirstName NVARCHAR(100),
            MiddleName NVARCHAR(100),
            LastName NVARCHAR(100),
            PersonalEmail NVARCHAR(200),
            MobileNumber NVARCHAR(50)
        ) j;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_Employee_BulkImport', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;