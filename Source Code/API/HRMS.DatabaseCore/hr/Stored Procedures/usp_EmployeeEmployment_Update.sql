
CREATE PROCEDURE hr.usp_EmployeeEmployment_Update
    @EmployeeEmploymentID BIGINT,
    @TenantID BIGINT,
    @CompanyID BIGINT,
    @BusinessUnitID BIGINT,
    @DepartmentID BIGINT,
    @DesignationID BIGINT,
    @LocationID BIGINT,
    @CostCenterID BIGINT,
    @EmploymentType NVARCHAR(50),
    @JoiningDate DATE,
    @ConfirmationDate DATE = NULL,
    @ProbationEndDate DATE = NULL,
    @NoticePeriodDays INT,
    @EmploymentStatus NVARCHAR(50),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE hr.EmployeeEmployment
        SET CompanyID = @CompanyID,
            BusinessUnitID = @BusinessUnitID,
            DepartmentID = @DepartmentID,
            DesignationID = @DesignationID,
            LocationID = @LocationID,
            CostCenterID = @CostCenterID,
            EmploymentType = @EmploymentType,
            JoiningDate = @JoiningDate,
            ConfirmationDate = @ConfirmationDate,
            ProbationEndDate = @ProbationEndDate,
            NoticePeriodDays = @NoticePeriodDays,
            EmploymentStatus = @EmploymentStatus,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE EmployeeEmploymentID = @EmployeeEmploymentID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeEmployment_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;