
-- asset.usp_AssetReturn_Create.sql
CREATE PROCEDURE asset.usp_AssetReturn_Create
    @TenantID BIGINT,
    @AssetAssignmentID BIGINT,
    @ReturnDate DATE,
    @ReturnCondition NVARCHAR(500) = NULL,
    @CreatedBy BIGINT,
    @AssetReturnID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO asset.AssetReturn (TenantID, AssetAssignmentID, ReturnDate, ReturnCondition, ReturnStatus, CreatedBy)
        VALUES (@TenantID, @AssetAssignmentID, @ReturnDate, @ReturnCondition, 'Pending', @CreatedBy);
        SET @AssetReturnID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;