
CREATE   PROCEDURE organization.usp_Location_Update
    @LocationID BIGINT,
    @TenantID BIGINT,
    @LocationName VARCHAR(200),
    @CountryCode VARCHAR(10) = NULL,
    @StateCode VARCHAR(10) = NULL,
    @City VARCHAR(100) = NULL,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE organization.Location
        SET LocationName = @LocationName,
            CountryCode = @CountryCode,
            StateCode = @StateCode,
            City = @City,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE(),
            VersionNo = VersionNo + 1
        WHERE LocationID = @LocationID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;