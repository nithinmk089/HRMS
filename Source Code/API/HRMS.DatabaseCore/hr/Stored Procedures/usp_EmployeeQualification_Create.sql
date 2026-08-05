
CREATE PROCEDURE hr.usp_EmployeeQualification_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @QualificationType NVARCHAR(100),
    @Institution NVARCHAR(200),
    @University NVARCHAR(200) = NULL,
    @YearOfPassing INT,
    @Percentage DECIMAL(5,2) = NULL,
    @CreatedBy BIGINT,
    @EmployeeQualificationID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO hr.EmployeeQualification (
            TenantID, EmployeeID, QualificationType, Institution, University, YearOfPassing, Percentage, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @QualificationType, @Institution, @University, @YearOfPassing, @Percentage, @CreatedBy, GETUTCDATE(), 0
        );

        SET @EmployeeQualificationID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeQualification_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;