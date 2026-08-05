import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';
import { AssetDepreciationReport } from '../../../core/models/asset.models';

@Component({
  selector: 'app-asset-depreciation',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './asset-depreciation.component.html'
})
export class AssetDepreciationComponent implements OnInit {
  private api = inject(ApiService);

  depreciations: AssetDepreciationReport[] = [];
  selectedTenantId = 1;
  loading = false;
  recalculating = false;

  ngOnInit() {
    this.loadReport();
  }

  loadReport() {
    this.loading = true;
    this.api.getDepreciationReport(this.selectedTenantId).subscribe({
      next: (res: any) => {
        this.depreciations = res.data || [];
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  recalculate() {
    this.recalculating = true;
    this.api.recalculateDepreciation(this.selectedTenantId).subscribe({
      next: () => {
        this.recalculating = false;
        this.loadReport();
      },
      error: () => this.recalculating = false
    });
  }
}