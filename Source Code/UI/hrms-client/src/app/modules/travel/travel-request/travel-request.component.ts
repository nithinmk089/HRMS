import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-travel-request',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="travel-request-container">
      <div class="header-section">
        <div>
          <h1>Travel Requests</h1>
          <p>Submit and monitor employee travel itineraries, schedules, and approval status.</p>
        </div>
        <button class="btn btn-primary" (click)="openCreateModal()">
          <i class="bi bi-plus-circle me-2"></i>New Travel Request
        </button>
      </div>

      <!-- Toast Notifications -->
      <div *ngIf="successMessage" class="alert alert-success mt-3">{{ successMessage }}</div>
      <div *ngIf="errorMessage" class="alert alert-danger mt-3">{{ errorMessage }}</div>

      <!-- Requests Table -->
      <div class="card grid-card mt-4">
        <div class="table-responsive">
          <table class="table custom-table">
            <thead>
              <tr>
                <th>Request ID</th>
                <th>Employee Name</th>
                <th>Travel Type</th>
                <th>Destination</th>
                <th>Start Date</th>
                <th>End Date</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let req of requests">
                <td>#{{ req.travelRequestID }}</td>
                <td>{{ req.employeeName }}</td>
                <td>{{ req.travelType }}</td>
                <td>{{ req.destination }}</td>
                <td>{{ req.travelStartDate | date:'mediumDate' }}</td>
                <td>{{ req.travelEndDate | date:'mediumDate' }}</td>
                <td>
                  <span class="badge" [ngClass]="getStatusClass(req.requestStatus)">
                    {{ req.requestStatus }}
                  </span>
                </td>
                <td>
                  <button *ngIf="req.requestStatus === 'Pending'" class="btn btn-sm btn-outline-danger" (click)="cancelRequest(req.travelRequestID)">
                    Cancel
                  </button>
                </td>
              </tr>
              <tr *ngIf="requests.length === 0">
                <td colspan="8" class="text-center text-muted">No travel requests found.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Modal -->
      <div class="modal-backdrop" *ngIf="showModal" (click)="closeModal()"></div>
      <div class="custom-modal" *ngIf="showModal">
        <div class="modal-header">
          <h2>Create Travel Request</h2>
          <button class="close-btn" (click)="closeModal()">&times;</button>
        </div>
        <form [formGroup]="form" (ngSubmit)="onSubmit()">
          <div class="modal-body">
            <div class="form-group mb-3">
              <label for="employeeId">Employee ID</label>
              <input id="employeeId" type="number" formControlName="employeeId" class="form-control" placeholder="Enter Employee ID">
            </div>

            <div class="form-group mb-3">
              <label for="travelType">Travel Type</label>
              <select id="travelType" formControlName="travelType" class="form-select form-control">
                <option value="Domestic">Domestic</option>
                <option value="International">International</option>
              </select>
            </div>

            <div class="form-group mb-3">
              <label for="destination">Destination</label>
              <input id="destination" type="text" formControlName="destination" class="form-control" placeholder="City, Country">
            </div>

            <div class="row">
              <div class="col-md-6 form-group mb-3">
                <label for="travelStartDate">Start Date</label>
                <input id="travelStartDate" type="date" formControlName="travelStartDate" class="form-control">
              </div>
              <div class="col-md-6 form-group mb-3">
                <label for="travelEndDate">End Date</label>
                <input id="travelEndDate" type="date" formControlName="travelEndDate" class="form-control">
              </div>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" (click)="closeModal()">Cancel</button>
            <button type="submit" class="btn btn-primary" [disabled]="form.invalid">Submit Request</button>
          </div>
        </form>
      </div>
    </div>
  `,
  styles: [`
    .travel-request-container {
      padding: 24px;
      font-family: 'Inter', sans-serif;
      color: #fff;
      background-color: #0b0c10;
      min-height: 100vh;
    }
    .header-section {
      display: flex;
      justify-content: space-between;
      align-items: center;
      background: linear-gradient(135deg, #1f2833 0%, #0b0c10 100%);
      padding: 24px 32px;
      border-radius: 16px;
      border: 1px solid #45f3ff22;
    }
    .header-section h1 {
      font-size: 2rem;
      margin: 0 0 6px 0;
      background: linear-gradient(to right, #45f3ff, #1f85de);
      -webkit-background-clip: text;
      -webkit-text-fill-color: transparent;
    }
    .header-section p {
      color: #c5c6c7;
      margin: 0;
    }
    .grid-card {
      background: #1f2833;
      border: 1px solid rgba(255, 255, 255, 0.05);
      border-radius: 16px;
      padding: 24px;
    }
    .custom-table {
      color: #c5c6c7;
      margin: 0;
    }
    .custom-table th {
      border-bottom: 2px solid rgba(255, 255, 255, 0.1);
      color: #fff;
    }
    .custom-table td {
      border-bottom: 1px solid rgba(255, 255, 255, 0.05);
      padding: 14px 8px;
    }
    .badge {
      padding: 6px 12px;
      border-radius: 20px;
    }
    .badge-approved { background-color: rgba(46,204,113,0.2); color: #2ecc71; border: 1px solid #2ecc71; }
    .badge-pending { background-color: rgba(241,196,15,0.2); color: #f1c40f; border: 1px solid #f1c40f; }
    .badge-rejected { background-color: rgba(231,76,60,0.2); color: #e74c3c; border: 1px solid #e74c3c; }

    /* Modal Styles */
    .modal-backdrop {
      position: fixed;
      top: 0; left: 0; right: 0; bottom: 0;
      background: rgba(0,0,0,0.75);
      z-index: 1000;
    }
    .custom-modal {
      position: fixed;
      top: 50%; left: 50%;
      transform: translate(-50%, -50%);
      background: #1f2833;
      border: 1px solid #45f3ff44;
      border-radius: 16px;
      width: 500px;
      max-width: 90%;
      z-index: 1001;
      color: #fff;
    }
    .modal-header {
      padding: 20px;
      border-bottom: 1px solid rgba(255,255,255,0.1);
      display: flex;
      justify-content: space-between;
      align-items: center;
    }
    .modal-header h2 { margin: 0; font-size: 1.3rem; color: #45f3ff; }
    .close-btn { background: none; border: none; color: #fff; font-size: 1.5rem; cursor: pointer; }
    .modal-body { padding: 20px; }
    .modal-footer {
      padding: 20px;
      border-top: 1px solid rgba(255,255,255,0.1);
      display: flex;
      justify-content: flex-end;
      gap: 12px;
    }
    .form-control {
      background: #0b0c10;
      border: 1px solid rgba(255,255,255,0.1);
      color: #fff;
    }
    .form-control:focus {
      background: #0b0c10;
      color: #fff;
      border-color: #45f3ff;
      box-shadow: 0 0 8px rgba(69,243,255,0.25);
    }
    .btn-primary {
      background: #1f85de;
      border: none;
    }
    .btn-outline-danger {
      color: #e74c3c;
      border-color: #e74c3c;
      background: transparent;
    }
    .btn-outline-danger:hover {
      background: #e74c3c;
      color: #fff;
    }
  `]
})
export class TravelRequestComponent implements OnInit {
  requests: any[] = [];
  form!: FormGroup;
  showModal = false;
  successMessage = '';
  errorMessage = '';

  constructor(private api: ApiService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initForm();
    this.loadRequests();
  }

  initForm(): void {
    this.form = this.fb.group({
      employeeId: ['', [Validators.required, Validators.min(1)]],
      travelType: ['Domestic', Validators.required],
      destination: ['', Validators.required],
      travelStartDate: ['', Validators.required],
      travelEndDate: ['', Validators.required]
    });
  }

  loadRequests(): void {
    this.api.getTravelRequests(1).subscribe(res => {
      this.requests = res.data || [];
    });
  }

  openCreateModal(): void {
    this.form.reset({ travelType: 'Domestic' });
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    const payload = {
      tenantId: 1,
      ...this.form.value
    };

    this.api.createTravelRequest(payload).subscribe({
      next: () => {
        this.successMessage = 'Travel request submitted successfully!';
        this.loadRequests();
        this.closeModal();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: () => {
        this.errorMessage = 'Failed to submit travel request.';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  cancelRequest(id: number): void {
    this.api.cancelTravelRequest(id, 1).subscribe({
      next: () => {
        this.successMessage = 'Travel request cancelled.';
        this.loadRequests();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: () => {
        this.errorMessage = 'Failed to cancel travel request.';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  getStatusClass(status: string): string {
    const s = (status || '').toLowerCase();
    if (s === 'approved') return 'badge-approved';
    if (s === 'pending') return 'badge-pending';
    return 'badge-rejected';
  }
}
