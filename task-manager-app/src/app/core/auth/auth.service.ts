import { HttpClient } from '@angular/common/http';
import { computed, inject, Service, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  AuthResponse,
  LoginRequest,
  RegisterRequest,
} from '../../shared/models/auth.models';

const TOKEN_KEY = 'token';
const EMAIL_KEY = 'email';

@Service()
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  private readonly tokenSignal = signal<string | null>(this.readStoredToken());

  readonly isAuthenticated = computed(() => this.tokenSignal() !== null);
  readonly email = signal<string | null>(sessionStorage.getItem(EMAIL_KEY));

  getToken(): string | null {
    return this.tokenSignal();
  }

  async register(request: RegisterRequest): Promise<void> {
    const response = await firstValueFrom(
      this.http.post<AuthResponse>(`${this.apiUrl}/api/auth/register`, request),
    );
    this.storeAuth(response);
  }

  async login(request: LoginRequest): Promise<void> {
    const response = await firstValueFrom(
      this.http.post<AuthResponse>(`${this.apiUrl}/api/auth/login`, request),
    );
    this.storeAuth(response);
  }

  logout(): void {
    sessionStorage.removeItem(TOKEN_KEY);
    sessionStorage.removeItem(EMAIL_KEY);
    this.tokenSignal.set(null);
    this.email.set(null);
  }

  private readStoredToken(): string | null {
    return sessionStorage.getItem(TOKEN_KEY);
  }

  private storeAuth(response: AuthResponse): void {
    sessionStorage.setItem(TOKEN_KEY, response.token);
    sessionStorage.setItem(EMAIL_KEY, response.email);
    this.tokenSignal.set(response.token);
    this.email.set(response.email);
  }
}
