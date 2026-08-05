
CREATE FUNCTION offboarding.fn_GetSettlementStatus
(
    @EmployeeID BIGINT,
    @TenantID BIGINT
)
RETURNS NVARCHAR(50)
AS
BEGIN
    DECLARE @Status NVARCHAR(50) = 'Pending';

    SELECT TOP 1 @Status = SettlementStatus
    FROM offboarding.FullAndFinalSettlement
    WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0;

    RETURN @Status;
END