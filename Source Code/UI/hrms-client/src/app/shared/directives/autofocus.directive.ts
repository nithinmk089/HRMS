import { Directive, ElementRef, OnInit, inject } from '@angular/core';

@Directive({
  selector: '[appAutoFocus]',
  standalone: true
})
export class AutoFocusDirective implements OnInit {
  private el = inject(ElementRef);

  ngOnInit() {
    setTimeout(() => {
      this.el.nativeElement.focus();
    }, 100);
  }
}
