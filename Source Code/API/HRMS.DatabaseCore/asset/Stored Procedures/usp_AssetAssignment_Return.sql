
-- asset.usp_AssetAssignment_Return.sql
CREATE PROCEDURE asset.usp_AssetAssignment_Return
    @AssetAssignmentID BIGINT,
    @TenantID BIGINT,
    @ReturnedDate DATE,
    @ReturnCondition NVARCHAR(500) = NULL,
    @CreatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        UPDATE asset.AssetAssignment
        SET ReturnedDate = @ReturnedDate,
            AssignmentStatus = 'Returned',
            ModifiedBy = @CreatedBy,
            ModifiedDate = GETUTCDATE()
        WHERE AssetAssignmentID = @AssetAssignmentID AND TenantID = @TenantID;

        INSERT INTO asset.AssetReturn (TenantID, AssetAssignmentID, ReturnDate, ReturnCondition, ReturnStatus, CreatedBy)
        VALUES (@TenantID, @AssetAssignmentID, @ReturnedDate, @ReturnCondition, 'Pending', @CreatedBy);

        DECLARE @AssetID BIGINT;
        SELECT @AssetID = AssetID FROM asset.AssetAssignment WHERE AssetAssignmentID = @AssetAssignmentID;
        UPDATE asset.AssetMaster SET [Status] = 'Available' WHERE AssetID = @AssetID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;