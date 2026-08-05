import { Component } from '@angular/core';
import { TestBed, ComponentFixture } from '@angular/core/testing';
import { By } from '@angular/platform-browser';

import { PermissionDirective } from './permission.directive';

@Component({
  template: `
    <div *appPermission="'COMPANY_MANAGE'">Company Section</div>
    <div *appPermission="'SYSADMIN'">SysAdmin Section</div>
  `,
  imports: [PermissionDirective]
})
class TestComponent {}

describe('PermissionDirective', () => {
  let fixture: ComponentFixture<TestComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [TestComponent, PermissionDirective]
    });
    localStorage.clear();
  });

  it('should render element when user has permission', () => {
    localStorage.setItem('permissions', JSON.stringify(['COMPANY_MANAGE']));
    fixture = TestBed.createComponent(TestComponent);
    fixture.detectChanges();

    const elements = fixture.debugElement.queryAll(By.css('div'));
    expect(elements.length).toBe(1);
    expect(elements[0].nativeElement.textContent).toBe('Company Section');
  });

  it('should not render element when user lacks permission', () => {
    localStorage.setItem('permissions', JSON.stringify(['USER_MANAGE']));
    fixture = TestBed.createComponent(TestComponent);
    fixture.detectChanges();

    const elements = fixture.debugElement.queryAll(By.css('div'));
    expect(elements.length).toBe(0);
  });
});
