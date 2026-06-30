import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';

import { AuthService } from '../auth/auth.service';
import { authErrorInterceptor } from './auth-error.interceptor';

describe('authErrorInterceptor', () => {
  let http: HttpClient;
  let httpMock: HttpTestingController;
  let authService: AuthService;
  let router: Router;

  beforeEach(() => {
    sessionStorage.clear();

    TestBed.configureTestingModule({
      providers: [
        AuthService,
        provideRouter([]),
        provideHttpClient(withInterceptors([authErrorInterceptor])),
        provideHttpClientTesting(),
      ],
    });

    http = TestBed.inject(HttpClient);
    httpMock = TestBed.inject(HttpTestingController);
    authService = TestBed.inject(AuthService);
    router = TestBed.inject(Router);
  });

  afterEach(() => {
    httpMock.verify();
    sessionStorage.clear();
  });

  it('logs out and redirects on 401 for protected endpoints', async () => {
    sessionStorage.setItem('token', 'test-token');
    const navigateSpy = vi.spyOn(router, 'navigateByUrl').mockResolvedValue(true);

    const promise = firstValueFrom(http.get('/api/tasks')).catch((err) => err);

    const request = httpMock.expectOne('/api/tasks');
    request.flush(
      { detail: 'Unauthorized' },
      { status: 401, statusText: 'Unauthorized' },
    );

    const error = await promise;
    expect(error.status).toBe(401);
    expect(authService.isAuthenticated()).toBe(false);
    expect(navigateSpy).toHaveBeenCalledWith('/login');
  });

  it('does not redirect on 401 for auth endpoints', async () => {
    const navigateSpy = vi.spyOn(router, 'navigateByUrl').mockResolvedValue(true);

    const promise = firstValueFrom(
      http.post('/api/auth/login', { email: 'a@b.com', password: 'secret' }),
    ).catch((err) => err);

    const request = httpMock.expectOne('/api/auth/login');
    request.flush(
      { detail: 'Invalid credentials' },
      { status: 401, statusText: 'Unauthorized' },
    );

    const error = await promise;
    expect(error.status).toBe(401);
    expect(navigateSpy).not.toHaveBeenCalled();
  });
});
