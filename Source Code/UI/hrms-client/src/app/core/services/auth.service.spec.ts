import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import { AuthService } from './auth.service';
import { environment } from '../../../environments/environment';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        AuthService,
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: Router, useValue: spy }
      ]
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
    routerSpy = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    localStorage.clear();
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should store token and user details on successful login', () => {
    const dummyResponse = {
      success: true,
      data: {
        accessToken: 'dummy-token',
        refreshToken: 'dummy-refresh',
        user: { userId: 1, email: 'admin@hrms.com', firstName: 'Admin', lastName: 'User', tenantId: 1 },
        roles: ['ADMIN'],
        permissions: ['USER_MANAGE']
      }
    };

    service.login('admin@hrms.com', 'password').subscribe(res => {
      expect(res.success).toBeTrue();
      expect(localStorage.getItem('accessToken')).toBe('dummy-token');
      expect(localStorage.getItem('tenantId')).toBe('1');
      expect(localStorage.getItem('roles')).toContain('ADMIN');
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/auth/login`);
    expect(req.request.method).toBe('POST');
    req.flush(dummyResponse);
  });

  it('should clear local storage on logout', () => {
    localStorage.setItem('accessToken', 'some-token');
    service.logout();
    expect(localStorage.getItem('accessToken')).toBeNull();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/login']);
  });
});
