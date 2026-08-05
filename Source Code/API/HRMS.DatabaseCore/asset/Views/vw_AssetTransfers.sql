
-- asset.vw_AssetTransfers.sql
CREATE VIEW asset.vw_AssetTransfers
AS
SELECT 
    at.AssetTransferID,
    at.TenantID,
    at.AssetID,
    am.AssetCode,
    am.AssetName,
    at.FromEmployeeID,
    ef.FirstName + ' ' + ef.LastName AS FromEmployeeName,
    at.ToEmployeeID,
    et.FirstName + ' ' + et.LastName AS ToEmployeeName,
    at.TransferDate,
    at.TransferReason,
    at.TransferStatus
FROM asset.AssetTransfer at
JOIN asset.AssetMaster am ON at.AssetID = am.AssetID
LEFT JOIN hr.Employee ef ON at.FromEmployeeID = ef.EmployeeID
JOIN hr.Employee et ON at.ToEmployeeID = et.EmployeeID
WHERE at.IsDeleted = 0;