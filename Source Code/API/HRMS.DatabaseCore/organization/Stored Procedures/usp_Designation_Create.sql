
-- STORED PROCEDURES - DESIGNATION
CREATE   PROCEDURE organization.usp_Designation_Create
    @TenantID BIGINT,
    @DesignationCode VARCHAR(50),
    @DesignationName VARCHAR(200),
    @Grade VARCHAR(50) = NULL,
    @CreatedBy BIGINT,
    @DesignationID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO organization.Designation (TenantID, DesignationCode, DesignationName, Grade, CreatedBy)
        VALUES (@TenantID, @DesignationCode, @DesignationName, @Grade, @CreatedBy);
        SET @DesignationID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;