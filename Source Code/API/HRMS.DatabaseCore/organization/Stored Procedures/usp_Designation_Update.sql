
CREATE   PROCEDURE organization.usp_Designation_Update
    @DesignationID BIGINT,
    @TenantID BIGINT,
    @DesignationName VARCHAR(200),
    @Grade VARCHAR(50) = NULL,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE organization.Designation
        SET DesignationName = @DesignationName,
            Grade = @Grade,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE(),
            VersionNo = VersionNo + 1
        WHERE DesignationID = @DesignationID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;