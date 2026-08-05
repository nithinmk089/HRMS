import { Directive, EventEmitter, Input, OnDestroy, OnInit, Output, HostListener } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Directive({
  selector: '[appDebounce]',
  standalone: true
})
export class DebounceDirective implements OnInit, OnDestroy {
  @Input() delay = 300;
  @Output() debouncedSearch = new EventEmitter<string>();

  private clicks = new Subject<string>();
  private subscription!: Subscription;

  ngOnInit() {
    this.subscription = this.clicks.pipe(
      debounceTime(this.delay)
    ).subscribe(e => this.debouncedSearch.emit(e));
  }

  @HostListener('input', ['$event.target.value'])
  onInput(value: string) {
    this.clicks.next(value);
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
