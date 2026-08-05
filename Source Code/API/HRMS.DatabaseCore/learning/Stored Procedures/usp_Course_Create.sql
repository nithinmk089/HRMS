
-- learning.usp_Course_Create.sql
CREATE PROCEDURE learning.usp_Course_Create
    @TenantID BIGINT, @CourseCode NVARCHAR(50), @CourseName NVARCHAR(200),
    @CourseCategoryID BIGINT, @Description NVARCHAR(1000) = NULL, @DurationMinutes INT = 60,
    @CreatedBy BIGINT, @CourseID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO learning.Course (TenantID, CourseCode, CourseName, CourseCategoryID, Description, DurationMinutes, CreatedBy)
    VALUES (@TenantID, @CourseCode, @CourseName, @CourseCategoryID, @Description, @DurationMinutes, @CreatedBy);
    SET @CourseID = SCOPE_IDENTITY();
END