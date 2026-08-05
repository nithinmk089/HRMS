
-- performance.usp_PerformanceCycle_Create.sql
CREATE PROCEDURE performance.usp_PerformanceCycle_Create
    @TenantID BIGINT,
    @CycleCode NVARCHAR(50),
    @CycleName NVARCHAR(100),
    @StartDate DATE,
    @EndDate DATE,
    @CreatedBy BIGINT,
    @PerformanceCycleID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO performance.PerformanceCycle (TenantID, CycleCode, CycleName, StartDate, EndDate, CycleStatus, CreatedBy)
    VALUES (@TenantID, @CycleCode, @CycleName, @StartDate, @EndDate, 'Draft', @CreatedBy);
    SET @PerformanceCycleID = SCOPE_IDENTITY();
END