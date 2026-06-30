import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

import { AuthService } from '../auth/auth.service';

function isAuthEndpoint(url: string): boolean {
  return url.includes('/api/auth/');
}

export const authErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: unknown) => {
      if (
        error instanceof HttpErrorResponse &&
        (error.status === 401 || error.status === 403) &&
        !isAuthEndpoint(req.url)
      ) {
        authService.logout();
        void router.navigateByUrl('/login');
      }

      return throwError(() => error);
    }),
  );
};
