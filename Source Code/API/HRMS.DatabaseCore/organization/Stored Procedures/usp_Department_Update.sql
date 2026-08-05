
CREATE   PROCEDURE organization.usp_Department_Update
    @DepartmentID BIGINT,
    @TenantID BIGINT,
    @DepartmentName VARCHAR(200),
    @ParentDepartmentID BIGINT = NULL,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE organization.Department
        SET DepartmentName = @DepartmentName,
            ParentDepartmentID = @ParentDepartmentID,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE(),
            VersionNo = VersionNo + 1
        WHERE DepartmentID = @DepartmentID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;