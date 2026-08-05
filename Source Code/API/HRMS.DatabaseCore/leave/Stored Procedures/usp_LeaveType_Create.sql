
CREATE PROCEDURE leave.usp_LeaveType_Create
    @TenantID BIGINT,
    @LeaveCode NVARCHAR(50),
    @LeaveName NVARCHAR(100),
    @IsPaid BIT = 1,
    @IsAccrualBased BIT = 1,
    @CreatedBy BIGINT,
    @LeaveTypeID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO leave.LeaveType (
            TenantID, LeaveCode, LeaveName, IsPaid, IsAccrualBased, CreatedBy, CreatedDate, IsDeleted
        )
        VALUES (
            @TenantID, @LeaveCode, @LeaveName, @IsPaid, @IsAccrualBased, @CreatedBy, GETUTCDATE(), 0
        );
        SET @LeaveTypeID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace, OccurredDate)
        VALUES ('usp_LeaveType_Create', ERROR_MESSAGE(), ERROR_STATE(), GETUTCDATE());
        THROW;
    END CATCH
END;