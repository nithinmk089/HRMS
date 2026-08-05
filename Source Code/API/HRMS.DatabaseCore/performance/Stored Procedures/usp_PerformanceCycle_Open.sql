
-- performance.usp_PerformanceCycle_Open.sql
CREATE PROCEDURE performance.usp_PerformanceCycle_Open
    @PerformanceCycleID BIGINT,
    @TenantID BIGINT,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE performance.PerformanceCycle
    SET CycleStatus = 'Active', ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE()
    WHERE PerformanceCycleID = @PerformanceCycleID AND TenantID = @TenantID AND IsDeleted = 0;
END