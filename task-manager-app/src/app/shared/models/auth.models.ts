export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  email: string;
}

export interface ProblemDetails {
  title?: string;
  detail?: string;
  status?: number;
}
