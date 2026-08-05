import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';

@Component({
  selector: 'app-data-import',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './data-import.component.html',
  styleUrl: './data-import.component.scss'
})
export class DataImportComponent implements OnInit {
  private api = inject(ApiService);

  activeTab: 'employees' | 'master-data' | 'operational' = 'employees';

  // Master Data entity selector
  selectedMasterEntity = 'departments';
  masterEntities = [
    { code: 'companies', name: 'Operating Companies' },
    { code: 'business-units', name: 'Business Units' },
    { code: 'departments', name: 'Departments' },
    { code: 'designations', name: 'Designations' },
    { code: 'locations', name: 'Physical Locations' },
    { code: 'cost-centers', name: 'Cost Centers' },
    { code: 'leave-types', name: 'Leave Types' },
    { code: 'shifts', name: 'Work Shifts' },
    { code: 'salary-components', name: 'Salary Components' },
    { code: 'expense-categories', name: 'Travel & Expense Categories' },
    { code: 'asset-categories', name: 'Asset Categories' },
    { code: 'skills', name: 'Skills Catalog' }
  ];

  // Operational items entity selector
  selectedOpEntity = 'attendance';
  opEntities = [
    { code: 'attendance', name: 'Attendance Punch Logs' },
    { code: 'leave-balances', name: 'Employee Leave Balances' }
  ];

  // File upload state
  selectedFile: File | null = null;
  isDragging = false;
  isUploading = false;
  uploadProgress = 0;

  // Import Result report
  importResult: any = null;
  errorMessage = '';

  ngOnInit() {}

  setTab(tab: 'employees' | 'master-data' | 'operational') {
    this.activeTab = tab;
    this.selectedFile = null;
    this.importResult = null;
    this.errorMessage = '';
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      this.importResult = null;
      this.errorMessage = '';
    }
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    this.isDragging = true;
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    this.isDragging = false;
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    this.isDragging = false;
    if (event.dataTransfer && event.dataTransfer.files.length > 0) {
      this.selectedFile = event.dataTransfer.files[0];
      this.importResult = null;
      this.errorMessage = '';
    }
  }

  downloadTemplate(entityType: string) {
    this.api.downloadImportTemplate(entityType).subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `${entityType}_import_template.csv`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: () => {
        this.errorMessage = 'Failed to download import template.';
      }
    });
  }

  uploadFile() {
    if (!this.selectedFile) return;

    this.isUploading = true;
    this.uploadProgress = 30;
    this.errorMessage = '';
    this.importResult = null;

    let uploadObs;
    if (this.activeTab === 'employees') {
      uploadObs = this.api.uploadEmployeeData(this.selectedFile);
    } else if (this.activeTab === 'master-data') {
      uploadObs = this.api.uploadMasterData(this.selectedMasterEntity, this.selectedFile);
    } else {
      uploadObs = this.api.uploadOperationalData(this.selectedOpEntity, this.selectedFile);
    }

    uploadObs.subscribe({
      next: (res: any) => {
        this.isUploading = false;
        this.uploadProgress = 100;
        if (res.success && res.data) {
          this.importResult = res.data;
        } else {
          this.errorMessage = res.message || 'Data import failed.';
        }
      },
      error: (err) => {
        this.isUploading = false;
        this.uploadProgress = 0;
        this.errorMessage = err.error?.message || 'Server error processing file import.';
      }
    });
  }
}
