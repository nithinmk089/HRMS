using System.Collections.Generic;

namespace HRMS.Application.DTOs
{
    public class ImportResultDto
    {
        public bool Success { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public int TotalRows { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public List<ImportRowErrorDto> Errors { get; set; } = new List<ImportRowErrorDto>();
        public string Message { get; set; } = string.Empty;
    }

    public class ImportRowErrorDto
    {
        public int RowIndex { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class MasterDataImportItem
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ParentCode { get; set; }
        public string? Category { get; set; }
    }

    public class EmployeeImportItem
    {
        public string? EmployeeCode { get; set; }
        public string? EmployeeNumber { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PersonalEmail { get; set; }
        public string? MobileNumber { get; set; }
        public string? Gender { get; set; }
        public string? DepartmentCode { get; set; }
        public string? DesignationCode { get; set; }
        public string? LocationCode { get; set; }
        public string? Status { get; set; }
    }

    public class OperationalImportItem
    {
        public string? EmployeeCode { get; set; }
        public string? Code { get; set; }
        public string? Date { get; set; }
        public decimal Value { get; set; }
        public string? Remarks { get; set; }
    }
}
