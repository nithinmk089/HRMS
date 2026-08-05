
-- asset.usp_AssetAssignment_Create.sql
CREATE PROCEDURE asset.usp_AssetAssignment_Create
    @TenantID BIGINT,
    @AssetID BIGINT,
    @EmployeeID BIGINT,
    @AssignedDate DATE,
    @ExpectedReturnDate DATE = NULL,
    @CreatedBy BIGINT,
    @AssetAssignmentID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO asset.AssetAssignment (TenantID, AssetID, EmployeeID, AssignedDate, ExpectedReturnDate, AssignmentStatus, CreatedBy)
        VALUES (@TenantID, @AssetID, @EmployeeID, @AssignedDate, @ExpectedReturnDate, 'Active', @CreatedBy);
        SET @AssetAssignmentID = SCOPE_IDENTITY();
        
        UPDATE asset.AssetMaster SET [Status] = 'Assigned' WHERE AssetID = @AssetID;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;