
-- payroll.usp_LoanAdvance_Approve.sql
CREATE PROCEDURE payroll.usp_LoanAdvance_Approve
    @LoanAdvanceID BIGINT,
    @TenantID BIGINT,
    @ApprovedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRANSACTION;
    
    UPDATE payroll.LoanAdvance
    SET Status = 'Approved', ModifiedBy = @ApprovedBy, ModifiedDate = GETUTCDATE()
    WHERE LoanAdvanceID = @LoanAdvanceID AND TenantID = @TenantID AND Status = 'Pending';
    
    -- Generate repayment schedule
    DECLARE @Principal DECIMAL(18,2), @Tenure INT, @MonthlyInstallment DECIMAL(18,2);
    SELECT @Principal = PrincipalAmount, @Tenure = TenureMonths, @MonthlyInstallment = MonthlyInstallment
    FROM payroll.LoanAdvance WHERE LoanAdvanceID = @LoanAdvanceID;
    
    DECLARE @i INT = 1;
    WHILE @i <= @Tenure
    BEGIN
        INSERT INTO payroll.LoanRepayment (TenantID, LoanAdvanceID, InstallmentNo, RepaymentAmount, DueDate, Status, CreatedBy)
        VALUES (@TenantID, @LoanAdvanceID, @i, @MonthlyInstallment, DATEADD(month, @i, GETUTCDATE()), 'Unpaid', @ApprovedBy);
        SET @i = @i + 1;
    END
    
    COMMIT TRANSACTION;
END