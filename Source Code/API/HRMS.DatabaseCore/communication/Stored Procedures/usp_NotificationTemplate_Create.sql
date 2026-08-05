
-- communication SPs
CREATE   PROCEDURE communication.usp_NotificationTemplate_Create
    @TenantID BIGINT,
    @TemplateName VARCHAR(100),
    @BusinessEvent VARCHAR(100),
    @Channel VARCHAR(50),
    @SubjectTemplate VARCHAR(200),
    @BodyTemplate VARCHAR(MAX),
    @CreatedBy BIGINT,
    @TemplateID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO communication.NotificationTemplate (TenantID, TemplateName, BusinessEvent, Channel, SubjectTemplate, BodyTemplate, CreatedBy)
        VALUES (@TenantID, @TemplateName, @BusinessEvent, @Channel, @SubjectTemplate, @BodyTemplate, @CreatedBy);
        SET @TemplateID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace)
        VALUES ('communication.usp_NotificationTemplate_Create', ERROR_MESSAGE(), CONVERT(VARCHAR, ERROR_STATE()));
        THROW;
    END CATCH
END;