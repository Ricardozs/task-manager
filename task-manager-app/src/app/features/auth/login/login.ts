import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { email, form, FormField, minLength, required, submit } from '@angular/forms/signals';

import { AuthService } from '../../../core/auth/auth.service';
import { LoginRequest } from '../../../shared/models/auth.models';
import { getApiErrorMessage } from '../../../shared/utils/api-error';

@Component({
  selector: 'app-login',
  imports: [FormField, RouterLink],
  template: `
    <main class="auth-page">
      <section class="auth-card" aria-labelledby="login-heading">
        <h1 id="login-heading">Log in</h1>
        <p class="auth-subtitle">Access your tasks with your account.</p>

        <form (submit)="onSubmit($event)" novalidate>
          @if (apiError()) {
            <p class="form-error" role="alert">{{ apiError() }}</p>
          }

          <div class="form-field">
            <label for="login-email">Email</label>
            <input
              id="login-email"
              type="email"
              autocomplete="email"
              [formField]="loginForm.email"
            />
            @if (loginForm.email().touched() && loginForm.email().invalid()) {
              <ul class="field-errors">
                @for (error of loginForm.email().errors(); track error.kind) {
                  <li>{{ error.message }}</li>
                }
              </ul>
            }
          </div>

          <div class="form-field">
            <label for="login-password">Password</label>
            <input
              id="login-password"
              type="password"
              autocomplete="current-password"
              [formField]="loginForm.password"
            />
            @if (loginForm.password().touched() && loginForm.password().invalid()) {
              <ul class="field-errors">
                @for (error of loginForm.password().errors(); track error.kind) {
                  <li>{{ error.message }}</li>
                }
              </ul>
            }
          </div>

          <button type="submit" [disabled]="submitting()">
            {{ submitting() ? 'Logging in…' : 'Log in' }}
          </button>
        </form>

        <p class="auth-footer">
          No account yet?
          <a routerLink="/register">Register</a>
        </p>
      </section>
    </main>
  `,
})
export class Login {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  protected readonly submitting = signal(false);
  protected readonly apiError = signal<string | null>(null);

  protected readonly loginModel = signal<LoginRequest>({
    email: '',
    password: '',
  });

  protected readonly loginForm = form(this.loginModel, (schemaPath) => {
    required(schemaPath.email, { message: 'Email is required' });
    email(schemaPath.email, { message: 'Enter a valid email address' });
    required(schemaPath.password, { message: 'Password is required' });
  });

  protected async onSubmit(event: Event): Promise<void> {
    event.preventDefault();
    this.apiError.set(null);
    this.submitting.set(true);

    try {
      await submit(this.loginForm, async () => {
        try {
          await this.authService.login(this.loginModel());
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
