
-- ==========================================
-- 4. STORED PROCEDURES
-- ==========================================

-- asset.usp_AssetAssignment_Update.sql
CREATE PROCEDURE asset.usp_AssetAssignment_Update
    @AssetAssignmentID BIGINT,
    @TenantID BIGINT,
    @ExpectedReturnDate DATE = NULL,
    @AssignmentStatus NVARCHAR(50),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE asset.AssetAssignment
        SET ExpectedReturnDate = @ExpectedReturnDate,
            AssignmentStatus = @AssignmentStatus,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE AssetAssignmentID = @AssetAssignmentID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;