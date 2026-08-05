import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';
import { AssetInventory } from '../../../core/models/asset.models';

@Component({
  selector: 'app-asset-inventory',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './asset-inventory.component.html'
})
export class AssetInventoryComponent implements OnInit {
  private api = inject(ApiService);

  inventory: AssetInventory[] = [];
  selectedTenantId = 1;
  loading = false;

  ngOnInit() {
    this.loadInventory();
  }

  loadInventory() {
    this.loading = true;
    this.api.getInventory(this.selectedTenantId).subscribe({
      next: (res: any) => {
        this.inventory = res.data || [];
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }
}