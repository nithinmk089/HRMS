
CREATE   FUNCTION organization.fn_GetOrganizationTree
(
    @TenantID BIGINT
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        bu.BusinessUnitID, bu.BusinessUnitName,
        d.DepartmentID, d.DepartmentName, d.ParentDepartmentID
    FROM organization.BusinessUnit bu
    LEFT JOIN organization.Department d ON d.BusinessUnitID = bu.BusinessUnitID AND d.IsDeleted = 0
    WHERE bu.TenantID = @TenantID AND bu.IsDeleted = 0
);