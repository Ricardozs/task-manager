import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import {
  email,
  form,
  FormField,
  minLength,
  required,
  submit,
} from '@angular/forms/signals';

import { AuthService } from '../../../core/auth/auth.service';
import { RegisterRequest } from '../../../shared/models/auth.models';
import { getApiErrorMessage } from '../../../shared/utils/api-error';

const MIN_PASSWORD_LENGTH = 8;

@Component({
  selector: 'app-register',
  imports: [FormField, RouterLink],
  template: `
    <main class="auth-page">
      <section class="auth-card" aria-labelledby="register-heading">
        <h1 id="register-heading">Create account</h1>
        <p class="auth-subtitle">Register to start managing your tasks.</p>

        <form (submit)="onSubmit($event)" novalidate>
          @if (apiError()) {
            <p class="form-error" role="alert">{{ apiError() }}</p>
          }

          <div class="form-field">
            <label for="register-email">Email</label>
            <input
              id="register-email"
              type="email"
              autocomplete="email"
              [formField]="registerForm.email"
            />
            @if (registerForm.email().touched() && registerForm.email().invalid()) {
              <ul class="field-errors">
                @for (error of registerForm.email().errors(); track error.kind) {
                  <li>{{ error.message }}</li>
                }
              </ul>
            }
          </div>

          <div class="form-field">
            <label for="register-password">Password</label>
            <input
              id="register-password"
              type="password"
              autocomplete="new-password"
              [formField]="registerForm.password"
            />
            @if (registerForm.password().touched() && registerForm.password().invalid()) {
              <ul class="field-errors">
                @for (error of registerForm.password().errors(); track error.kind) {
                  <li>{{ error.message }}</li>
                }
              </ul>
            }
          </div>

          <button type="submit" [disabled]="submitting()">
            {{ submitting() ? 'Creating account…' : 'Register' }}
          </button>
        </form>

        <p class="auth-footer">
          Already have an account?
          <a routerLink="/login">Log in</a>
        </p>
      </section>
    </main>
  `,
})
export class Register {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  protected readonly submitting = signal(false);
  protected readonly apiError = signal<string | null>(null);

  protected readonly registerModel = signal<RegisterRequest>({
    email: '',
    password: '',
  });

  protected readonly registerForm = form(this.registerModel, (schemaPath) => {
    required(schemaPath.email, { message: 'Email is required' });
    email(schemaPath.email, { message: 'Enter a valid email address' });
    required(schemaPath.password, { message: 'Password is required' });
    minLength(schemaPath.password, MIN_PASSWORD_LENGTH, {
      message: `Password must be at least ${MIN_PASSWORD_LENGTH} characters`,
    });
  });

  protected async onSubmit(event: Event): Promise<void> {
    event.preventDefault();
    this.apiError.set(null);
    this.submitting.set(true);

    try {
      await submit(this.registerForm, async () => {
        try {
          await this.authService.register(this.registerModel());
          await this.router.navigateByUrl('/tasks');
        } catch (error) {
          this.apiError.set(getApiErrorMessage(error));
        }
      });
    } finally {
      this.submitting.set(false);
    }
  }
}
