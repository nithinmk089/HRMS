import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  ApiResponse, Tenant, Company, BusinessUnit, Department, Designation,
  Location, CostCenter, User, Role, Permission, SystemConfiguration
} from '../models/phase01.models';
import {
  Employee, EmployeeDto, EmployeeDirectoryDto, EmployeeEmployment, EmployeeAddress,
  EmployeeContact, EmployeeEmergencyContact, EmployeeQualification, EmployeeCertification,
  EmployeeDocument, EmployeeManager, EmployeeTransfer, EmployeePromotion,
  EmployeeServiceHistoryReport, CertificationExpiryReport
} from '../models/employee.models';
import {
  Shift, ShiftAssignment, Attendance, AttendanceAdjustment, AttendanceRegularization,
  HolidayCalendar, Holiday, LeaveType, LeavePolicy, LeaveBalance, LeaveRequest,
  LeaveEncashment, OvertimeRequest
} from '../models/time-attendance.models';
import {
  OnboardingWorkflowDto, DocumentSubmissionDto, OnboardingStatusReportDto, PendingTasksReportDto,
  ExitStatusReportDto, ClearanceStatusReportDto, FullAndFinalSummaryReportDto
} from '../models/onboarding-offboarding.models';
import {
  AssetCategory, Asset, AssetAssignment, AssetTransfer, AssetMaintenance, AssetRepair,
  AssetWarranty, WarrantyExpiryReport, AssetDepreciation, AssetDepreciationReport,
  AssetAudit, AssetAuditReport, AssetDisposal, AssetReturnWorkflow, AssetInventory
} from '../models/asset.models';


@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  // --- Auth ---
  login(email: string, password: string): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/auth/login`, { email, password });
  }

  logout(): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/auth/logout`, {});
  }

  // --- Tenants ---
  getTenants(searchText?: string, status?: string, page: number = 1, pageSize: number = 50): Observable<ApiResponse<Tenant[]>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    if (searchText) params = params.set('searchText', searchText);
    if (status) params = params.set('status', status);
    return this.http.get<ApiResponse<Tenant[]>>(`${this.baseUrl}/tenants`, { params });
  }

  getTenant(id: number): Observable<ApiResponse<Tenant>> {
    return this.http.get<ApiResponse<Tenant>>(`${this.baseUrl}/tenants/${id}`);
  }

  createTenant(tenant: Partial<Tenant>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/tenants`, tenant);
  }

  updateTenant(id: number, tenant: Partial<Tenant>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/tenants/${id}`, tenant);
  }

  deleteTenant(id: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/tenants/${id}`);
  }

  activateTenant(id: number): Observable<ApiResponse<boolean>> {
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/tenants/${id}/activate`, {});
  }

  deactivateTenant(id: number): Observable<ApiResponse<boolean>> {
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/tenants/${id}/deactivate`, {});
  }

  // --- Companies ---
  getCompanies(tenantId: number, searchText?: string, page: number = 1, pageSize: number = 50): Observable<ApiResponse<Company[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString()).set('page', page.toString()).set('pageSize', pageSize.toString());
    if (searchText) params = params.set('searchText', searchText);
    return this.http.get<ApiResponse<Company[]>>(`${this.baseUrl}/companies`, { params });
  }

  getCompany(id: number, tenantId: number): Observable<ApiResponse<Company>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<Company>>(`${this.baseUrl}/companies/${id}`, { params });
  }

  createCompany(company: Partial<Company>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/companies`, company);
  }

  updateCompany(id: number, company: Partial<Company>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/companies/${id}`, company);
  }

  deleteCompany(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/companies/${id}`, { params });
  }

  // --- Business Units ---
  getBusinessUnits(tenantId: number, companyId?: number, searchText?: string): Observable<ApiResponse<BusinessUnit[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (companyId) params = params.set('companyId', companyId.toString());
    if (searchText) params = params.set('searchText', searchText);
    return this.http.get<ApiResponse<BusinessUnit[]>>(`${this.baseUrl}/business-units`, { params });
  }

  createBusinessUnit(bu: Partial<BusinessUnit>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/business-units`, bu);
  }

  updateBusinessUnit(id: number, bu: Partial<BusinessUnit>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/business-units/${id}`, bu);
  }

  deleteBusinessUnit(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/business-units/${id}`, { params });
  }

  // --- Departments ---
  getDepartments(tenantId: number, businessUnitId?: number, searchText?: string): Observable<ApiResponse<Department[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (businessUnitId) params = params.set('businessUnitId', businessUnitId.toString());
    if (searchText) params = params.set('searchText', searchText);
    return this.http.get<ApiResponse<Department[]>>(`${this.baseUrl}/departments`, { params });
  }

  createDepartment(dept: Partial<Department>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/departments`, dept);
  }

  updateDepartment(id: number, dept: Partial<Department>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/departments/${id}`, dept);
  }

  deleteDepartment(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/departments/${id}`, { params });
  }

  // --- Designations ---
  getDesignations(tenantId: number, searchText?: string): Observable<ApiResponse<Designation[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (searchText) params = params.set('searchText', searchText);
    return this.http.get<ApiResponse<Designation[]>>(`${this.baseUrl}/designations`, { params });
  }

  createDesignation(desg: Partial<Designation>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/designations`, desg);
  }

  updateDesignation(id: number, desg: Partial<Designation>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/designations/${id}`, desg);
  }

  deleteDesignation(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/designations/${id}`, { params });
  }

  // --- Locations ---
  getLocations(tenantId: number, searchText?: string): Observable<ApiResponse<Location[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (searchText) params = params.set('searchText', searchText);
    return this.http.get<ApiResponse<Location[]>>(`${this.baseUrl}/locations`, { params });
  }

  createLocation(loc: Partial<Location>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/locations`, loc);
  }

  updateLocation(id: number, loc: Partial<Location>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/locations/${id}`, loc);
  }

  deleteLocation(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/locations/${id}`, { params });
  }

  // --- Cost Centers ---
  getCostCenters(tenantId: number, searchText?: string): Observable<ApiResponse<CostCenter[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (searchText) params = params.set('searchText', searchText);
    return this.http.get<ApiResponse<CostCenter[]>>(`${this.baseUrl}/cost-centers`, { params });
  }

  createCostCenter(cc: Partial<CostCenter>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/cost-centers`, cc);
  }

  updateCostCenter(id: number, cc: Partial<CostCenter>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/cost-centers/${id}`, cc);
  }

  deleteCostCenter(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/cost-centers/${id}`, { params });
  }

  // --- Users ---
  getUsers(tenantId: number, searchText?: string): Observable<ApiResponse<User[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (searchText) params = params.set('searchText', searchText);
    return this.http.get<ApiResponse<User[]>>(`${this.baseUrl}/users`, { params });
  }

  getUser(id: number, tenantId: number): Observable<ApiResponse<User>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<User>>(`${this.baseUrl}/users/${id}`, { params });
  }

  createUser(user: Partial<User> & { password?: string }): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/users`, user);
  }

  updateUser(id: number, user: Partial<User>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/users/${id}`, user);
  }

  deleteUser(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/users/${id}`, { params });
  }

  lockUser(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/users/${id}/lock`, {}, { params });
  }

  unlockUser(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/users/${id}/unlock`, {}, { params });
  }

  // --- Roles ---
  getRoles(tenantId: number, searchText?: string): Observable<ApiResponse<Role[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (searchText) params = params.set('searchText', searchText);
    return this.http.get<ApiResponse<Role[]>>(`${this.baseUrl}/roles`, { params });
  }

  getRoleUserIds(roleId: number, tenantId: number): Observable<ApiResponse<number[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<number[]>>(`${this.baseUrl}/roles/${roleId}/users`, { params });
  }

  createRole(role: Partial<Role>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/roles`, role);
  }

  updateRole(id: number, role: Partial<Role>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/roles/${id}`, role);
  }

  deleteRole(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/roles/${id}`, { params });
  }

  assignRole(roleId: number, userId: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('userId', userId.toString()).set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/roles/${roleId}/assign-user`, {}, { params });
  }

  removeRole(roleId: number, userId: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('userId', userId.toString()).set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/roles/${roleId}/remove-user`, {}, { params });
  }

  // --- Permissions ---
  getPermissions(tenantId: number, searchText?: string): Observable<ApiResponse<Permission[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (searchText) params = params.set('searchText', searchText);
    return this.http.get<ApiResponse<Permission[]>>(`${this.baseUrl}/permissions`, { params });
  }

  createPermission(perm: Partial<Permission>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/permissions`, perm);
  }

  updatePermission(id: number, perm: Partial<Permission>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/permissions/${id}`, perm);
  }

  deletePermission(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/permissions/${id}`, { params });
  }

  assignPermissionToRole(tenantId: number, roleId: number, permissionId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString()).set('roleId', roleId.toString()).set('permissionId', permissionId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/permissions/assign-role`, {}, { params });
  }

  removePermissionFromRole(tenantId: number, roleId: number, permissionId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString()).set('roleId', roleId.toString()).set('permissionId', permissionId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/permissions/remove-role`, {}, { params });
  }

  // --- Data Import ---
  downloadImportTemplate(entityType: string): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/import/template/${entityType}`, { responseType: 'blob' });
  }

  uploadEmployeeData(file: File): Observable<ApiResponse<any>> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/import/employees`, formData);
  }

  uploadMasterData(entityType: string, file: File): Observable<ApiResponse<any>> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/import/master-data/${entityType}`, formData);
  }

  uploadOperationalData(entityType: string, file: File): Observable<ApiResponse<any>> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/import/operational/${entityType}`, formData);
  }

  // --- Configurations ---
  getConfigurations(tenantId: number, searchText?: string): Observable<ApiResponse<SystemConfiguration[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (searchText) params = params.set('searchText', searchText);
    return this.http.get<ApiResponse<SystemConfiguration[]>>(`${this.baseUrl}/configurations`, { params });
  }

  createConfiguration(cfg: Partial<SystemConfiguration>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/configurations`, cfg);
  }

  updateConfiguration(id: number, cfg: Partial<SystemConfiguration>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/configurations/${id}`, cfg);
  }

  deleteConfiguration(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/configurations/${id}`, { params });
  }

  // --- Administration Email & Notification Settings ---
  getEmailSettings(tenantId: number): Observable<ApiResponse<any>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any>>(`${this.baseUrl}/notifications/email-settings`, { params });
  }

  saveEmailSettings(tenantId: number, settings: any): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/notifications/email-settings`, settings, { params });
  }

  sendTestEmail(tenantId: number, recipient: string): Observable<ApiResponse<any>> {
    const params = new HttpParams().set('tenantId', tenantId.toString()).set('recipient', recipient);
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/notifications/test-email`, {}, { params });
  }

  broadcastLiveNotification(tenantId: number, subject: string, body: string): Observable<ApiResponse<boolean>> {
    const params = new HttpParams()
      .set('tenantId', tenantId.toString())
      .set('subject', subject)
      .set('body', body);
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/notifications/broadcast-live`, {}, { params });
  }


  // ==========================================
  // --- PHASE 2 CORE HR & LIFECYCLE ---
  // ==========================================

  // --- Employees ---
  getEmployees(tenantId: number, searchText?: string, status?: string, page: number = 1, pageSize: number = 50): Observable<ApiResponse<EmployeeDirectoryDto[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString()).set('page', page.toString()).set('pageSize', pageSize.toString());
    if (searchText) params = params.set('searchText', searchText);
    if (status) params = params.set('status', status);
    return this.http.get<ApiResponse<EmployeeDirectoryDto[]>>(`${this.baseUrl}/employees`, { params });
  }

  getEmployee(id: number, tenantId: number): Observable<ApiResponse<EmployeeDto>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<EmployeeDto>>(`${this.baseUrl}/employees/${id}`, { params });
  }

  createEmployee(emp: Partial<Employee>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/employees`, emp);
  }

  updateEmployee(id: number, emp: Partial<Employee>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/employees/${id}`, emp);
  }

  deleteEmployee(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/employees/${id}`, { params });
  }

  activateEmployee(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/employees/${id}/activate`, {}, { params });
  }

  suspendEmployee(id: number, tenantId: number, reason?: string): Observable<ApiResponse<boolean>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (reason) params = params.set('reason', reason);
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/employees/${id}/suspend`, {}, { params });
  }

  terminateEmployee(id: number, tenantId: number, reason?: string): Observable<ApiResponse<boolean>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (reason) params = params.set('reason', reason);
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/employees/${id}/terminate`, {}, { params });
  }

  rehireEmployee(id: number, tenantId: number, reason?: string): Observable<ApiResponse<boolean>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (reason) params = params.set('reason', reason);
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/employees/${id}/rehire`, {}, { params });
  }

  updateEmployeeContactDetails(id: number, tenantId: number, personalEmail?: string, mobileNumber?: string): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/employees/${id}/contact-details`, { tenantId, personalEmail, mobileNumber });
  }

  updateEmployeeProfile(id: number, tenantId: number, preferredName?: string, maritalStatus?: string): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/employees/${id}/profile`, { tenantId, preferredName, maritalStatus });
  }

  // --- Employment ---
  getEmploymentDetails(employeeId: number, tenantId: number): Observable<ApiResponse<EmployeeEmployment>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<EmployeeEmployment>>(`${this.baseUrl}/employment/${employeeId}`, { params });
  }

  createEmploymentDetails(details: Partial<EmployeeEmployment>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/employment`, details);
  }

  updateEmploymentDetails(id: number, details: Partial<EmployeeEmployment>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/employment/${id}`, details);
  }

  // --- Addresses ---
  getAddresses(employeeId: number, tenantId: number): Observable<ApiResponse<EmployeeAddress[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<EmployeeAddress[]>>(`${this.baseUrl}/employees/${employeeId}/addresses`, { params });
  }

  createAddress(employeeId: number, addr: Partial<EmployeeAddress>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/employees/${employeeId}/addresses`, addr);
  }

  updateAddress(employeeId: number, addressId: number, addr: Partial<EmployeeAddress>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/employees/${employeeId}/addresses/${addressId}`, addr);
  }

  deleteAddress(employeeId: number, addressId: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/employees/${employeeId}/addresses/${addressId}`, { params });
  }

  // --- Contacts ---
  getContacts(employeeId: number, tenantId: number): Observable<ApiResponse<EmployeeContact[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<EmployeeContact[]>>(`${this.baseUrl}/employees/${employeeId}/contacts`, { params });
  }

  createContact(employeeId: number, contact: Partial<EmployeeContact>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/employees/${employeeId}/contacts`, contact);
  }

  updateContact(employeeId: number, contactId: number, contact: Partial<EmployeeContact>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/employees/${employeeId}/contacts/${contactId}`, contact);
  }

  deleteContact(employeeId: number, contactId: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/employees/${employeeId}/contacts/${contactId}`, { params });
  }

  // --- Emergency Contacts ---
  getEmergencyContacts(employeeId: number, tenantId: number): Observable<ApiResponse<EmployeeEmergencyContact[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<EmployeeEmergencyContact[]>>(`${this.baseUrl}/employees/${employeeId}/emergency-contacts`, { params });
  }

  createEmergencyContact(employeeId: number, contact: Partial<EmployeeEmergencyContact>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/employees/${employeeId}/emergency-contacts`, contact);
  }

  updateEmergencyContact(employeeId: number, contactId: number, contact: Partial<EmployeeEmergencyContact>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/employees/${employeeId}/emergency-contacts/${contactId}`, contact);
  }

  deleteEmergencyContact(employeeId: number, contactId: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/employees/${employeeId}/emergency-contacts/${contactId}`, { params });
  }

  // --- Qualifications ---
  getQualifications(employeeId: number, tenantId: number): Observable<ApiResponse<EmployeeQualification[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<EmployeeQualification[]>>(`${this.baseUrl}/employees/${employeeId}/qualifications`, { params });
  }

  createQualification(employeeId: number, qual: Partial<EmployeeQualification>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/employees/${employeeId}/qualifications`, qual);
  }

  updateQualification(employeeId: number, qualificationId: number, qual: Partial<EmployeeQualification>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/employees/${employeeId}/qualifications/${qualificationId}`, qual);
  }

  deleteQualification(employeeId: number, qualificationId: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/employees/${employeeId}/qualifications/${qualificationId}`, { params });
  }

  // --- Certifications ---
  getCertifications(employeeId: number, tenantId: number): Observable<ApiResponse<EmployeeCertification[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<EmployeeCertification[]>>(`${this.baseUrl}/employees/${employeeId}/certifications`, { params });
  }

  createCertification(employeeId: number, cert: Partial<EmployeeCertification>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/employees/${employeeId}/certifications`, cert);
  }

  updateCertification(employeeId: number, certificationId: number, cert: Partial<EmployeeCertification>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/employees/${employeeId}/certifications/${certificationId}`, cert);
  }

  deleteCertification(employeeId: number, certificationId: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/employees/${employeeId}/certifications/${certificationId}`, { params });
  }

  getCertificationExpiryReport(tenantId: number, withinDays: number = 30): Observable<ApiResponse<CertificationExpiryReport[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString()).set('withinDays', withinDays.toString());
    return this.http.get<ApiResponse<CertificationExpiryReport[]>>(`${this.baseUrl}/employees/certification-expiry`, { params });
  }

  // --- Documents ---
  getDocuments(employeeId: number, tenantId: number, docType?: string): Observable<ApiResponse<EmployeeDocument[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (docType) params = params.set('docType', docType);
    return this.http.get<ApiResponse<EmployeeDocument[]>>(`${this.baseUrl}/employees/${employeeId}/documents`, { params });
  }

  uploadDocument(employeeId: number, doc: Partial<EmployeeDocument>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/employees/${employeeId}/documents`, doc);
  }

  updateDocument(employeeId: number, docId: number, doc: Partial<EmployeeDocument>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/employees/${employeeId}/documents/${docId}`, doc);
  }

  deleteDocument(employeeId: number, docId: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/employees/${employeeId}/documents/${docId}`, { params });
  }

  createDocumentVersion(employeeId: number, docId: number, tenantId: number, fileName: string, filePath: string, mimeType: string, versionNumber: number): Observable<ApiResponse<number>> {
    const params = new HttpParams()
      .set('tenantId', tenantId.toString())
      .set('fileName', fileName)
      .set('filePath', filePath)
      .set('mimeType', mimeType)
      .set('versionNumber', versionNumber.toString());
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/employees/${employeeId}/documents/${docId}/versions`, {}, { params });
  }

  restoreDocumentVersion(employeeId: number, docId: number, versionNumber: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/employees/${employeeId}/documents/${docId}/versions/${versionNumber}/restore`, {}, { params });
  }

  // --- Managers ---
  getManagers(employeeId: number, tenantId: number): Observable<ApiResponse<EmployeeManager[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<EmployeeManager[]>>(`${this.baseUrl}/employees/${employeeId}/managers`, { params });
  }

  assignManager(employeeId: number, assign: { tenantId: number, managerId: number, effectiveFrom: string }): Observable<ApiResponse<boolean>> {
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/employees/${employeeId}/managers`, assign);
  }

  removeManager(employeeId: number, managerId: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/employees/${employeeId}/managers/${managerId}`, { params });
  }

  getDirectReports(employeeId: number, tenantId: number): Observable<ApiResponse<EmployeeDirectoryDto[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<EmployeeDirectoryDto[]>>(`${this.baseUrl}/employees/${employeeId}/direct-reports`, { params });
  }

  // --- Transfers ---
  createTransfer(transfer: { tenantId: number, employeeId: number, fromDepartmentId: number, toDepartmentId: number, fromLocationId: number, toLocationId: number, effectiveDate: string, reason?: string }): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/transfers`, transfer);
  }

  approveTransfer(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/transfers/${id}/approve`, {}, { params });
  }

  completeTransfer(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/transfers/${id}/complete`, {}, { params });
  }

  getTransferHistoryReport(tenantId: number, employeeId?: number): Observable<ApiResponse<EmployeeTransfer[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    return this.http.get<ApiResponse<EmployeeTransfer[]>>(`${this.baseUrl}/transfers/report`, { params });
  }

  // --- Promotions ---
  createPromotion(promotion: { tenantId: number, employeeId: number, oldDesignationId: number, newDesignationId: number, oldGrade?: string, newGrade?: string, effectiveDate: string, reason?: string }): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/promotions`, promotion);
  }

  approvePromotion(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/promotions/${id}/approve`, {}, { params });
  }

  completePromotion(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/promotions/${id}/complete`, {}, { params });
  }

  getPromotionHistoryReport(tenantId: number, employeeId?: number): Observable<ApiResponse<EmployeePromotion[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    return this.http.get<ApiResponse<EmployeePromotion[]>>(`${this.baseUrl}/promotions/report`, { params });
  }

  // --- Service History ---
  getServiceHistoryReport(employeeId: number, tenantId: number): Observable<ApiResponse<EmployeeServiceHistoryReport[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<EmployeeServiceHistoryReport[]>>(`${this.baseUrl}/employees/${employeeId}/service-history`, { params });
  }

  // --- Directory Report ---
  getEmployeeDirectoryReport(tenantId: number, departmentId?: number, locationId?: number, status?: string): Observable<ApiResponse<EmployeeDirectoryDto[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (departmentId) params = params.set('departmentId', departmentId.toString());
    if (locationId) params = params.set('locationId', locationId.toString());
    if (status) params = params.set('status', status);
    return this.http.get<ApiResponse<EmployeeDirectoryDto[]>>(`${this.baseUrl}/employees/directory-report`, { params });
  }

  // --- Shifts ---
  getShifts(tenantId: number, searchTerm?: string): Observable<ApiResponse<Shift[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (searchTerm) params = params.set('searchTerm', searchTerm);
    return this.http.get<ApiResponse<Shift[]>>(`${this.baseUrl}/shifts`, { params });
  }

  createShift(shift: Partial<Shift>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/shifts`, shift);
  }

  updateShift(id: number, shift: Partial<Shift>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/shifts/${id}`, shift);
  }

  deleteShift(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/shifts/${id}`, { params });
  }

  assignShift(assignment: Partial<ShiftAssignment>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/shifts/assignments`, assignment);
  }

  // --- Attendance ---
  clockIn(request: Partial<Attendance>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/attendance/clock-in`, request);
  }

  clockOut(request: Partial<Attendance>): Observable<ApiResponse<boolean>> {
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/attendance/clock-out`, request);
  }

  getAttendanceRecords(tenantId: number, employeeId?: number, startDate?: string, endDate?: string): Observable<ApiResponse<Attendance[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    if (startDate) params = params.set('startDate', startDate);
    if (endDate) params = params.set('endDate', endDate);
    return this.http.get<ApiResponse<Attendance[]>>(`${this.baseUrl}/attendance`, { params });
  }

  recalculate(tenantId: number, employeeId: number, attendanceDate: string): Observable<ApiResponse<boolean>> {
    let params = new HttpParams()
      .set('tenantId', tenantId.toString())
      .set('employeeId', employeeId.toString())
      .set('attendanceDate', attendanceDate);
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/attendance/recalculate`, {}, { params });
  }

  createRegularization(request: Partial<AttendanceRegularization>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/attendance/regularizations`, request);
  }

  approveRegularization(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/attendance/regularizations/${id}/approve`, {}, { params });
  }

  rejectRegularization(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/attendance/regularizations/${id}/reject`, {}, { params });
  }

  // --- Leaves ---
  getLeaveTypes(tenantId: number, searchTerm?: string): Observable<ApiResponse<LeaveType[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (searchTerm) params = params.set('searchTerm', searchTerm);
    return this.http.get<ApiResponse<LeaveType[]>>(`${this.baseUrl}/leaves/types`, { params });
  }

  createLeaveType(leaveType: Partial<LeaveType>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/leaves/types`, leaveType);
  }

  createLeavePolicy(policy: Partial<LeavePolicy>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/leaves/policies`, policy);
  }

  getLeaveBalances(employeeId: number, tenantId: number): Observable<ApiResponse<LeaveBalance[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<LeaveBalance[]>>(`${this.baseUrl}/leaves/balances/${employeeId}`, { params });
  }

  submitLeaveRequest(request: Partial<LeaveRequest>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/leaves/requests`, request);
  }

  getLeaveRequests(tenantId: number, employeeId?: number, status?: string): Observable<ApiResponse<LeaveRequest[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    if (status) params = params.set('status', status);
    return this.http.get<ApiResponse<LeaveRequest[]>>(`${this.baseUrl}/leaves/requests`, { params });
  }

  approveLeaveRequest(id: number, tenantId: number, remarks?: string): Observable<ApiResponse<boolean>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (remarks) params = params.set('remarks', remarks);
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/leaves/requests/${id}/approve`, {}, { params });
  }

  rejectLeaveRequest(id: number, tenantId: number, remarks?: string): Observable<ApiResponse<boolean>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (remarks) params = params.set('remarks', remarks);
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/leaves/requests/${id}/reject`, {}, { params });
  }

  // --- Overtime ---
  submitOvertimeRequest(request: Partial<OvertimeRequest>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/overtime`, request);
  }

  getOvertimeRequests(tenantId: number, employeeId?: number, status?: string): Observable<ApiResponse<OvertimeRequest[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    if (status) params = params.set('status', status);
    return this.http.get<ApiResponse<OvertimeRequest[]>>(`${this.baseUrl}/overtime`, { params });
  }

  approveOvertimeRequest(id: number, tenantId: number, remarks?: string): Observable<ApiResponse<boolean>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (remarks) params = params.set('remarks', remarks);
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/overtime/${id}/approve`, {}, { params });
  }

  rejectOvertimeRequest(id: number, tenantId: number, remarks?: string): Observable<ApiResponse<boolean>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (remarks) params = params.set('remarks', remarks);
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/overtime/${id}/reject`, {}, { params });
  }

  // --- Self Service (Time & Attendance) ---
  getSelfAttendance(employeeId: number, tenantId: number, startDate?: string, endDate?: string): Observable<ApiResponse<Attendance[]>> {
    let params = new HttpParams().set('employeeId', employeeId.toString()).set('tenantId', tenantId.toString());
    if (startDate) params = params.set('startDate', startDate);
    if (endDate) params = params.set('endDate', endDate);
    return this.http.get<ApiResponse<Attendance[]>>(`${this.baseUrl}/self/attendance`, { params });
  }

  getSelfLeaveBalances(employeeId: number, tenantId: number): Observable<ApiResponse<LeaveBalance[]>> {
    const params = new HttpParams().set('employeeId', employeeId.toString()).set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<LeaveBalance[]>>(`${this.baseUrl}/self/leave-balances`, { params });
  }

  getSelfLeaveHistory(employeeId: number, tenantId: number): Observable<ApiResponse<LeaveRequest[]>> {
    const params = new HttpParams().set('employeeId', employeeId.toString()).set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<LeaveRequest[]>>(`${this.baseUrl}/self/leave-history`, { params });
  }

  submitSelfLeaveRequest(request: Partial<LeaveRequest>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/self/leave-request`, request);
  }

  submitSelfRegularization(request: Partial<AttendanceRegularization>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/self/regularization`, request);
  }

  // --- Onboarding ---
  getOnboardingWorkflows(tenantId: number, employeeId?: number, status?: string): Observable<ApiResponse<OnboardingWorkflowDto[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    if (status) params = params.set('status', status);
    return this.http.get<ApiResponse<OnboardingWorkflowDto[]>>(`${this.baseUrl}/onboarding/workflows`, { params });
  }

  createOnboardingWorkflow(workflow: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/onboarding/workflows`, workflow);
  }

  updateOnboardingWorkflow(id: number, workflow: any): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/onboarding/workflows/${id}`, workflow);
  }

  startOnboardingWorkflow(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/onboarding/workflows/${id}/start`, {}, { params });
  }

  completeOnboardingWorkflow(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/onboarding/workflows/${id}/complete`, {}, { params });
  }

  getOnboardingTasks(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/onboarding/tasks`, { params });
  }

  createOnboardingTask(task: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/onboarding/tasks`, task);
  }

  assignOnboardingTask(taskId: number, request: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/onboarding/tasks/${taskId}/assign`, request);
  }

  completeOnboardingTaskAssignment(assignmentId: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/onboarding/tasks/${assignmentId}/complete`, {}, { params });
  }

  getOnboardingStatusReport(tenantId: number): Observable<ApiResponse<OnboardingStatusReportDto[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<OnboardingStatusReportDto[]>>(`${this.baseUrl}/onboarding/reports/status`, { params });
  }

  getPendingOnboardingTasksReport(tenantId: number): Observable<ApiResponse<PendingTasksReportDto[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<PendingTasksReportDto[]>>(`${this.baseUrl}/onboarding/reports/pending-tasks`, { params });
  }

  // --- Offboarding ---
  submitExitRequest(request: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/offboarding/exit-requests`, request);
  }

  getExitStatusReport(tenantId: number): Observable<ApiResponse<ExitStatusReportDto[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<ExitStatusReportDto[]>>(`${this.baseUrl}/offboarding/reports/exit-status`, { params });
  }

  approveExitRequest(id: number, request: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/offboarding/exit-requests/${id}/approve`, request);
  }

  rejectExitRequest(id: number, request: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/offboarding/exit-requests/${id}/reject`, request);
  }

  initiateClearance(request: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/offboarding/clearances`, request);
  }

  getClearanceStatusReport(tenantId: number): Observable<ApiResponse<ClearanceStatusReportDto[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<ClearanceStatusReportDto[]>>(`${this.baseUrl}/offboarding/reports/clearance-status`, { params });
  }

  completeClearanceTask(taskId: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/offboarding/clearances/tasks/${taskId}/complete`, {}, { params });
  }

  calculateFullAndFinal(request: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/offboarding/settlements/calculate`, request);
  }

  getFullAndFinalSummaryReport(tenantId: number): Observable<ApiResponse<FullAndFinalSummaryReportDto[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<FullAndFinalSummaryReportDto[]>>(`${this.baseUrl}/offboarding/reports/settlement-summary`, { params });
  }

  // --- Asset Management (Phase 5) ---
  // Categories
  getCategories(tenantId: number, searchText?: string, page: number = 1, pageSize: number = 50): Observable<ApiResponse<AssetCategory[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString()).set('page', page.toString()).set('pageSize', pageSize.toString());
    if (searchText) params = params.set('searchText', searchText);
    return this.http.get<ApiResponse<AssetCategory[]>>(`${this.baseUrl}/assets/categories`, { params });
  }

  getCategory(id: number, tenantId: number): Observable<ApiResponse<AssetCategory>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<AssetCategory>>(`${this.baseUrl}/assets/categories/${id}`, { params });
  }

  createCategory(category: Partial<AssetCategory>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/assets/categories`, category);
  }

  updateCategory(id: number, category: Partial<AssetCategory>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/assets/categories/${id}`, category);
  }

  deleteCategory(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/assets/categories/${id}`, { params });
  }

  // Asset Master
  getAssets(tenantId: number, searchText?: string, status?: string, page: number = 1, pageSize: number = 50): Observable<ApiResponse<Asset[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString()).set('page', page.toString()).set('pageSize', pageSize.toString());
    if (searchText) params = params.set('searchText', searchText);
    if (status) params = params.set('status', status);
    return this.http.get<ApiResponse<Asset[]>>(`${this.baseUrl}/assets`, { params });
  }

  getAsset(id: number, tenantId: number): Observable<ApiResponse<Asset>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<Asset>>(`${this.baseUrl}/assets/${id}`, { params });
  }

  createAsset(asset: Partial<Asset>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/assets`, asset);
  }

  updateAsset(id: number, asset: Partial<Asset>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/assets/${id}`, asset);
  }

  deleteAsset(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/assets/${id}`, { params });
  }

  // Assignments
  getAssignments(tenantId: number, employeeId?: number, assetId?: number, status?: string, page: number = 1, pageSize: number = 50): Observable<ApiResponse<AssetAssignment[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString()).set('page', page.toString()).set('pageSize', pageSize.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    if (assetId) params = params.set('assetId', assetId.toString());
    if (status) params = params.set('status', status);
    return this.http.get<ApiResponse<AssetAssignment[]>>(`${this.baseUrl}/assets/assignments`, { params });
  }

  assignAsset(assignment: Partial<AssetAssignment>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/assets/assignments`, assignment);
  }

  updateAssignment(id: number, assignment: Partial<AssetAssignment>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/assets/assignments/${id}`, assignment);
  }

  returnAsset(id: number, returnRequest: { tenantId: number, returnedDate: string, returnCondition?: string }): Observable<ApiResponse<boolean>> {
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/assets/assignments/${id}/return`, returnRequest);
  }

  // Transfers
  getTransfers(tenantId: number): Observable<ApiResponse<AssetTransfer[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<AssetTransfer[]>>(`${this.baseUrl}/assets/transfers`, { params });
  }

  createAssetTransfer(transfer: Partial<AssetTransfer>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/assets/transfers`, transfer);
  }

  approveAssetTransfer(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/assets/transfers/${id}/approve`, {}, { params });
  }

  completeAssetTransfer(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/assets/transfers/${id}/complete`, {}, { params });
  }

  // Maintenance
  getMaintenance(tenantId: number): Observable<ApiResponse<AssetMaintenance[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<AssetMaintenance[]>>(`${this.baseUrl}/assets/maintenance`, { params });
  }

  createMaintenance(maint: Partial<AssetMaintenance>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/assets/maintenance`, maint);
  }

  updateMaintenance(id: number, maint: Partial<AssetMaintenance>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/assets/maintenance/${id}`, maint);
  }

  closeMaintenance(id: number, tenantId: number, remarks?: string): Observable<ApiResponse<boolean>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (remarks) params = params.set('remarks', remarks);
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/assets/maintenance/${id}/close`, {}, { params });
  }

  // Repairs
  getRepairs(tenantId: number): Observable<ApiResponse<AssetRepair[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<AssetRepair[]>>(`${this.baseUrl}/assets/repairs`, { params });
  }

  createRepair(repair: Partial<AssetRepair>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/assets/repairs`, repair);
  }

  updateRepair(id: number, repair: Partial<AssetRepair>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/assets/repairs/${id}`, repair);
  }

  closeRepair(id: number, tenantId: number, remarks?: string): Observable<ApiResponse<boolean>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (remarks) params = params.set('remarks', remarks);
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/assets/repairs/${id}/close`, {}, { params });
  }

  // Warranties
  getWarranties(tenantId: number): Observable<ApiResponse<AssetWarranty[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<AssetWarranty[]>>(`${this.baseUrl}/assets/warranties`, { params });
  }

  createWarranty(warranty: Partial<AssetWarranty>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/assets/warranties`, warranty);
  }

  updateWarranty(id: number, warranty: Partial<AssetWarranty>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/assets/warranties/${id}`, warranty);
  }

  getWarrantyExpiryReport(tenantId: number, withinDays: number = 30): Observable<ApiResponse<WarrantyExpiryReport[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString()).set('withinDays', withinDays.toString());
    return this.http.get<ApiResponse<WarrantyExpiryReport[]>>(`${this.baseUrl}/assets/warranties/expiry-report`, { params });
  }

  // Depreciation
  getDepreciations(tenantId: number): Observable<ApiResponse<AssetDepreciation[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<AssetDepreciation[]>>(`${this.baseUrl}/assets/depreciation`, { params });
  }

  recalculateDepreciation(tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/assets/depreciation/recalculate`, {}, { params });
  }

  getDepreciationReport(tenantId: number): Observable<ApiResponse<AssetDepreciationReport[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<AssetDepreciationReport[]>>(`${this.baseUrl}/assets/depreciation/report`, { params });
  }

  // Audits
  getAudits(tenantId: number): Observable<ApiResponse<AssetAudit[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<AssetAudit[]>>(`${this.baseUrl}/assets/audits`, { params });
  }

  createAudit(audit: Partial<AssetAudit>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/assets/audits`, audit);
  }

  completeAudit(id: number, tenantId: number, status: string, findings?: string): Observable<ApiResponse<boolean>> {
    let params = new HttpParams().set('tenantId', tenantId.toString()).set('status', status);
    if (findings) params = params.set('findings', findings);
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/assets/audits/${id}/complete`, {}, { params });
  }

  // Disposals
  getDisposals(tenantId: number): Observable<ApiResponse<AssetDisposal[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<AssetDisposal[]>>(`${this.baseUrl}/assets/disposals`, { params });
  }

  createDisposal(disposal: Partial<AssetDisposal>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/assets/disposals`, disposal);
  }

  approveDisposal(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/assets/disposals/${id}/approve`, {}, { params });
  }

  closeDisposal(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/assets/disposals/${id}/close`, {}, { params });
  }

  // Returns
  getReturns(tenantId: number): Observable<ApiResponse<AssetReturnWorkflow[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<AssetReturnWorkflow[]>>(`${this.baseUrl}/assets/returns`, { params });
  }

  createReturn(ret: Partial<AssetReturnWorkflow>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/assets/returns`, ret);
  }

  verifyReturn(id: number, tenantId: number, condition?: string): Observable<ApiResponse<boolean>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (condition) params = params.set('condition', condition);
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/assets/returns/${id}/verify`, {}, { params });
  }

  // Inventory
  getInventory(tenantId: number, locationId?: number, status?: string, page: number = 1, pageSize: number = 50): Observable<ApiResponse<AssetInventory[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString()).set('page', page.toString()).set('pageSize', pageSize.toString());
    if (locationId) params = params.set('locationId', locationId.toString());
    if (status) params = params.set('status', status);
    return this.http.get<ApiResponse<AssetInventory[]>>(`${this.baseUrl}/assets/inventory`, { params });
  }

  reconcileInventory(tenantId: number, assetId: number, locationId: number, quantity: number, status: string): Observable<ApiResponse<number>> {
    let params = new HttpParams()
      .set('tenantId', tenantId.toString())
      .set('assetId', assetId.toString())
      .set('locationId', locationId.toString())
      .set('quantity', quantity.toString())
      .set('status', status);
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/assets/inventory/reconcile`, {}, { params });
  }

  // ==========================================
  // --- Payroll & Compensation Management ---
  // ==========================================

  // Calendars
  getPayrollCalendars(tenantId: number, searchText?: string, page: number = 1, pageSize: number = 50): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString()).set('page', page.toString()).set('pageSize', pageSize.toString());
    if (searchText) params = params.set('searchText', searchText);
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/payroll/calendars`, { params });
  }

  createPayrollCalendar(cal: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/payroll/calendars`, cal);
  }

  updatePayrollCalendar(id: number, cal: any): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/payroll/calendars/${id}`, cal);
  }

  deletePayrollCalendar(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/payroll/calendars/${id}`, { params });
  }

  // Periods
  getPayrollPeriods(tenantId: number, calendarId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString()).set('calendarId', calendarId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/payroll/periods`, { params });
  }

  createPayrollPeriod(period: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/payroll/periods`, period);
  }

  openPayrollPeriod(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/payroll/periods/${id}/open`, {}, { params });
  }

  closePayrollPeriod(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/payroll/periods/${id}/close`, {}, { params });
  }

  lockPayrollPeriod(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/payroll/periods/${id}/lock`, {}, { params });
  }

  // Salary Structures
  getSalaryStructures(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/payroll/salary-structures`, { params });
  }

  createSalaryStructure(struct: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/payroll/salary-structures`, struct);
  }

  updateSalaryStructure(id: number, struct: any): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/payroll/salary-structures/${id}`, struct);
  }

  // Salary Components
  getSalaryComponents(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/payroll/salary-components`, { params });
  }

  createSalaryComponent(comp: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/payroll/salary-components`, comp);
  }

  // Employee Compensation
  getEmployeeCompensations(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/payroll/compensations`, { params });
  }

  createEmployeeCompensation(comp: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/payroll/compensations`, comp);
  }

  // Payroll Runs
  getPayrollRuns(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/payroll/runs`, { params });
  }

  createPayrollRun(run: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/payroll/runs`, run);
  }

  processPayrollRun(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/payroll/runs/${id}/process`, {}, { params });
  }

  getPayrollTransactions(runId: number, tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/payroll/runs/${runId}/transactions`, { params });
  }

  // Adjustments
  getPayrollAdjustments(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/payroll/adjustments`, { params });
  }

  createPayrollAdjustment(adj: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/payroll/adjustments`, adj);
  }

  approvePayrollAdjustment(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/payroll/adjustments/${id}/approve`, {}, { params });
  }

  // Loans
  getLoans(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/payroll/loans`, { params });
  }

  createLoan(loan: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/payroll/loans`, loan);
  }

  approveLoan(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/payroll/loans/${id}/approve`, {}, { params });
  }

  getLoanRepayments(loanId: number, tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/payroll/loans/${loanId}/repayments`, { params });
  }

  // Bonus & Incentives
  getBonuses(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/payroll/bonuses`, { params });
  }

  createBonus(bonus: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/payroll/bonuses`, bonus);
  }

  getIncentives(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/payroll/incentives`, { params });
  }

  createIncentive(inc: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/payroll/incentives`, inc);
  }

  // Payslips
  getPayslips(tenantId: number, employeeId?: number, periodId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    if (periodId) params = params.set('periodId', periodId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/payroll/payslips`, { params });
  }

  generatePayslip(tenantId: number, employeeId: number, periodId: number): Observable<ApiResponse<number>> {
    const params = new HttpParams().set('tenantId', tenantId.toString()).set('employeeId', employeeId.toString()).set('periodId', periodId.toString());
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/payroll/payslips/generate`, {}, { params });
  }

  // Tax Regimes & Slabs
  getTaxRegimes(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/tax/regimes`, { params });
  }

  createTaxRegime(regime: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/tax/regimes`, regime);
  }

  getTaxSlabs(regimeId: number, tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString()).set('regimeId', regimeId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/tax/slabs`, { params });
  }

  createTaxSlab(slab: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/tax/slabs`, slab);
  }

  // Declarations
  getTaxDeclarations(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/tax/declarations`, { params });
  }

  createTaxDeclaration(decl: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/tax/declarations`, decl);
  }

  // Computations & Statutory Deductions
  getTaxComputations(tenantId: number, financialYear: string): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString()).set('financialYear', financialYear);
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/tax/computations`, { params });
  }

  getStatutoryDeductions(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/tax/statutory-deductions`, { params });
  }

  // ==========================================
  // --- Performance Management System (PMS) ---
  // ==========================================

  getPerformanceCycles(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/performance/cycles`, { params });
  }

  createPerformanceCycle(cycle: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/performance/cycles`, cycle);
  }

  openPerformanceCycle(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/performance/cycles/${id}/open`, {}, { params });
  }

  getGoals(tenantId: number, employeeId?: number, cycleId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    if (cycleId) params = params.set('cycleId', cycleId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/performance/goals`, { params });
  }

  createGoal(goal: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/performance/goals`, goal);
  }

  logGoalProgress(goalId: number, progress: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/performance/goals/${goalId}/progress`, progress);
  }

  getGoalProgress(goalId: number, tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/performance/goals/${goalId}/progress`, { params });
  }

  getAppraisalTemplates(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/performance/templates`, { params });
  }

  getCompetencyFrameworks(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/performance/competencies`, { params });
  }

  getContinuousFeedback(tenantId: number, employeeId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/performance/feedback`, { params });
  }

  createContinuousFeedback(feedback: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/performance/feedback`, feedback);
  }

  getCheckIns(tenantId: number, employeeId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/performance/checkins`, { params });
  }

  createCheckIn(checkin: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/performance/checkins`, checkin);
  }

  getSelfAssessments(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/performance/self-assessments`, { params });
  }

  createSelfAssessment(sa: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/performance/self-assessments`, sa);
  }

  // ==========================================
  // --- Learning & Compliance Management ---
  // ==========================================

  getCourses(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/learning/courses`, { params });
  }

  createCourse(course: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/learning/courses`, course);
  }

  getCourseCategories(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/learning/categories`, { params });
  }

  createCourseCategory(cat: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/learning/categories`, cat);
  }

  getLearningEnrollments(tenantId: number, employeeId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/learning/enrollments`, { params });
  }

  createLearningEnrollment(enroll: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/learning/enrollments`, enroll);
  }

  getLearningAssignments(tenantId: number, employeeId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/learning/assignments`, { params });
  }

  createLearningAssignment(assignment: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/learning/assignments`, assignment);
  }

  getLearningAssessments(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/learning/assessments`, { params });
  }

  createLearningAssessment(assessment: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/learning/assessments`, assessment);
  }

  getAssessmentResults(tenantId: number, employeeId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/learning/assessment-results`, { params });
  }

  submitAssessmentResult(result: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/learning/assessment-results`, result);
  }

  getLearningCertifications(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/learning/certifications`, { params });
  }

  getCertificationRenewals(tenantId: number, employeeId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/learning/certification-renewals`, { params });
  }

  getComplianceTraining(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/learning/compliance-training`, { params });
  }

  recordComplianceAck(ack: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/learning/compliance-acknowledgements`, ack);
  }

  getComplianceAcks(tenantId: number, employeeId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/learning/compliance-acknowledgements`, { params });
  }

  getLearningSkills(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/learning/skills`, { params });
  }

  getEmployeeSkills(tenantId: number, employeeId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/learning/employee-skills`, { params });
  }

  getLearningAnalyticsSummary(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/learning/analytics/summary`, { params });
  }

  // ==========================================
  // --- Travel & Expense Management ---
  // ==========================================

  getTravelPolicies(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/travel/policies`, { params });
  }

  createTravelPolicy(policy: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/travel/policies`, policy);
  }

  getTravelRequests(tenantId: number, employeeId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/travel/requests`, { params });
  }

  getTravelRequestById(id: number, tenantId: number): Observable<ApiResponse<any>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any>>(`${this.baseUrl}/travel/requests/${id}`, { params });
  }

  createTravelRequest(request: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/travel/requests`, request);
  }

  approveTravelRequest(id: number, approval: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/travel/requests/${id}/approve`, approval);
  }

  cancelTravelRequest(id: number, tenantId: number): Observable<ApiResponse<any>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/travel/requests/${id}/cancel`, {}, { params });
  }

  getTravelAdvances(tenantId: number, employeeId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/travel/advances`, { params });
  }

  createTravelAdvance(advance: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/travel/advances`, advance);
  }

  approveTravelAdvance(id: number, tenantId: number): Observable<ApiResponse<any>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/travel/advances/${id}/approve`, {}, { params });
  }

  disburseTravelAdvance(id: number, tenantId: number): Observable<ApiResponse<any>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/travel/advances/${id}/disburse`, {}, { params });
  }

  getExpenseCategories(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/travel/expense-categories`, { params });
  }

  createExpenseCategory(cat: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/travel/expense-categories`, cat);
  }

  getExpenseClaims(tenantId: number, employeeId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/travel/expense-claims`, { params });
  }

  getExpenseClaimById(id: number, tenantId: number): Observable<ApiResponse<any>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any>>(`${this.baseUrl}/travel/expense-claims/${id}`, { params });
  }

  createExpenseClaim(claim: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/travel/expense-claims`, claim);
  }

  submitExpenseClaim(id: number, tenantId: number): Observable<ApiResponse<any>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/travel/expense-claims/${id}/submit`, {}, { params });
  }

  approveExpenseClaim(id: number, approval: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/travel/expense-claims/${id}/approve`, approval);
  }

  getExpenseSettlements(tenantId: number): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/travel/settlements`, { params });
  }

  processExpenseSettlement(settlement: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/travel/settlements/process`, settlement);
  }

  uploadReceipt(itemId: number, tenantId: number, fileName: string): Observable<ApiResponse<number>> {
    let params = new HttpParams()
      .set('itemId', itemId.toString())
      .set('tenantId', tenantId.toString())
      .set('fileName', fileName);
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/travel/receipts/upload`, {}, { params });
  }

  deleteReceipt(id: number, tenantId: number): Observable<ApiResponse<any>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<any>>(`${this.baseUrl}/travel/receipts/${id}`, { params });
  }

  importCorporateCardTransaction(tx: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/travel/corporate-cards/import`, tx);
  }

  reconcileCorporateCardTransaction(recon: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/travel/corporate-cards/reconcile`, recon);
  }

  getCorporateCardTransactions(tenantId: number, employeeId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/travel/corporate-cards/transactions`, { params });
  }

  getTravelCompliance(tenantId: number, travelRequestId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (travelRequestId) params = params.set('travelRequestId', travelRequestId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/travel/compliance`, { params });
  }

  getTravelAnalyticsSummary(tenantId: number, employeeId?: number): Observable<ApiResponse<any[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString());
    if (employeeId) params = params.set('employeeId', employeeId.toString());
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/travel/analytics/summary`, { params });
  }
}

