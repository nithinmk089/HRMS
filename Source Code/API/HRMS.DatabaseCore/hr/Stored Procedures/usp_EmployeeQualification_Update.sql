
CREATE PROCEDURE hr.usp_EmployeeQualification_Update
    @EmployeeQualificationID BIGINT,
    @TenantID BIGINT,
    @QualificationType NVARCHAR(100),
    @Institution NVARCHAR(200),
    @University NVARCHAR(200) = NULL,
    @YearOfPassing INT,
    @Percentage DECIMAL(5,2) = NULL,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE hr.EmployeeQualification
        SET QualificationType = @QualificationType,
            Institution = @Institution,
            University = @University,
            YearOfPassing = @YearOfPassing,
            Percentage = @Percentage,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE EmployeeQualificationID = @EmployeeQualificationID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeQualification_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;