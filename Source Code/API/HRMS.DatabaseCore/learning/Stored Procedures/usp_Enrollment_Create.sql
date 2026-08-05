
-- learning.usp_Enrollment_Create.sql
CREATE PROCEDURE learning.usp_Enrollment_Create
    @TenantID BIGINT, @EmployeeID BIGINT, @CourseID BIGINT,
    @CreatedBy BIGINT, @EnrollmentID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
    INSERT INTO learning.Enrollment (TenantID, EmployeeID, CourseID, CreatedBy)
    VALUES (@TenantID, @EmployeeID, @CourseID, @CreatedBy);
    SET @EnrollmentID = SCOPE_IDENTITY();
    INSERT INTO learning.LearningProgress (TenantID, EnrollmentID, CompletionPercentage, CreatedBy)
    VALUES (@TenantID, @EnrollmentID, 0, @CreatedBy);
    COMMIT TRANSACTION;
END