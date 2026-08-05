export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export interface Tenant {
  tenantId: number;
  tenantCode: string;
  tenantName: string;
  status: string;
  effectiveFrom: string;
  effectiveTo?: string;
  versionNo: number;
}

export interface Company {
  companyId: number;
  tenantId: number;
  companyCode: string;
  companyName: string;
  legalName?: string;
  taxNumber?: string;
  email?: string;
  phone?: string;
  website?: string;
  versionNo: number;
}

export interface BusinessUnit {
  businessUnitId: number;
  tenantId: number;
  companyId: number;
  businessUnitCode: string;
  businessUnitName: string;
  versionNo: number;
}

export interface Department {
  departmentId: number;
  tenantId: number;
  businessUnitId: number;
  departmentCode: string;
  departmentName: string;
  parentDepartmentId?: number;
  versionNo: number;
}

export interface Designation {
  designationId: number;
  tenantId: number;
  designationCode: string;
  designationName: string;
  grade?: string;
  versionNo: number;
}

export interface Location {
  locationId: number;
  tenantId: number;
  locationCode: string;
  locationName: string;
  countryCode?: string;
  stateCode?: string;
  city?: string;
  versionNo: number;
}

export interface CostCenter {
  costCenterId: number;
  tenantId: number;
  costCenterCode: string;
  costCenterName: string;
  versionNo: number;
}

export interface User {
  userId: number;
  tenantId: number;
  employeeId?: number;
  employeeCode?: string;
  userName: string;
  email: string;
  isLocked: boolean;
  lastLoginDate?: string;
  versionNo: number;
}

export interface Role {
  roleId: number;
  tenantId: number;
  roleCode: string;
  roleName: string;
  description?: string;
  versionNo: number;
}

export interface Permission {
  permissionId: number;
  tenantId: number;
  permissionCode: string;
  permissionName: string;
  moduleCode: string;
  versionNo: number;
}

export interface SystemConfiguration {
  configurationId: number;
  tenantId: number;
  configurationKey: string;
  configurationValue?: string;
  dataType: string;
  versionNo: number;
}
