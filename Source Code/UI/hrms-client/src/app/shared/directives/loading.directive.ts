import { Directive, ElementRef, Input, OnChanges, Renderer2, SimpleChanges, inject } from '@angular/core';

@Directive({
  selector: '[appLoading]',
  standalone: true
})
export class LoadingDirective implements OnChanges {
  @Input('appLoading') isLoading = false;

  private el = inject(ElementRef);
  private renderer = inject(Renderer2);
  private spinnerEl?: HTMLElement;

  ngOnChanges(changes: SimpleChanges) {
    if (changes['isLoading']) {
      if (this.isLoading) {
        this.showSpinner();
      } else {
        this.hideSpinner();
      }
    }
  }

  private showSpinner() {
    this.renderer.setStyle(this.el.nativeElement, 'position', 'relative');
    this.renderer.setStyle(this.el.nativeElement, 'pointer-events', 'none');
    this.renderer.setStyle(this.el.nativeElement, 'opacity', '0.6');

    if (!this.spinnerEl) {
      this.spinnerEl = this.renderer.createElement('div');
      this.renderer.addClass(this.spinnerEl, 'spinner-border');
      this.renderer.addClass(this.spinnerEl, 'spinner-border-sm');
      this.renderer.addClass(this.spinnerEl, 'text-primary');
      this.renderer.setStyle(this.spinnerEl, 'position', 'absolute');
      this.spinnerEl!.style.top = 'calc(50% - 8px)';
      this.spinnerEl!.style.left = 'calc(50% - 8px)';
      this.renderer.appendChild(this.el.nativeElement, this.spinnerEl);
    }
  }

  private hideSpinner() {
    this.renderer.removeStyle(this.el.nativeElement, 'pointer-events');
    this.renderer.removeStyle(this.el.nativeElement, 'opacity');

    if (this.spinnerEl) {
      this.renderer.removeChild(this.el.nativeElement, this.spinnerEl);
      this.spinnerEl = undefined;
    }
  }
}
