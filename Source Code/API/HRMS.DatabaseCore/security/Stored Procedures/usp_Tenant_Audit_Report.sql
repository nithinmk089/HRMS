
CREATE   PROCEDURE security.usp_Tenant_Audit_Report
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AuditLogID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy, PerformedDate
    FROM system.AuditLog
    WHERE TenantID = @TenantID
    ORDER BY PerformedDate DESC;
END;