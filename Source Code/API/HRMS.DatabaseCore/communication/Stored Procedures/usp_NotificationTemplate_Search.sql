
CREATE   PROCEDURE communication.usp_NotificationTemplate_Search
    @TenantID BIGINT,
    @SearchText VARCHAR(100) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    SELECT COUNT(*) AS TotalCount
    FROM communication.NotificationTemplate
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR TemplateName LIKE '%' + @SearchText + '%' OR BusinessEvent LIKE '%' + @SearchText + '%');
    SELECT TemplateID, TenantID, TemplateName, BusinessEvent, Channel, SubjectTemplate, BodyTemplate, IsActive, VersionNo
    FROM communication.NotificationTemplate
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR TemplateName LIKE '%' + @SearchText + '%' OR BusinessEvent LIKE '%' + @SearchText + '%')
    ORDER BY TemplateID DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;