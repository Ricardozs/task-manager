import { HttpErrorResponse } from '@angular/common/http';

import { ProblemDetails } from '../models/auth.models';

export function getApiErrorMessage(error: unknown): string {
  if (error instanceof HttpErrorResponse) {
    const problem = error.error as ProblemDetails | undefined;
    if (problem?.detail) {
      return problem.detail;
    }

    if (error.status === 401) {
      return 'Invalid email or password.';
    }

    if (error.status === 409) {
      return 'Email is already registered.';
    }
  }

  return 'Something went wrong. Please try again.';
}
