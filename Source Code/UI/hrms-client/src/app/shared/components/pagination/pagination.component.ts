import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.scss'
})
export class PaginationComponent implements OnChanges {
  @Input() page: number = 1;
  @Input() pageSize: number = 10;
  @Input() totalItems: number = 0;
  @Input() pageSizes: number[] = [5, 10, 25, 50];

  @Output() pageChange = new EventEmitter<number>();
  @Output() pageSizeChange = new EventEmitter<number>();

  totalPages: number = 1;
  pages: number[] = [];
  protected readonly Math = Math;

  ngOnChanges() {
    this.calculatePages();
  }

  calculatePages() {
    this.totalPages = Math.max(1, Math.ceil(this.totalItems / this.pageSize));
    this.pages = [];
    for (let i = 1; i <= this.totalPages; i++) {
      this.pages.push(i);
    }
  }

  onPageSelect(p: number) {
    if (p < 1 || p > this.totalPages || p === this.page) return;
    this.page = p;
    this.pageChange.emit(p);
  }

  onPageSizeSelect(event: Event) {
    const size = Number((event.target as HTMLSelectElement).value);
    this.pageSize = size;
    this.page = 1;
    this.pageSizeChange.emit(size);
    this.pageChange.emit(1);
  }
}
