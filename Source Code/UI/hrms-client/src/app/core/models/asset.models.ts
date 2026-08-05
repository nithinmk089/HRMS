export interface AssetCategory {
  assetCategoryID?: number;
  tenantID: number;
  categoryCode: string;
  categoryName: string;
  parentCategoryID?: number;
  description?: string;
  isDepreciable: boolean;
}

export interface Asset {
  assetID?: number;
  tenantID: number;
  assetCode: string;
  assetTag: string;
  assetName: string;
  assetCategoryID: number;
  categoryName?: string;
  manufacturer?: string;
  model?: string;
  serialNumber: string;
  purchaseDate: string;
  purchaseCost: number;
  currentBookValue: number;
  status: string;
  currentHolder?: string;
  warrantyStatus?: string;
}

export interface AssetAssignment {
  assetAssignmentID?: number;
  tenantID: number;
  assetID: number;
  assetCode?: string;
  assetName?: string;
  employeeID: number;
  employeeCode?: string;
  employeeName?: string;
  assignedDate: string;
  expectedReturnDate?: string;
  returnedDate?: string;
  assignmentStatus: string;
}

export interface AssetTransfer {
  assetTransferID?: number;
  tenantID: number;
  assetID: number;
  assetCode?: string;
  assetName?: string;
  fromEmployeeID?: number;
  fromEmployeeName?: string;
  toEmployeeID: number;
  toEmployeeName?: string;
  transferDate: string;
  transferReason?: string;
  transferStatus: string;
}

export interface AssetMaintenance {
  assetMaintenanceID?: number;
  tenantID: number;
  assetID: number;
  assetCode?: string;
  assetName?: string;
  maintenanceDate: string;
  maintenanceType: string;
  vendorName?: string;
  cost: number;
  maintenanceStatus: string;
}

export interface AssetRepair {
  assetRepairID?: number;
  tenantID: number;
  assetID: number;
  repairDate: string;
  repairReason?: string;
  repairCost: number;
  repairStatus: string;
}

export interface AssetWarranty {
  assetWarrantyID?: number;
  tenantID: number;
  assetID: number;
  assetCode?: string;
  assetName?: string;
  warrantyStartDate: string;
  warrantyEndDate: string;
  warrantyProvider: string;
  warrantyStatus?: string;
}

export interface WarrantyExpiryReport {
  assetWarrantyID: number;
  assetID: number;
  assetCode: string;
  assetName: string;
  warrantyEndDate: string;
  warrantyProvider: string;
}

export interface AssetDepreciation {
  assetDepreciationID?: number;
  tenantID: number;
  assetID: number;
  assetCode?: string;
  assetName?: string;
  depreciationMethod: string;
  depreciationRate: number;
  bookValue: number;
}

export interface AssetDepreciationReport {
  assetDepreciationID: number;
  assetID: number;
  assetCode: string;
  assetName: string;
  depreciationMethod: string;
  depreciationRate: number;
  purchaseCost: number;
  bookValue: number;
}

export interface AssetAudit {
  assetAuditID?: number;
  tenantID: number;
  assetID: number;
  assetCode?: string;
  assetName?: string;
  auditDate: string;
  auditorID: number;
  auditStatus: string;
  findings?: string;
}

export interface AssetAuditReport {
  assetAuditID: number;
  assetID: number;
  assetCode: string;
  assetName: string;
  auditDate: string;
  auditor: string;
  auditStatus: string;
  findings?: string;
}

export interface AssetDisposal {
  assetDisposalID?: number;
  tenantID: number;
  assetID: number;
  assetCode?: string;
  assetName?: string;
  disposalDate: string;
  disposalMethod: string;
  disposalValue: number;
  disposalStatus: string;
}

export interface AssetReturnWorkflow {
  assetReturnID?: number;
  tenantID: number;
  assetAssignmentID: number;
  returnDate: string;
  returnCondition?: string;
  returnStatus: string;
}

export interface AssetInventory {
  assetInventoryID?: number;
  tenantID: number;
  assetID: number;
  assetCode?: string;
  assetName?: string;
  locationID: number;
  locationName?: string;
  quantity: number;
  inventoryStatus: string;
}
