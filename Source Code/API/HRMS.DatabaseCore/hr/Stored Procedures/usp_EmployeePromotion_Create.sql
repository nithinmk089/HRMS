
CREATE PROCEDURE hr.usp_EmployeePromotion_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @OldDesignationID BIGINT,
    @NewDesignationID BIGINT,
    @OldGrade NVARCHAR(50) = NULL,
    @NewGrade NVARCHAR(50) = NULL,
    @EffectiveDate DATE,
    @Reason NVARCHAR(500) = NULL,
    @CreatedBy BIGINT,
    @EmployeePromotionID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO hr.EmployeePromotion (
            TenantID, EmployeeID, OldDesignationID, NewDesignationID, OldGrade, NewGrade, 
            EffectiveDate, Reason, [Status], CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @EmployeeID, @OldDesignationID, @NewDesignationID, @OldGrade, @NewGrade, 
            @EffectiveDate, @Reason, 'Pending', @CreatedBy, GETUTCDATE(), 0
        );

        SET @EmployeePromotionID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_EmployeePromotion_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;