using System;

namespace HRMS.Application.DTOs
{
    // --- Employee ---
    public class EmployeeDto
    {
        public long EmployeeID { get; set; }
        public long TenantID { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? PreferredName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Nationality { get; set; }
        public string? PersonalEmail { get; set; }
        public string? MobileNumber { get; set; }
        public string EmployeeStatus { get; set; } = string.Empty;
        public string? EmploymentType { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? ProbationEndDate { get; set; }
        public int? NoticePeriodDays { get; set; }
        public string? EmploymentStatus { get; set; }
        public long? CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public long? BusinessUnitID { get; set; }
        public string? BusinessUnitName { get; set; }
        public long? DepartmentID { get; set; }
        public string? DepartmentName { get; set; }
        public long? DesignationID { get; set; }
        public string? DesignationName { get; set; }
        public string? Grade { get; set; }
        public long? LocationID { get; set; }
        public string? LocationName { get; set; }
        public long? CostCenterID { get; set; }
        public string? CostCenterName { get; set; }
        public long? ManagerID { get; set; }
        public string? ManagerFullName { get; set; }
    }

    public class EmployeeDirectoryDto
    {
        public long EmployeeID { get; set; }
        public long TenantID { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? PersonalEmail { get; set; }
        public string? MobileNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? DepartmentName { get; set; }
        public string? DesignationName { get; set; }
        public string? LocationName { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string? EmploymentType { get; set; }
    }

    public class CreateEmployeeRequest
    {
        public long TenantId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? PreferredName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Nationality { get; set; }
        public string? PersonalEmail { get; set; }
        public string? MobileNumber { get; set; }
        public string Status { get; set; } = "Active";
        public long CreatedBy { get; set; }
    }

    public class UpdateEmployeeRequest
    {
        public long EmployeeId { get; set; }
        public long TenantId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? PreferredName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Nationality { get; set; }
        public string? PersonalEmail { get; set; }
        public string? MobileNumber { get; set; }
        public string Status { get; set; } = "Active";
        public long ModifiedBy { get; set; }
    }

    // --- Employment ---
    public class EmployeeEmploymentDto
    {
        public long EmployeeEmploymentID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeFullName { get; set; } = string.Empty;
        public long CompanyID { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public long BusinessUnitID { get; set; }
        public string BusinessUnitName { get; set; } = string.Empty;
        public long DepartmentID { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public long DesignationID { get; set; }
        public string DesignationName { get; set; } = string.Empty;
        public long LocationID { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public long CostCenterID { get; set; }
        public string CostCenterName { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? ProbationEndDate { get; set; }
        public int NoticePeriodDays { get; set; }
        public string EmploymentStatus { get; set; } = string.Empty;
    }

    public class CreateEmployeeEmploymentRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public long CompanyId { get; set; }
        public long BusinessUnitId { get; set; }
        public long DepartmentId { get; set; }
        public long DesignationId { get; set; }
        public long LocationId { get; set; }
        public long CostCenterId { get; set; }
        public string EmploymentType { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? ProbationEndDate { get; set; }
        public int NoticePeriodDays { get; set; }
        public string EmploymentStatus { get; set; } = "Active";
        public long CreatedBy { get; set; }
    }

    public class UpdateEmployeeEmploymentRequest
    {
        public long EmployeeEmploymentId { get; set; }
        public long TenantId { get; set; }
        public long CompanyId { get; set; }
        public long BusinessUnitId { get; set; }
        public long DepartmentId { get; set; }
        public long DesignationId { get; set; }
        public long LocationId { get; set; }
        public long CostCenterId { get; set; }
        public string EmploymentType { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? ProbationEndDate { get; set; }
        public int NoticePeriodDays { get; set; }
        public string EmploymentStatus { get; set; } = "Active";
        public long ModifiedBy { get; set; }
    }

    // --- Address ---
    public class EmployeeAddressDto
    {
        public long EmployeeAddressID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string AddressType { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string? State { get; set; }
        public string Country { get; set; } = string.Empty;
        public string? ZipCode { get; set; }
    }

    public class CreateEmployeeAddressRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public string AddressType { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string? State { get; set; }
        public string Country { get; set; } = string.Empty;
        public string? ZipCode { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateEmployeeAddressRequest
    {
        public long EmployeeAddressId { get; set; }
        public long TenantId { get; set; }
        public string AddressType { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string? State { get; set; }
        public string Country { get; set; } = string.Empty;
        public string? ZipCode { get; set; }
        public long ModifiedBy { get; set; }
    }

    // --- Contact ---
    public class EmployeeContactDto
    {
        public long EmployeeContactID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string ContactType { get; set; } = string.Empty;
        public string ContactValue { get; set; } = string.Empty;
    }

    public class CreateEmployeeContactRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public string ContactType { get; set; } = string.Empty;
        public string ContactValue { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    public class UpdateEmployeeContactRequest
    {
        public long EmployeeContactId { get; set; }
        public long TenantId { get; set; }
        public string ContactType { get; set; } = string.Empty;
        public string ContactValue { get; set; } = string.Empty;
        public long ModifiedBy { get; set; }
    }

    // --- Emergency Contact ---
    public class EmployeeEmergencyContactDto
    {
        public long EmployeeEmergencyContactID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string ContactName { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
    }

    public class CreateEmployeeEmergencyContactRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public string ContactName { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateEmployeeEmergencyContactRequest
    {
        public long EmployeeEmergencyContactId { get; set; }
        public long TenantId { get; set; }
        public string ContactName { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public long ModifiedBy { get; set; }
    }

    // --- Qualification ---
    public class EmployeeQualificationDto
    {
        public long EmployeeQualificationID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string QualificationType { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public string? University { get; set; }
        public int YearOfPassing { get; set; }
        public decimal? Percentage { get; set; }
    }

    public class CreateEmployeeQualificationRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public string QualificationType { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public string? University { get; set; }
        public int YearOfPassing { get; set; }
        public decimal? Percentage { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateEmployeeQualificationRequest
    {
        public long EmployeeQualificationId { get; set; }
        public long TenantId { get; set; }
        public string QualificationType { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public string? University { get; set; }
        public int YearOfPassing { get; set; }
        public decimal? Percentage { get; set; }
        public long ModifiedBy { get; set; }
    }

    // --- Certification ---
    public class EmployeeCertificationDto
    {
        public long EmployeeCertificationID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string CertificationName { get; set; } = string.Empty;
        public string CertificationAuthority { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? CertificateNumber { get; set; }
    }

    public class CreateEmployeeCertificationRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public string CertificationName { get; set; } = string.Empty;
        public string CertificationAuthority { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? CertificateNumber { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateEmployeeCertificationRequest
    {
        public long EmployeeCertificationId { get; set; }
        public long TenantId { get; set; }
        public string CertificationName { get; set; } = string.Empty;
        public string CertificationAuthority { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? CertificateNumber { get; set; }
        public long ModifiedBy { get; set; }
    }

    // --- Document ---
    public class EmployeeDocumentDto
    {
        public long EmployeeDocumentID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeFullName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public int VersionNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UploadEmployeeDocumentRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    public class UpdateEmployeeDocumentRequest
    {
        public long EmployeeDocumentId { get; set; }
        public long TenantId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public long ModifiedBy { get; set; }
    }

    // --- Manager / Hierarchy ---
    public class EmployeeManagerDto
    {
        public long EmployeeManagerID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeFullName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public long ManagerID { get; set; }
        public string ManagerFullName { get; set; } = string.Empty;
        public string ManagerCode { get; set; } = string.Empty;
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }

    public class AssignManagerRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public long ManagerId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public long CreatedBy { get; set; }
    }

    // --- Status History ---
    public class EmployeeStatusHistoryDto
    {
        public long EmployeeStatusHistoryID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime EffectiveDate { get; set; }
        public string? Reason { get; set; }
    }
}