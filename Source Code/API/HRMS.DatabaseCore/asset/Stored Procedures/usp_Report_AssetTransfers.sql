
-- asset.usp_Report_AssetTransfers.sql
CREATE PROCEDURE asset.usp_Report_AssetTransfers
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT at.AssetTransferID, am.AssetCode, am.AssetName, ef.FirstName + ' ' + ef.LastName AS FromEmployee, et.FirstName + ' ' + et.LastName AS ToEmployee, at.TransferDate, at.TransferReason, at.TransferStatus
    FROM asset.AssetTransfer at
    JOIN asset.AssetMaster am ON at.AssetID = am.AssetID
    LEFT JOIN hr.Employee ef ON at.FromEmployeeID = ef.EmployeeID
    JOIN hr.Employee et ON at.ToEmployeeID = et.EmployeeID
    WHERE at.TenantID = @TenantID AND at.IsDeleted = 0;
END;