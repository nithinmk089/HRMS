
-- learning.vw_CourseCatalog.sql
CREATE VIEW learning.vw_CourseCatalog
AS
SELECT c.CourseID, c.TenantID, c.CourseCode, c.CourseName, c.DurationMinutes, c.CourseStatus,
       cc.CategoryName
FROM learning.Course c
LEFT JOIN learning.CourseCategory cc ON c.CourseCategoryID = cc.CourseCategoryID
WHERE c.IsDeleted = 0;