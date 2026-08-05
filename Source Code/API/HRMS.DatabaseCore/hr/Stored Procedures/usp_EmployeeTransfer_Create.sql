
CREATE PROCEDURE hr.usp_EmployeeTransfer_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @FromDepartmentID BIGINT,
    @ToDepartmentID BIGINT,
    @FromLocationID BIGINT,
    @ToLocationID BIGINT,
    @EffectiveDate DATE,
    @Reason NVARCHAR(500) = NULL,
    @CreatedBy BIGINT,
    @EmployeeTransferID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO hr.EmployeeTransfer (
            TenantID, EmployeeID, FromDepartmentID, ToDepartmentID, FromLocationID, ToLocationID, 
            EffectiveDate, Reason, [Status], CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @FromDepartmentID, @ToDepartmentID, @FromLocationID, @ToLocationID, 
            @EffectiveDate, @Reason, 'Pending', @CreatedBy, GETUTCDATE(), 0
        );

        SET @EmployeeTransferID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeTransfer_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;