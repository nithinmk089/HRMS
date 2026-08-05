
-- STORED PROCEDURES - DEPARTMENT
CREATE   PROCEDURE organization.usp_Department_Create
    @TenantID BIGINT,
    @BusinessUnitID BIGINT,
    @DepartmentCode VARCHAR(50),
    @DepartmentName VARCHAR(200),
    @ParentDepartmentID BIGINT = NULL,
    @CreatedBy BIGINT,
    @DepartmentID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO organization.Department (TenantID, BusinessUnitID, DepartmentCode, DepartmentName, ParentDepartmentID, CreatedBy)
        VALUES (@TenantID, @BusinessUnitID, @DepartmentCode, @DepartmentName, @ParentDepartmentID, @CreatedBy);
        SET @DepartmentID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;