import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';
import { AssetReturnWorkflow } from '../../../core/models/asset.models';

@Component({
  selector: 'app-asset-return',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './asset-return.component.html'
})
export class AssetReturnComponent implements OnInit {
  private api = inject(ApiService);

  returns: AssetReturnWorkflow[] = [];
  selectedTenantId = 1;
  loading = false;

  ngOnInit() {
    this.loadReturns();
  }

  loadReturns() {
    this.loading = true;
    this.api.getReturns(this.selectedTenantId).subscribe({
      next: (res: any) => {
        this.returns = res.data || [];
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  verifyReturn(ret: AssetReturnWorkflow) {
    const cond = prompt('Verify asset return condition:', ret.returnCondition || 'Good Condition');
    if (cond !== null) {
      this.api.verifyReturn(ret.assetReturnID!, this.selectedTenantId, cond).subscribe({
        next: () => this.loadReturns()
      });
    }
  }
}