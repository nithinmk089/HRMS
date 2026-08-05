import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';
import { Asset, AssetCategory, WarrantyExpiryReport } from '../../../core/models/asset.models';

@Component({
  selector: 'app-asset-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './asset-dashboard.component.html',
  styleUrl: './asset-dashboard.component.scss'
})
export class AssetDashboardComponent implements OnInit {
  private api = inject(ApiService);
  
  totalAssets = 0;
  assignedAssets = 0;
  availableAssets = 0;
  maintenanceCount = 0;
  disposalCount = 0;
  expiringWarrantiesCount = 0;
  
  loading = false;
  selectedTenantId = 1;

  ngOnInit() {
    this.loadMetrics();
  }

  loadMetrics() {
    this.loading = true;
    this.api.getAssets(this.selectedTenantId).subscribe({
      next: (res: any) => {
        const assets = res.data || [];
        this.totalAssets = assets.length;
        this.assignedAssets = assets.filter((a: any) => a.status === 'Assigned').length;
        this.availableAssets = assets.filter((a: any) => a.status === 'Available').length;
        this.maintenanceCount = assets.filter((a: any) => a.status === 'Maintenance').length;
        this.disposalCount = assets.filter((a: any) => a.status === 'Disposed').length;
        this.loading = false;
      },
      error: () => this.loading = false
    });

    this.api.getWarrantyExpiryReport(this.selectedTenantId, 30).subscribe({
      next: (res: any) => {
        this.expiringWarrantiesCount = (res.data || []).length;
      }
    });
  }
}