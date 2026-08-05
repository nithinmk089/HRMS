
CREATE PROCEDURE hr.usp_Report_EmployeeServiceHistory
    @TenantID BIGINT,
    @EmployeeID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        sh.EmployeeStatusHistoryID AS RecordID,
        'StatusChange' AS RecordType,
        sh.[Status] AS DetailText,
        sh.EffectiveDate,
        sh.Reason,
        sh.CreatedDate
    FROM hr.EmployeeStatusHistory sh
    WHERE sh.EmployeeID = @EmployeeID AND sh.TenantID = @TenantID AND sh.IsDeleted = 0
    
    UNION ALL
    
    SELECT 
        t.EmployeeTransferID AS RecordID,
        'Transfer' AS RecordType,
        CONCAT('Transferred from Dept ID ', t.FromDepartmentID, ' to Dept ID ', t.ToDepartmentID) AS DetailText,
        t.EffectiveDate,
        t.Reason,
        t.CreatedDate
    FROM hr.EmployeeTransfer t
    WHERE t.EmployeeID = @EmployeeID AND t.TenantID = @TenantID AND t.[Status] = 'Completed' AND t.IsDeleted = 0

    UNION ALL

    SELECT 
        p.EmployeePromotionID AS RecordID,
        'Promotion' AS RecordType,
        CONCAT('Promoted from Designation ID ', p.OldDesignationID, ' to Designation ID ', p.NewDesignationID) AS DetailText,
        p.EffectiveDate,
        p.Reason,
        p.CreatedDate
    FROM hr.EmployeePromotion p
    WHERE p.EmployeeID = @EmployeeID AND p.TenantID = @TenantID AND p.[Status] = 'Completed' AND p.IsDeleted = 0
    
    ORDER BY EffectiveDate DESC, RecordID DESC;
END;