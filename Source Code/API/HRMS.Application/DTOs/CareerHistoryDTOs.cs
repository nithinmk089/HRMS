using System;

namespace HRMS.Application.DTOs
{
    // --- Transfers ---
    public class EmployeeTransferDto
    {
        public long EmployeeTransferID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeFullName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public long FromDepartmentID { get; set; }
        public string FromDepartmentName { get; set; } = string.Empty;
        public long ToDepartmentID { get; set; }
        public string ToDepartmentName { get; set; } = string.Empty;
        public long FromLocationID { get; set; }
        public string FromLocationName { get; set; } = string.Empty;
        public long ToLocationID { get; set; }
        public string ToLocationName { get; set; } = string.Empty;
        public DateTime EffectiveDate { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }

    public class CreateEmployeeTransferRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public long FromDepartmentId { get; set; }
        public long ToDepartmentId { get; set; }
        public long FromLocationId { get; set; }
        public long ToLocationId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? Reason { get; set; }
        public long CreatedBy { get; set; }
    }

    // --- Promotions ---
    public class EmployeePromotionDto
    {
        public long EmployeePromotionID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeFullName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public long OldDesignationID { get; set; }
        public string OldDesignationName { get; set; } = string.Empty;
        public long NewDesignationID { get; set; }
        public string NewDesignationName { get; set; } = string.Empty;
        public string? OldGrade { get; set; }
        public string? NewGrade { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }

    public class CreateEmployeePromotionRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public long OldDesignationId { get; set; }
        public long NewDesignationId { get; set; }
        public string? OldGrade { get; set; }
        public string? NewGrade { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? Reason { get; set; }
        public long CreatedBy { get; set; }
    }
}