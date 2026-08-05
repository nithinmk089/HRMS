
CREATE   PROCEDURE security.usp_Tenant_GetById
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TenantID, TenantCode, TenantName, [Status], EffectiveFrom, EffectiveTo, VersionNo
    FROM security.Tenant
    WHERE TenantID = @TenantID AND IsDeleted = 0;
END;