
CREATE PROCEDURE hr.usp_EmployeeEmployment_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
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
    @NoticePeriodDays INT = 0,
    @EmploymentStatus NVARCHAR(50) = 'Active',
    @CreatedBy BIGINT,
    @EmployeeEmploymentID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO hr.EmployeeEmployment (
            TenantID, EmployeeID, CompanyID, BusinessUnitID, DepartmentID, DesignationID, 
            LocationID, CostCenterID, EmploymentType, JoiningDate, ConfirmationDate, 
            ProbationEndDate, NoticePeriodDays, EmploymentStatus, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @CompanyID, @BusinessUnitID, @DepartmentID, @DesignationID, 
            @LocationID, @CostCenterID, @EmploymentType, @JoiningDate, @ConfirmationDate, 
            @ProbationEndDate, @NoticePeriodDays, @EmploymentStatus, @CreatedBy, GETUTCDATE(), 0
        );

        SET @EmployeeEmploymentID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeEmployment_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;