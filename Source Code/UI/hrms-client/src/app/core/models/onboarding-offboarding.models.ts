export interface OnboardingWorkflowDto {
  onboardingWorkflowID: number;
  tenantID: number;
  employeeID: number;
  workflowCode: string;
  workflowName: string;
  startDate: string;
  targetCompletionDate: string;
  completionDate?: string;
  workflowStatus: string;
}

export interface DocumentSubmissionDto {
  employeeDocumentSubmissionID: number;
  tenantID: number;
  employeeID: number;
  documentType: string;
  fileName: string;
  filePath: string;
  mimeType: string;
  uploadedDate: string;
}

export interface OnboardingStatusReportDto {
  onboardingWorkflowID: number;
  tenantID: number;
  employeeID: number;
  employeeCode: string;
  employeeName: string;
  workflowCode: string;
  workflowName: string;
  startDate: string;
  targetCompletionDate: string;
  completionDate?: string;
  workflowStatus: string;
  completionPercentage: number;
}

export interface PendingTasksReportDto {
  onboardingTaskAssignmentID: number;
  tenantID: number;
  employeeID: number;
  employeeName: string;
  onboardingTaskID: number;
  taskCode: string;
  taskName: string;
  assignedDate: string;
  dueDate: string;
  taskStatus: string;
}

export interface ExitStatusReportDto {
  exitRequestID: number;
  tenantID: number;
  employeeID: number;
  employeeCode: string;
  employeeName: string;
  resignationDate: string;
  lastWorkingDate: string;
  exitReason: string;
  status: string;
  clearanceStatus: string;
  settlementStatus: string;
}

export interface ClearanceStatusReportDto {
  clearanceRequestID: number;
  tenantID: number;
  employeeID: number;
  employeeName: string;
  clearanceStatus: string;
  initiatedDate: string;
  completedDate?: string;
  totalTasks: number;
  approvedTasks: number;
}

export interface FullAndFinalSummaryReportDto {
  fullAndFinalSettlementID: number;
  tenantID: number;
  employeeID: number;
  employeeCode: string;
  employeeName: string;
  settlementAmount: number;
  settlementDate?: string;
  settlementStatus: string;
}
