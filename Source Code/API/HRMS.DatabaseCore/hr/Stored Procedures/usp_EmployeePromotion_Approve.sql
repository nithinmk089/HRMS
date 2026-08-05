
CREATE PROCEDURE hr.usp_EmployeePromotion_Approve
    @EmployeePromotionID BIGINT,
    @TenantID BIGINT,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE hr.EmployeePromotion
        SET [Status] = 'Approved',
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE EmployeePromotionID = @EmployeePromotionID AND TenantID = @TenantID AND [Status] = 'Pending' AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeePromotion_Approve', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;