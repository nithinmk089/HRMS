export interface Employee {
  employeeID: number;
  tenantID: number;
  employeeCode: string;
  employeeNumber: string;
  firstName: string;
  middleName?: string;
  lastName: string;
  preferredName?: string;
  gender?: string;
  dateOfBirth?: string;
  maritalStatus?: string;
  nationality?: string;
  personalEmail?: string;
  mobileNumber?: string;
  status: string;
}

export interface EmployeeDto {
  employeeID: number;
  tenantID: number;
  employeeCode: string;
  employeeNumber: string;
  firstName: string;
  middleName?: string;
  lastName: string;
  preferredName?: string;
  gender?: string;
  dateOfBirth?: string;
  maritalStatus?: string;
  nationality?: string;
  personalEmail?: string;
  mobileNumber?: string;
  employeeStatus: string;
  employmentType?: string;
  joiningDate?: string;
  confirmationDate?: string;
  probationEndDate?: string;
  noticePeriodDays?: number;
  employmentStatus?: string;
  companyID?: number;
  companyName?: string;
  businessUnitID?: number;
  businessUnitName?: string;
  departmentID?: number;
  departmentName?: string;
  designationID?: number;
  designationName?: string;
  grade?: string;
  locationID?: number;
  locationName?: string;
  costCenterID?: number;
  costCenterName?: string;
  managerID?: number;
  managerFullName?: string;
}

export interface EmployeeDirectoryDto {
  employeeID: number;
  tenantID: number;
  employeeCode: string;
  employeeNumber: string;
  firstName: string;
  middleName?: string;
  lastName: string;
  personalEmail?: string;
  mobileNumber?: string;
  status: string;
  departmentName?: string;
  designationName?: string;
  locationName?: string;
  joiningDate?: string;
  employmentType?: string;
}

export interface EmployeeEmployment {
  employeeEmploymentID: number;
  tenantID: number;
  employeeID: number;
  companyID: number;
  companyName?: string;
  businessUnitID: number;
  businessUnitName?: string;
  departmentID: number;
  departmentName?: string;
  designationID: number;
  designationName?: string;
  locationID: number;
  locationName?: string;
  costCenterID: number;
  costCenterName?: string;
  employmentType: string;
  joiningDate: string;
  confirmationDate?: string;
  probationEndDate?: string;
  noticePeriodDays: number;
  employmentStatus: string;
}

export interface EmployeeAddress {
  employeeAddressID: number;
  tenantID: number;
  employeeID: number;
  addressType: string;
  addressLine1: string;
  addressLine2?: string;
  city: string;
  state?: string;
  country: string;
  zipCode?: string;
}

export interface EmployeeContact {
  employeeContactID: number;
  tenantID: number;
  employeeID: number;
  contactType: string;
  contactValue: string;
}

export interface EmployeeEmergencyContact {
  employeeEmergencyContactID: number;
  tenantID: number;
  employeeID: number;
  contactName: string;
  relationship: string;
  mobileNumber: string;
  email?: string;
}

export interface EmployeeQualification {
  employeeQualificationID: number;
  tenantID: number;
  employeeID: number;
  qualificationType: string;
  institution: string;
  university?: string;
  yearOfPassing: number;
  percentage?: number;
}

export interface EmployeeCertification {
  employeeCertificationID: number;
  tenantID: number;
  employeeID: number;
  certificationName: string;
  certificationAuthority: string;
  issueDate: string;
  expiryDate?: string;
  certificateNumber?: string;
}

export interface EmployeeDocument {
  employeeDocumentID: number;
  tenantID: number;
  employeeID: number;
  employeeFullName?: string;
  employeeCode?: string;
  documentType: string;
  fileName: string;
  filePath: string;
  mimeType: string;
  versionNumber: number;
  createdDate?: string;
  createdBy?: number;
}

export interface EmployeeManager {
  employeeManagerID: number;
  tenantID: number;
  employeeID: number;
  employeeFullName?: string;
  employeeCode?: string;
  managerID: number;
  managerFullName?: string;
  managerCode?: string;
  effectiveFrom: string;
  effectiveTo?: string;
}

export interface EmployeeTransfer {
  employeeTransferID: number;
  tenantID: number;
  employeeID: number;
  employeeFullName?: string;
  employeeCode?: string;
  fromDepartmentID: number;
  fromDepartmentName?: string;
  toDepartmentID: number;
  toDepartmentName?: string;
  fromLocationID: number;
  fromLocationName?: string;
  toLocationID: number;
  toLocationName?: string;
  effectiveDate: string;
  reason?: string;
  status: string;
  createdDate?: string;
}

export interface EmployeePromotion {
  employeePromotionID: number;
  tenantID: number;
  employeeID: number;
  employeeFullName?: string;
  employeeCode?: string;
  oldDesignationID: number;
  oldDesignationName?: string;
  newDesignationID: number;
  newDesignationName?: string;
  oldGrade?: string;
  newGrade?: string;
  effectiveDate: string;
  reason?: string;
  status: string;
  createdDate?: string;
}

export interface EmployeeStatusHistory {
  employeeStatusHistoryID: number;
  tenantID: number;
  employeeID: number;
  status: string;
  effectiveDate: string;
  reason?: string;
}

export interface EmployeeServiceHistoryReport {
  recordID: number;
  recordType: string;
  detailText: string;
  effectiveDate: string;
  reason?: string;
  createdDate: string;
}

export interface CertificationExpiryReport {
  documentID: number;
  tenantID: number;
  employeeID: number;
  employeeFullName: string;
  employeeCode: string;
  documentCategory: string;
  documentName: string;
  documentNumber?: string;
  expiryDate: string;
  daysToExpiry: number;
}
