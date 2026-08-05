import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-filter-panel',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './filter-panel.component.html',
  styleUrl: './filter-panel.component.scss'
})
export class FilterPanelComponent {
  @Input() title: string = 'Filters';
  @Input() isOpen: boolean = false;
  
  @Output() toggle = new EventEmitter<boolean>();
  @Output() reset = new EventEmitter<void>();

  onToggle() {
    this.isOpen = !this.isOpen;
    this.toggle.emit(this.isOpen);
  }

  onReset() {
    this.reset.emit();
  }
}
