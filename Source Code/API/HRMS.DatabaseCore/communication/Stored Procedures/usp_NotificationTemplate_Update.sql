
CREATE   PROCEDURE communication.usp_NotificationTemplate_Update
    @TemplateID BIGINT,
    @TenantID BIGINT,
    @TemplateName VARCHAR(100),
    @BusinessEvent VARCHAR(100),
    @Channel VARCHAR(50),
    @SubjectTemplate VARCHAR(200),
    @BodyTemplate VARCHAR(MAX),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE communication.NotificationTemplate
        SET TemplateName = @TemplateName,
            BusinessEvent = @BusinessEvent,
            Channel = @Channel,
            SubjectTemplate = @SubjectTemplate,
            BodyTemplate = @BodyTemplate,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE(),
            VersionNo = VersionNo + 1
        WHERE TemplateID = @TemplateID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace)
        VALUES ('communication.usp_NotificationTemplate_Update', ERROR_MESSAGE(), CONVERT(VARCHAR, ERROR_STATE()));
        THROW;
    END CATCH
END;