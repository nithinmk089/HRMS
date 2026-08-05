
-- STORED PROCEDURES - LOCATION
CREATE   PROCEDURE organization.usp_Location_Create
    @TenantID BIGINT,
    @LocationCode VARCHAR(50),
    @LocationName VARCHAR(200),
    @CountryCode VARCHAR(10) = NULL,
    @StateCode VARCHAR(10) = NULL,
    @City VARCHAR(100) = NULL,
    @CreatedBy BIGINT,
    @LocationID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO organization.Location (TenantID, LocationCode, LocationName, CountryCode, StateCode, City, CreatedBy)
        VALUES (@TenantID, @LocationCode, @LocationName, @CountryCode, @StateCode, @City, @CreatedBy);
        SET @LocationID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;