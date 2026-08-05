using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/employees")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeAddressRepository _addressRepository;
        private readonly IEmployeeContactRepository _contactRepository;
        private readonly IEmployeeEmergencyContactRepository _emergencyContactRepository;
        private readonly IEmployeeQualificationRepository _qualificationRepository;
        private readonly IEmployeeCertificationRepository _certificationRepository;
        private readonly IEmployeeDocumentRepository _documentRepository;
        private readonly IEmployeeManagerRepository _managerRepository;

        public EmployeesController(
            IEmployeeRepository employeeRepository,
            IEmployeeAddressRepository addressRepository,
            IEmployeeContactRepository contactRepository,
            IEmployeeEmergencyContactRepository emergencyContactRepository,
            IEmployeeQualificationRepository qualificationRepository,
            IEmployeeCertificationRepository certificationRepository,
            IEmployeeDocumentRepository documentRepository,
            IEmployeeManagerRepository managerRepository)
        {
            _employeeRepository = employeeRepository;
            _addressRepository = addressRepository;
            _contactRepository = contactRepository;
            _emergencyContactRepository = emergencyContactRepository;
            _qualificationRepository = qualificationRepository;
            _certificationRepository = certificationRepository;
            _documentRepository = documentRepository;
            _managerRepository = managerRepository;
        }

        // --- Core Employee CRUD ---

        [HttpPost]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest r)
        {
            r.CreatedBy = 1;
            var id = await _employeeRepository.CreateAsync(r);
            return CreatedAtAction(nameof(GetById), new { id, tenantId = r.TenantId }, ApiResponse<long>.SuccessResult(id, "Employee created."));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateEmployeeRequest r)
        {
            r.EmployeeId = id;
            r.ModifiedBy = 1;
            var ok = await _employeeRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Employee updated."));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long tenantId)
        {
            var ok = await _employeeRepository.DeleteAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Employee deleted."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id, [FromQuery] long tenantId)
        {
            var emp = await _employeeRepository.GetByIdAsync(id, tenantId);
            if (emp == null) return NotFound(ApiResponse<EmployeeDto>.FailureResult("Employee not found."));
            return Ok(ApiResponse<EmployeeDto>.SuccessResult(emp));
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] long tenantId, [FromQuery] string? searchText, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var emps = await _employeeRepository.SearchAsync(tenantId, searchText, status, page, pageSize);
            return Ok(ApiResponse<IEnumerable<EmployeeDirectoryDto>>.SuccessResult(emps));
        }

        // --- Lifecycle Actions ---

        [HttpPut("{id}/activate")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Activate(long id, [FromQuery] long tenantId)
        {
            var ok = await _employeeRepository.ActivateAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Employee activated."));
        }

        [HttpPut("{id}/suspend")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Suspend(long id, [FromQuery] long tenantId, [FromQuery] string? reason)
        {
            var ok = await _employeeRepository.SuspendAsync(id, tenantId, reason, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Employee suspended."));
        }

        [HttpPut("{id}/terminate")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Terminate(long id, [FromQuery] long tenantId, [FromQuery] string? reason)
        {
            var ok = await _employeeRepository.TerminateAsync(id, tenantId, reason, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Employee terminated."));
        }

        [HttpPut("{id}/rehire")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Rehire(long id, [FromQuery] long tenantId, [FromQuery] string? reason)
        {
            var ok = await _employeeRepository.RehireAsync(id, tenantId, reason, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Employee rehired."));
        }

        [HttpPut("{id}/contact-details")]
        public async Task<IActionResult> UpdateContactDetails(long id, [FromBody] UpdateEmployeeRequest r)
        {
            var ok = await _employeeRepository.UpdateContactDetailsAsync(id, r.TenantId, r.PersonalEmail, r.MobileNumber, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Contact details updated."));
        }

        [HttpPut("{id}/profile")]
        public async Task<IActionResult> UpdateProfile(long id, [FromBody] UpdateEmployeeRequest r)
        {
            var ok = await _employeeRepository.UpdateProfileAsync(id, r.TenantId, r.PreferredName, r.MaritalStatus, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Profile updated."));
        }

        // --- Address Management ---

        [HttpGet("{employeeId}/addresses")]
        public async Task<IActionResult> GetAddresses(long employeeId, [FromQuery] long tenantId)
        {
            var addrs = await _addressRepository.GetByEmployeeIdAsync(employeeId, tenantId);
            return Ok(ApiResponse<IEnumerable<EmployeeAddressDto>>.SuccessResult(addrs));
        }

        [HttpPost("{employeeId}/addresses")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> CreateAddress(long employeeId, [FromBody] CreateEmployeeAddressRequest r)
        {
            r.EmployeeId = employeeId;
            r.CreatedBy = 1;
            var id = await _addressRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Address added."));
        }

        [HttpPut("{employeeId}/addresses/{addressId}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> UpdateAddress(long employeeId, long addressId, [FromBody] UpdateEmployeeAddressRequest r)
        {
            r.EmployeeAddressId = addressId;
            r.ModifiedBy = 1;
            var ok = await _addressRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Address updated."));
        }

        [HttpDelete("{employeeId}/addresses/{addressId}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> DeleteAddress(long employeeId, long addressId, [FromQuery] long tenantId)
        {
            var ok = await _addressRepository.DeleteAsync(addressId, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Address deleted."));
        }

        // --- Contact Management ---

        [HttpGet("{employeeId}/contacts")]
        public async Task<IActionResult> GetContacts(long employeeId, [FromQuery] long tenantId)
        {
            var contacts = await _contactRepository.GetByEmployeeIdAsync(employeeId, tenantId);
            return Ok(ApiResponse<IEnumerable<EmployeeContactDto>>.SuccessResult(contacts));
        }

        [HttpPost("{employeeId}/contacts")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> CreateContact(long employeeId, [FromBody] CreateEmployeeContactRequest r)
        {
            r.EmployeeId = employeeId;
            r.CreatedBy = 1;
            var id = await _contactRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Contact added."));
        }

        [HttpPut("{employeeId}/contacts/{contactId}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> UpdateContact(long employeeId, long contactId, [FromBody] UpdateEmployeeContactRequest r)
        {
            r.EmployeeContactId = contactId;
            r.ModifiedBy = 1;
            var ok = await _contactRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Contact updated."));
        }

        [HttpDelete("{employeeId}/contacts/{contactId}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> DeleteContact(long employeeId, long contactId, [FromQuery] long tenantId)
        {
            var ok = await _contactRepository.DeleteAsync(contactId, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Contact deleted."));
        }

        // --- Emergency Contact Management ---

        [HttpGet("{employeeId}/emergency-contacts")]
        public async Task<IActionResult> GetEmergencyContacts(long employeeId, [FromQuery] long tenantId)
        {
            var contacts = await _emergencyContactRepository.GetByEmployeeIdAsync(employeeId, tenantId);
            return Ok(ApiResponse<IEnumerable<EmployeeEmergencyContactDto>>.SuccessResult(contacts));
        }

        [HttpPost("{employeeId}/emergency-contacts")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> CreateEmergencyContact(long employeeId, [FromBody] CreateEmployeeEmergencyContactRequest r)
        {
            r.EmployeeId = employeeId;
            r.CreatedBy = 1;
            var id = await _emergencyContactRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Emergency contact added."));
        }

        [HttpPut("{employeeId}/emergency-contacts/{contactId}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> UpdateEmergencyContact(long employeeId, long contactId, [FromBody] UpdateEmployeeEmergencyContactRequest r)
        {
            r.EmployeeEmergencyContactId = contactId;
            r.ModifiedBy = 1;
            var ok = await _emergencyContactRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Emergency contact updated."));
        }

        [HttpDelete("{employeeId}/emergency-contacts/{contactId}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> DeleteEmergencyContact(long employeeId, long contactId, [FromQuery] long tenantId)
        {
            var ok = await _emergencyContactRepository.DeleteAsync(contactId, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Emergency contact deleted."));
        }

        // --- Qualification Management ---

        [HttpGet("{employeeId}/qualifications")]
        public async Task<IActionResult> GetQualifications(long employeeId, [FromQuery] long tenantId)
        {
            var quals = await _qualificationRepository.GetByEmployeeIdAsync(employeeId, tenantId);
            return Ok(ApiResponse<IEnumerable<EmployeeQualificationDto>>.SuccessResult(quals));
        }

        [HttpPost("{employeeId}/qualifications")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> CreateQualification(long employeeId, [FromBody] CreateEmployeeQualificationRequest r)
        {
            r.EmployeeId = employeeId;
            r.CreatedBy = 1;
            var id = await _qualificationRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Qualification added."));
        }

        [HttpPut("{employeeId}/qualifications/{qualificationId}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> UpdateQualification(long employeeId, long qualificationId, [FromBody] UpdateEmployeeQualificationRequest r)
        {
            r.EmployeeQualificationId = qualificationId;
            r.ModifiedBy = 1;
            var ok = await _qualificationRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Qualification updated."));
        }

        [HttpDelete("{employeeId}/qualifications/{qualificationId}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> DeleteQualification(long employeeId, long qualificationId, [FromQuery] long tenantId)
        {
            var ok = await _qualificationRepository.DeleteAsync(qualificationId, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Qualification deleted."));
        }

        // --- Certification Management ---

        [HttpGet("{employeeId}/certifications")]
        public async Task<IActionResult> GetCertifications(long employeeId, [FromQuery] long tenantId)
        {
            var certs = await _certificationRepository.GetByEmployeeIdAsync(employeeId, tenantId);
            return Ok(ApiResponse<IEnumerable<EmployeeCertificationDto>>.SuccessResult(certs));
        }

        [HttpPost("{employeeId}/certifications")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> CreateCertification(long employeeId, [FromBody] CreateEmployeeCertificationRequest r)
        {
            r.EmployeeId = employeeId;
            r.CreatedBy = 1;
            var id = await _certificationRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Certification added."));
        }

        [HttpPut("{employeeId}/certifications/{certificationId}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> UpdateCertification(long employeeId, long certificationId, [FromBody] UpdateEmployeeCertificationRequest r)
        {
            r.EmployeeCertificationId = certificationId;
            r.ModifiedBy = 1;
            var ok = await _certificationRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Certification updated."));
        }

        [HttpDelete("{employeeId}/certifications/{certificationId}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> DeleteCertification(long employeeId, long certificationId, [FromQuery] long tenantId)
        {
            var ok = await _certificationRepository.DeleteAsync(certificationId, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Certification deleted."));
        }

        // --- Document Management ---

        [HttpGet("{employeeId}/documents")]
        public async Task<IActionResult> GetDocuments(long employeeId, [FromQuery] long tenantId, [FromQuery] string? docType)
        {
            var docs = await _documentRepository.SearchAsync(tenantId, employeeId, docType);
            return Ok(ApiResponse<IEnumerable<EmployeeDocumentDto>>.SuccessResult(docs));
        }

        [HttpPost("{employeeId}/documents")]
        public async Task<IActionResult> UploadDocument(long employeeId, [FromBody] UploadEmployeeDocumentRequest r)
        {
            r.EmployeeId = employeeId;
            r.CreatedBy = 1;
            var id = await _documentRepository.UploadAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Document uploaded."));
        }

        [HttpPut("{employeeId}/documents/{docId}")]
        public async Task<IActionResult> UpdateDocument(long employeeId, long docId, [FromBody] UpdateEmployeeDocumentRequest r)
        {
            r.EmployeeDocumentId = docId;
            r.ModifiedBy = 1;
            var ok = await _documentRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Document updated."));
        }

        [HttpDelete("{employeeId}/documents/{docId}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> DeleteDocument(long employeeId, long docId, [FromQuery] long tenantId)
        {
            var ok = await _documentRepository.DeleteAsync(docId, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Document deleted."));
        }

        [HttpPost("{employeeId}/documents/{docId}/versions")]
        public async Task<IActionResult> CreateDocumentVersion(long employeeId, long docId, [FromQuery] long tenantId, [FromQuery] string fileName, [FromQuery] string filePath, [FromQuery] string mimeType, [FromQuery] int versionNumber)
        {
            var id = await _documentRepository.CreateVersionAsync(docId, tenantId, fileName, filePath, mimeType, versionNumber, 1);
            return Ok(ApiResponse<long>.SuccessResult(id, "Document version created."));
        }

        [HttpPut("{employeeId}/documents/{docId}/versions/{versionNumber}/restore")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> RestoreDocumentVersion(long employeeId, long docId, int versionNumber, [FromQuery] long tenantId)
        {
            var ok = await _documentRepository.RestoreVersionAsync(docId, tenantId, versionNumber, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Document version restored."));
        }

        // --- Manager Assignment ---

        [HttpGet("{employeeId}/managers")]
        public async Task<IActionResult> GetManagers(long employeeId, [FromQuery] long tenantId)
        {
            var managers = await _managerRepository.GetManagersAsync(employeeId, tenantId);
            return Ok(ApiResponse<IEnumerable<EmployeeManagerDto>>.SuccessResult(managers));
        }

        [HttpPost("{employeeId}/managers")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> AssignManager(long employeeId, [FromBody] AssignManagerRequest r)
        {
            r.EmployeeId = employeeId;
            r.CreatedBy = 1;
            var ok = await _managerRepository.AssignAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Manager assigned."));
        }

        [HttpDelete("{employeeId}/managers/{managerId}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> RemoveManager(long employeeId, long managerId, [FromQuery] long tenantId)
        {
            var ok = await _managerRepository.RemoveAsync(employeeId, managerId, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Manager removed."));
        }

        [HttpGet("{employeeId}/direct-reports")]
        public async Task<IActionResult> GetDirectReports(long employeeId, [FromQuery] long tenantId)
        {
            var reports = await _managerRepository.GetDirectReportsAsync(employeeId, tenantId);
            return Ok(ApiResponse<IEnumerable<EmployeeDirectoryDto>>.SuccessResult(reports));
        }

        // --- Reports & Diagnostics ---

        [HttpGet("{id}/service-history")]
        public async Task<IActionResult> GetServiceHistory(long id, [FromQuery] long tenantId)
        {
            var history = await _employeeRepository.GetServiceHistoryReportAsync(tenantId, id);
            return Ok(ApiResponse<IEnumerable<EmployeeServiceHistoryReportDto>>.SuccessResult(history));
        }

        [HttpGet("directory-report")]
        public async Task<IActionResult> GetDirectoryReport([FromQuery] long tenantId, [FromQuery] long? departmentId, [FromQuery] long? locationId, [FromQuery] string? status)
        {
            var directory = await _employeeRepository.GetDirectoryReportAsync(tenantId, departmentId, locationId, status);
            return Ok(ApiResponse<IEnumerable<EmployeeDirectoryDto>>.SuccessResult(directory));
        }

        [HttpGet("certification-expiry")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> GetCertificationExpiry([FromQuery] long tenantId, [FromQuery] int withinDays = 30)
        {
            var expiryReport = await _certificationRepository.GetCertificationExpiryReportAsync(tenantId, withinDays);
            return Ok(ApiResponse<IEnumerable<CertificationExpiryReportDto>>.SuccessResult(expiryReport));
        }
    }
}
