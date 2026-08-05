
CREATE PROCEDURE leave.usp_LeaveType_Update
    @LeaveTypeID BIGINT,
    @TenantID BIGINT,
    @LeaveCode NVARCHAR(50),
    @LeaveName NVARCHAR(100),
    @IsPaid BIT,
    @IsAccrualBased BIT,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE leave.LeaveType
        SET LeaveCode = @LeaveCode,
            LeaveName = @LeaveName,
            IsPaid = @IsPaid,
            IsAccrualBased = @IsAccrualBased,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE LeaveTypeID = @LeaveTypeID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_LeaveType_Update', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;