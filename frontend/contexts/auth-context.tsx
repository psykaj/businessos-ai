"use client";

import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useState,
} from "react";
import { useRouter } from "next/navigation";
import { authService } from "@/lib/auth-service";
import type { LoginRequest, RegisterRequest, User } from "@/types/auth";
import { toast } from "sonner";

interface AuthContextValue {
  user: User | null;
  isLoading: boolean;
  isAuthenticated: boolean;
  login: (data: LoginRequest) => Promise<void>;
  register: (data: RegisterRequest) => Promise<void>;
  logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const router = useRouter();

  // ── Restore session on mount ─────────────────────────────────────────────
  useEffect(() => {
    const restoreSession = () => {
      const token = authService.getStoredToken();
      const storedUser = authService.getStoredUser();

      if (token && storedUser && !authService.isTokenExpired(token)) {
        setUser(storedUser);
      } else {
        authService.clearSession();
        setUser(null);
      }
      setIsLoading(false);
    };

    restoreSession();
  }, []);

  // ── Listen for 401 auto-logout events ───────────────────────────────────
  useEffect(() => {
    const handleAuthLogout = () => {
      setUser(null);
      router.push("/login");
    };

    window.addEventListener("auth:logout", handleAuthLogout);
    return () => window.removeEventListener("auth:logout", handleAuthLogout);
  }, [router]);

  // ── Token expiry watchdog (check every 60s) ─────────────────────────────
  useEffect(() => {
    if (!user) return;

    const interval = setInterval(() => {
      const token = authService.getStoredToken();
      if (!token || authService.isTokenExpired(token)) {
        authService.clearSession();
        setUser(null);
        router.push("/login");
      }
    }, 60_000);

    return () => clearInterval(interval);
  }, [user, router]);

  const login = useCallback(
    async (data: LoginRequest) => {
      const response = await authService.login(data);
      authService.saveSession(response);
      setUser(response.user);
      toast.success(`Welcome, ${response.user.fullName}!`);
      router.push("/dashboard");
    },
    [router]
  );

  const register = useCallback(
    async (data: RegisterRequest) => {
      const response = await authService.register(data);
      authService.saveSession(response);
      setUser(response.user);
      toast.success(`Welcome, ${response.user.fullName}!`);
      router.push("/dashboard");
    },
    [router]
  );

  const logout = useCallback(async () => {
    const userName = user?.fullName;
    await authService.logout();
    setUser(null);
    if (userName) {
      toast.success(`Goodbye, ${userName}! See you soon.`);
    }
    router.push("/login");
  }, [router, user]);

  return (
    <AuthContext.Provider
      value={{
        user,
        isLoading,
        isAuthenticated: user !== null,
        login,
        register,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
}
