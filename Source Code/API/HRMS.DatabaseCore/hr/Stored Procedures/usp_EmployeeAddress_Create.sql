
CREATE PROCEDURE hr.usp_EmployeeAddress_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @AddressType NVARCHAR(50),
    @AddressLine1 NVARCHAR(250),
    @AddressLine2 NVARCHAR(250) = NULL,
    @City NVARCHAR(100),
    @State NVARCHAR(100) = NULL,
    @Country NVARCHAR(100),
    @ZipCode NVARCHAR(20) = NULL,
    @CreatedBy BIGINT,
    @EmployeeAddressID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO hr.EmployeeAddress (
            TenantID, EmployeeID, AddressType, AddressLine1, AddressLine2, City, 
            [State], Country, ZipCode, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @AddressType, @AddressLine1, @AddressLine2, @City, 
            @State, @Country, @ZipCode, @CreatedBy, GETUTCDATE(), 0
        );

        SET @EmployeeAddressID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeAddress_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;