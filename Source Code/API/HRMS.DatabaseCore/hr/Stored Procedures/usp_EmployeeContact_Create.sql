
CREATE PROCEDURE hr.usp_EmployeeContact_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @ContactType NVARCHAR(50),
    @ContactValue NVARCHAR(200),
    @CreatedBy BIGINT,
    @EmployeeContactID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO hr.EmployeeContact (
            TenantID, EmployeeID, ContactType, ContactValue, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @ContactType, @ContactValue, @CreatedBy, GETUTCDATE(), 0
        );

        SET @EmployeeContactID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeContact_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;