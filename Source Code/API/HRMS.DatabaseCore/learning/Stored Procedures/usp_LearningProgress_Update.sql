
-- learning.usp_LearningProgress_Update.sql
CREATE PROCEDURE learning.usp_LearningProgress_Update
    @LearningProgressID BIGINT, @TenantID BIGINT,
    @CompletionPercentage DECIMAL(5,2), @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE learning.LearningProgress
    SET CompletionPercentage = @CompletionPercentage, LastAccessedDate = GETUTCDATE(),
        ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE()
    WHERE LearningProgressID = @LearningProgressID AND TenantID = @TenantID AND IsDeleted = 0;
END