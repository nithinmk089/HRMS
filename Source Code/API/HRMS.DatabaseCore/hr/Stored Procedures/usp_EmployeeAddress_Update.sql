
CREATE PROCEDURE hr.usp_EmployeeAddress_Update
    @EmployeeAddressID BIGINT,
    @TenantID BIGINT,
    @AddressType NVARCHAR(50),
    @AddressLine1 NVARCHAR(250),
    @AddressLine2 NVARCHAR(250) = NULL,
    @City NVARCHAR(100),
    @State NVARCHAR(100) = NULL,
    @Country NVARCHAR(100),
    @ZipCode NVARCHAR(20) = NULL,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE hr.EmployeeAddress
        SET AddressType = @AddressType,
            AddressLine1 = @AddressLine1,
            AddressLine2 = @AddressLine2,
            City = @City,
            [State] = @State,
            Country = @Country,
            ZipCode = @ZipCode,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE EmployeeAddressID = @EmployeeAddressID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeAddress_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;