
CREATE PROCEDURE onboarding.usp_ProbationReview_Create
    @TenantID          BIGINT,
    @EmployeeID        BIGINT,
    @ReviewDate        DATE,
    @ReviewStatus      NVARCHAR(50),
    @ReviewerID        BIGINT,
    @Comments          NVARCHAR(1000),
    @CreatedBy         BIGINT,
    @ProbationReviewID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO onboarding.ProbationReview (
            TenantID, EmployeeID, ReviewDate, ReviewStatus, ReviewerID, Comments, CreatedBy
        )
        VALUES (
            @TenantID, @EmployeeID, @ReviewDate, @ReviewStatus, @ReviewerID, @Comments, @CreatedBy
        );

        SET @ProbationReviewID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END