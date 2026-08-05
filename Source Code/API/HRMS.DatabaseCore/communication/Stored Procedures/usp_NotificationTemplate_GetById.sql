
CREATE   PROCEDURE communication.usp_NotificationTemplate_GetById
    @TemplateID BIGINT,
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TemplateID, TenantID, TemplateName, BusinessEvent, Channel, SubjectTemplate, BodyTemplate, IsActive, VersionNo
    FROM communication.NotificationTemplate
    WHERE TemplateID = @TemplateID AND TenantID = @TenantID AND IsDeleted = 0;
END;