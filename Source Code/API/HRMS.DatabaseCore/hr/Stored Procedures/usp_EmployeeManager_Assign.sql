
CREATE PROCEDURE hr.usp_EmployeeManager_Assign
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @ManagerID BIGINT,
    @EffectiveFrom DATE,
    @CreatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- End date existing active manager
        UPDATE hr.EmployeeManager
        SET EffectiveTo = DATEADD(day, -1, @EffectiveFrom),
            ModifiedBy = @CreatedBy,
            ModifiedDate = GETUTCDATE()
        WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0 AND EffectiveTo IS NULL;

        INSERT INTO hr.EmployeeManager (
            TenantID, EmployeeID, ManagerID, EffectiveFrom, EffectiveTo, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @ManagerID, @EffectiveFrom, NULL, @CreatedBy, GETUTCDATE(), 0
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeeManager_Assign', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;