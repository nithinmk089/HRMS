using System;
using System.Collections.Generic;

namespace HRMS.Application.DTOs
{
    // TRAVEL POLICY
    public class TravelPolicyDto
    {
        public long TravelPolicyID { get; set; }
        public long TenantID { get; set; }
        public string PolicyCode { get; set; } = string.Empty;
        public string PolicyName { get; set; } = string.Empty;
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateTravelPolicyRequest
    {
        public long TenantID { get; set; }
        public string PolicyCode { get; set; } = string.Empty;
        public string PolicyName { get; set; } = string.Empty;
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public long CreatedBy { get; set; }
    }

    // TRAVEL REQUEST
    public class TravelRequestDto
    {
        public long TravelRequestID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string TravelType { get; set; } = string.Empty;
        public DateTime TravelStartDate { get; set; }
        public DateTime TravelEndDate { get; set; }
        public string Destination { get; set; } = string.Empty;
        public string RequestStatus { get; set; } = string.Empty;
    }

    public class CreateTravelRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string TravelType { get; set; } = "Domestic";
        public DateTime TravelStartDate { get; set; }
        public DateTime TravelEndDate { get; set; }
        public string Destination { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    public class ApproveTravelRequestRequest
    {
        public long TravelRequestID { get; set; }
        public long TenantID { get; set; }
        public long ApproverID { get; set; }
        public string ApprovalStatus { get; set; } = "Approved"; // 'Approved', 'Rejected'
        public long ModifiedBy { get; set; }
    }

    // TRAVEL ADVANCE
    public class TravelAdvanceDto
    {
        public long TravelAdvanceID { get; set; }
        public long TenantID { get; set; }
        public long TravelRequestID { get; set; }
        public decimal AdvanceAmount { get; set; }
        public DateTime? DisbursementDate { get; set; }
        public string AdvanceStatus { get; set; } = string.Empty;
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
    }

    public class CreateTravelAdvanceRequest
    {
        public long TenantID { get; set; }
        public long TravelRequestID { get; set; }
        public decimal AdvanceAmount { get; set; }
        public long CreatedBy { get; set; }
    }

    // EXPENSE CATEGORY
    public class ExpenseCategoryDto
    {
        public long ExpenseCategoryID { get; set; }
        public long TenantID { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal? MaximumLimit { get; set; }
    }

    public class CreateExpenseCategoryRequest
    {
        public long TenantID { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal? MaximumLimit { get; set; }
        public long CreatedBy { get; set; }
    }

    // EXPENSE CLAIM
    public class ExpenseClaimDto
    {
        public long ExpenseClaimID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public long? TravelRequestID { get; set; }
        public string? TravelDestination { get; set; }
        public DateTime ClaimDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ClaimStatus { get; set; } = string.Empty;
    }

    public class CreateExpenseClaimRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long? TravelRequestID { get; set; }
        public DateTime ClaimDate { get; set; }
        public long CreatedBy { get; set; }
        public List<CreateExpenseClaimItemDto> Items { get; set; } = new();
    }

    public class CreateExpenseClaimItemDto
    {
        public long ExpenseCategoryID { get; set; }
        public decimal ExpenseAmount { get; set; }
        public DateTime ExpenseDate { get; set; }
    }

    public class ApproveExpenseClaimRequest
    {
        public long ExpenseClaimID { get; set; }
        public long TenantID { get; set; }
        public long ApproverID { get; set; }
        public string ApprovalStatus { get; set; } = "Approved"; // 'Approved', 'Rejected'
        public long ModifiedBy { get; set; }
    }

    // EXPENSE SETTLEMENT
    public class ExpenseSettlementDto
    {
        public long ExpenseSettlementID { get; set; }
        public long TenantID { get; set; }
        public long ExpenseClaimID { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal SettlementAmount { get; set; }
        public string SettlementStatus { get; set; } = string.Empty;
    }

    public class ProcessExpenseSettlementRequest
    {
        public long ExpenseClaimID { get; set; }
        public long TenantID { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal SettlementAmount { get; set; }
        public long CreatedBy { get; set; }
    }

    // CORPORATE CARD
    public class CorporateCardTransactionDto
    {
        public long CorporateCardTransactionID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string MerchantName { get; set; } = string.Empty;
        public decimal TransactionAmount { get; set; }
    }

    public class ImportCorporateCardTransactionRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string MerchantName { get; set; } = string.Empty;
        public decimal TransactionAmount { get; set; }
        public long CreatedBy { get; set; }
    }

    public class ReconcileCorporateCardRequest
    {
        public long CorporateCardTransactionID { get; set; }
        public long ExpenseClaimItemID { get; set; }
        public long TenantID { get; set; }
        public long CreatedBy { get; set; }
    }

    // COMPLIANCE
    public class TravelComplianceDto
    {
        public long TravelComplianceID { get; set; }
        public long TenantID { get; set; }
        public long TravelRequestID { get; set; }
        public string ComplianceType { get; set; } = string.Empty;
        public string ComplianceStatus { get; set; } = string.Empty;
        public long EmployeeID { get; set; }
        public string Destination { get; set; } = string.Empty;
        public DateTime TravelStartDate { get; set; }
    }

    // ANALYTICS
    public class TravelAnalyticsSummaryDto
    {
        public long TravelAnalyticsID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public int TotalTrips { get; set; }
        public decimal TotalExpenses { get; set; }
        public string ReportingPeriod { get; set; } = string.Empty;
    }
}
