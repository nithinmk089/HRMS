
CREATE PROCEDURE hr.usp_Employee_BulkUpdate
    @TenantID BIGINT,
    @ModifiedBy BIGINT,
    @JsonData NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE e
        SET e.FirstName = j.FirstName,
            e.MiddleName = j.MiddleName,
            e.LastName = j.LastName,
            e.PersonalEmail = j.PersonalEmail,
            e.MobileNumber = j.MobileNumber,
            e.ModifiedBy = @ModifiedBy,
            e.ModifiedDate = GETUTCDATE()
        FROM hr.Employee e
        INNER JOIN OPENJSON(@JsonData)
        WITH (
            EmployeeID BIGINT,
            FirstName NVARCHAR(100),
            MiddleName NVARCHAR(100),
            LastName NVARCHAR(100),
            PersonalEmail NVARCHAR(200),
            MobileNumber NVARCHAR(50)
        ) j ON e.EmployeeID = j.EmployeeID AND e.TenantID = @TenantID AND e.IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_Employee_BulkUpdate', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;