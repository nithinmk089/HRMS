
-- asset.usp_AssetAssignment_Search.sql
CREATE PROCEDURE asset.usp_AssetAssignment_Search
    @TenantID BIGINT,
    @EmployeeID BIGINT = NULL,
    @AssetID BIGINT = NULL,
    @Status NVARCHAR(50) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM asset.AssetAssignment
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@EmployeeID IS NULL OR EmployeeID = @EmployeeID)
      AND (@AssetID IS NULL OR AssetID = @AssetID)
      AND (@Status IS NULL OR AssignmentStatus = @Status);

    SELECT AssetAssignmentID, TenantID, AssetID, EmployeeID, AssignedDate, ExpectedReturnDate, ReturnedDate, AssignmentStatus
    FROM asset.AssetAssignment
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@EmployeeID IS NULL OR EmployeeID = @EmployeeID)
      AND (@AssetID IS NULL OR AssetID = @AssetID)
      AND (@Status IS NULL OR AssignmentStatus = @Status)
    ORDER BY AssignedDate DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;