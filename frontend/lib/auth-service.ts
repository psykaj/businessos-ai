import apiClient from "./api-client";
import type {
  ApiResponse,
  AuthResponse,
  LoginRequest,
  RegisterRequest,
  User,
} from "@/types/auth";

const TOKEN_KEY = "businessos_token";
const USER_KEY = "businessos_user";

export const authService = {
  async register(data: RegisterRequest): Promise<AuthResponse> {
    const response = await apiClient.post<ApiResponse<AuthResponse>>(
      "/api/auth/register",
      data
    );
    return response.data.data!;
  },

  async login(data: LoginRequest): Promise<AuthResponse> {
    const response = await apiClient.post<ApiResponse<AuthResponse>>(
      "/api/auth/login",
      data
    );
    return response.data.data!;
  },

  async getMe(): Promise<User> {
    const response = await apiClient.get<ApiResponse<User>>("/api/auth/me");
    return response.data.data!;
  },

  async logout(): Promise<void> {
    try {
      await apiClient.post("/api/auth/logout");
    } finally {
      authService.clearSession();
    }
  },

  saveSession(authResponse: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, authResponse.token);
    localStorage.setItem(USER_KEY, JSON.stringify(authResponse.user));
    // Sync token to a cookie so Next.js middleware can read it server-side
    const expiresAt = new Date(authResponse.expiresAt).toUTCString();
    document.cookie = `businessos_token=${authResponse.token}; path=/; expires=${expiresAt}; SameSite=Lax`;
  },

  clearSession(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    // Clear the middleware cookie
    document.cookie = "businessos_token=; path=/; max-age=0; SameSite=Lax";
  },

  getStoredToken(): string | null {
    if (typeof window === "undefined") return null;
    return localStorage.getItem(TOKEN_KEY);
  },

  getStoredUser(): User | null {
    if (typeof window === "undefined") return null;
    const raw = localStorage.getItem(USER_KEY);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as User;
    } catch {
      return null;
    }
  },

  isTokenExpired(token: string): boolean {
    try {
      const payload = JSON.parse(atob(token.split(".")[1]));
      return Date.now() >= payload.exp * 1000;
    } catch {
      return true;
    }
  },
};
