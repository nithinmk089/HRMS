import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DebounceDirective } from '../../directives/debounce.directive';

@Component({
  selector: 'app-search-panel',
  standalone: true,
  imports: [CommonModule, DebounceDirective],
  templateUrl: './search-panel.component.html',
  styleUrl: './search-panel.component.scss'
})
export class SearchPanelComponent {
  @Input() placeholder: string = 'Search...';
  @Input() initialValue: string = '';
  @Input() delay: number = 300;
  
  @Output() search = new EventEmitter<string>();

  onSearch(value: string) {
    this.search.emit(value);
  }
}
