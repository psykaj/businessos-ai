export interface User {
  id: string;
  fullName: string;
  email: string;
  role: string;
  isActive: boolean;
  createdAt: string;
}

export interface AuthResponse {
  token: string;
  expiresAt: string;
  user: User;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface ApiResponse<T> {
  success: boolean;
  data: T | null;
  message: string;
  errors: string[];
  timestamp: string;
}
