using System;

namespace HRMS.Application.DTOs
{
    public class CreateExitRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime ResignationDate { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public string ExitReason { get; set; } = null!;
        public long CreatedBy { get; set; }
    }

    public class UpdateExitRequest
    {
        public long ExitRequestID { get; set; }
        public long TenantID { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public string ExitReason { get; set; } = null!;
        public long ModifiedBy { get; set; }
    }

    public class ApproveExitRequest
    {
        public long ExitRequestID { get; set; }
        public long TenantID { get; set; }
        public long ApproverID { get; set; }
        public string Remarks { get; set; } = null!;
    }

    public class RejectExitRequest
    {
        public long ExitRequestID { get; set; }
        public long TenantID { get; set; }
        public long ApproverID { get; set; }
        public string Remarks { get; set; } = null!;
    }

    public class CreateClearanceRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime InitiatedDate { get; set; }
        public long CreatedBy { get; set; }
    }

    public class CreateAssetReturnRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long AssetID { get; set; }
        public DateTime ReturnDate { get; set; }
        public long CreatedBy { get; set; }
    }

    public class VerifyAssetReturnRequest
    {
        public long AssetReturnID { get; set; }
        public long TenantID { get; set; }
        public string ReturnCondition { get; set; } = null!;
        public long ModifiedBy { get; set; }
    }

    public class CreateKnowledgeTransferRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long SuccessorEmployeeID { get; set; }
        public DateTime KTDate { get; set; }
        public long CreatedBy { get; set; }
    }

    public class GenerateExperienceLetterRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long CreatedBy { get; set; }
    }

    public class CalculateFFSRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public decimal SettlementAmount { get; set; }
        public long CreatedBy { get; set; }
    }

    public class ExitStatusReportDto
    {
        public long ExitRequestID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public DateTime ResignationDate { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public string ExitReason { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string ClearanceStatus { get; set; } = null!;
        public string SettlementStatus { get; set; } = null!;
    }

    public class ClearanceStatusReportDto
    {
        public long ClearanceRequestID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string ClearanceStatus { get; set; } = null!;
        public DateTime InitiatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int TotalTasks { get; set; }
        public int ApprovedTasks { get; set; }
    }

    public class FullAndFinalSummaryReportDto
    {
        public long FullAndFinalSettlementID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public decimal SettlementAmount { get; set; }
        public DateTime? SettlementDate { get; set; }
        public string SettlementStatus { get; set; } = null!;
    }
}
