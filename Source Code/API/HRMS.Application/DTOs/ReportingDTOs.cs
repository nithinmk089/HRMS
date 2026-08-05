using System;

namespace HRMS.Application.DTOs
{
    // --- Service History Report ---
    public class EmployeeServiceHistoryReportDto
    {
        public long RecordID { get; set; }
        public string RecordType { get; set; } = string.Empty; // StatusChange, Transfer, Promotion
        public string DetailText { get; set; } = string.Empty;
        public DateTime EffectiveDate { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    // --- Certification Expiry Report ---
    public class CertificationExpiryReportDto
    {
        public long DocumentID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeFullName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string DocumentCategory { get; set; } = string.Empty;
        public string DocumentName { get; set; } = string.Empty;
        public string? DocumentNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int DaysToExpiry { get; set; }
    }
}