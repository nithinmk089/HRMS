
-- asset.usp_AssetDisposal_Create.sql
CREATE PROCEDURE asset.usp_AssetDisposal_Create
    @TenantID BIGINT,
    @AssetID BIGINT,
    @DisposalDate DATE,
    @DisposalMethod NVARCHAR(100),
    @DisposalValue DECIMAL(18,2),
    @CreatedBy BIGINT,
    @AssetDisposalID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO asset.AssetDisposal (TenantID, AssetID, DisposalDate, DisposalMethod, DisposalValue, DisposalStatus, CreatedBy)
        VALUES (@TenantID, @AssetID, @DisposalDate, @DisposalMethod, @DisposalValue, 'Pending', @CreatedBy);
        SET @AssetDisposalID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;