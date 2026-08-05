
CREATE PROCEDURE hr.usp_Employee_Suspend
    @EmployeeID BIGINT,
    @TenantID BIGINT,
    @Reason NVARCHAR(500) = NULL,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE hr.Employee
        SET [Status] = 'Suspended',
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0;

        INSERT INTO hr.EmployeeStatusHistory (TenantID, EmployeeID, [Status], EffectiveDate, Reason, CreatedBy)
        VALUES (@TenantID, @EmployeeID, 'Suspended', CAST(GETUTCDATE() AS DATE), @Reason, @ModifiedBy);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_Employee_Suspend', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;