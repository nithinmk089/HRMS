import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface GridColumn {
  key: string;
  label: string;
  sortable?: boolean;
  type?: 'text' | 'date' | 'status' | 'currency' | 'action';
}

@Component({
  selector: 'app-data-grid',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './data-grid.component.html',
  styleUrl: './data-grid.component.scss'
})
export class DataGridComponent {
  @Input() data: any[] = [];
  @Input() columns: GridColumn[] = [];
  @Input() isLoading: boolean = false;
  
  @Output() sortChange = new EventEmitter<{ key: string, order: 'asc' | 'desc' }>();
  @Output() rowAction = new EventEmitter<{ action: string, row: any }>();

  sortKey: string = '';
  sortOrder: 'asc' | 'desc' = 'asc';

  onSort(column: GridColumn) {
    if (!column.sortable) return;
    
    if (this.sortKey === column.key) {
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortKey = column.key;
      this.sortOrder = 'asc';
    }
    
    this.sortChange.emit({ key: this.sortKey, order: this.sortOrder });
  }

  onAction(action: string, row: any) {
    this.rowAction.emit({ action, row });
  }
}
