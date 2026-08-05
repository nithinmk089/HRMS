
CREATE PROCEDURE hr.usp_EmployeePromotion_Complete
    @EmployeePromotionID BIGINT,
    @TenantID BIGINT,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @EmployeeID BIGINT, @NewDesgID BIGINT, @NewGrade NVARCHAR(50);

        SELECT @EmployeeID = EmployeeID, @NewDesgID = NewDesignationID, @NewGrade = NewGrade
        FROM hr.EmployeePromotion
        WHERE EmployeePromotionID = @EmployeePromotionID AND TenantID = @TenantID AND [Status] = 'Approved' AND IsDeleted = 0;

        IF @EmployeeID IS NOT NULL
        BEGIN
            UPDATE hr.EmployeePromotion
            SET [Status] = 'Completed',
                ModifiedBy = @ModifiedBy,
                ModifiedDate = GETUTCDATE()
            WHERE EmployeePromotionID = @EmployeePromotionID;

            UPDATE hr.EmployeeEmployment
            SET DesignationID = @NewDesgID,
                ModifiedBy = @ModifiedBy,
                ModifiedDate = GETUTCDATE()
            WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0;

            -- Optionally update designation grade reference if designation grade is updated
            UPDATE organization.Designation
            SET Grade = @NewGrade,
                ModifiedBy = @ModifiedBy,
                ModifiedDate = GETUTCDATE()
            WHERE DesignationID = @NewDesgID AND TenantID = @TenantID AND IsDeleted = 0;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeePromotion_Complete', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;