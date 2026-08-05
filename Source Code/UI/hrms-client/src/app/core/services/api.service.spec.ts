import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';

import { ApiService } from './api.service';
import { environment } from '../../../environments/environment';

describe('ApiService', () => {
  let service: ApiService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ApiService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(ApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch tenants via GET', () => {
    const dummyResponse = {
      success: true,
      data: [{ tenantId: 1, tenantName: 'Tenant 1', status: 'Active' }]
    };

    service.getTenants('search', 'Active', 1, 10).subscribe(res => {
      expect(res.success).toBeTrue();
      expect(res.data?.length).toBe(1);
      expect(res.data?.[0].tenantName).toBe('Tenant 1');
    });

    const req = httpMock.expectOne(request => 
      request.url.includes('/tenants') && 
      request.params.get('searchText') === 'search' && 
      request.params.get('status') === 'Active'
    );
    expect(req.request.method).toBe('GET');
    req.flush(dummyResponse);
  });

  it('should fetch companies via GET', () => {
    const dummyResponse = {
      success: true,
      data: [{ companyId: 1, companyName: 'Company 1', tenantId: 1 }]
    };

    service.getCompanies(1, 'search', 1, 10).subscribe(res => {
      expect(res.success).toBeTrue();
      expect(res.data?.length).toBe(1);
    });

    const req = httpMock.expectOne(request => 
      request.url.includes('/companies') && 
      request.params.get('tenantId') === '1'
    );
    expect(req.request.method).toBe('GET');
    req.flush(dummyResponse);
  });
});
