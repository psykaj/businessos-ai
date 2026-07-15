"use client";

import { useState } from "react";
import type { Metadata } from "next";
import Link from "next/link";
import { Eye, EyeOff, Loader2 } from "lucide-react";
import { useAuth } from "@/contexts/auth-context";
import { AuthFormWrapper } from "@/components/auth/auth-form-wrapper";
import { FormField } from "@/components/auth/form-field";
import type { LoginRequest } from "@/types/auth";
import axios from "axios";

interface FormErrors {
  email?: string;
  password?: string;
  general?: string;
}

function validate(form: LoginRequest): FormErrors {
  const errors: FormErrors = {};
  if (!form.email) errors.email = "Email is required.";
  else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email))
    errors.email = "Enter a valid email address.";
  if (!form.password) errors.password = "Password is required.";
  return errors;
}

export default function LoginPage() {
  const { login } = useAuth();
  const [form, setForm] = useState<LoginRequest>({ email: "", password: "" });
  const [errors, setErrors] = useState<FormErrors>({});
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
    if (errors[name as keyof FormErrors]) {
      setErrors((prev) => ({ ...prev, [name]: undefined, general: undefined }));
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const validationErrors = validate(form);
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }
    setIsLoading(true);
    setErrors({});
    try {
      await login(form);
    } catch (err) {
      let message = "An unexpected error occurred. Please try again.";
      if (axios.isAxiosError(err)) {
        const data = err.response?.data;
        message = data?.message ?? message;
      }
      setErrors({ general: message });
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="flex justify-center">
      <AuthFormWrapper
        title="Welcome back"
        subtitle="Sign in to your BusinessOS AI account"
        footer={{
          text: "Don't have an account?",
          linkText: "Create one",
          href: "/register",
        }}
      >
        <form onSubmit={handleSubmit} noValidate className="flex flex-col gap-5">
          {/* General error */}
          {errors.general && (
            <div
              role="alert"
              className="rounded-lg border border-destructive/30 bg-destructive/10 px-4 py-3 text-sm text-destructive"
            >
              {errors.general}
            </div>
          )}

          <FormField
            id="login-email"
            label="Email address"
            type="email"
            name="email"
            placeholder="you@company.com"
            autoComplete="email"
            value={form.email}
            onChange={handleChange}
            error={errors.email}
            disabled={isLoading}
          />

          <div className="flex flex-col gap-1.5">
            <div className="flex items-center justify-between">
              <label
                htmlFor="login-password"
                className="text-sm font-medium text-foreground"
              >
                Password
              </label>
              <Link
                href="/forgot-password"
                className="text-xs font-medium text-primary underline-offset-4 hover:underline"
              >
                Forgot password?
              </Link>
            </div>
            <div className="relative">
              <input
                id="login-password"
                type={showPassword ? "text" : "password"}
                name="password"
                placeholder="••••••••"
                autoComplete="current-password"
                value={form.password}
                onChange={handleChange}
                disabled={isLoading}
                className={`h-11 w-full rounded-lg border bg-background px-3.5 pr-11 text-sm text-foreground placeholder:text-muted-foreground outline-none ring-offset-background transition-all duration-150 focus:ring-2 focus:ring-primary/20 ${
                  errors.password
                    ? "border-destructive focus:ring-destructive/20"
                    : "border-input focus:border-primary"
                }`}
              />
              <button
                type="button"
                onClick={() => setShowPassword((v) => !v)}
                className="absolute right-3 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground"
                aria-label={showPassword ? "Hide password" : "Show password"}
              >
                {showPassword ? (
                  <EyeOff className="h-4 w-4" />
                ) : (
                  <Eye className="h-4 w-4" />
                )}
              </button>
            </div>
            {errors.password && (
              <p className="text-xs font-medium text-destructive">
                {errors.password}
              </p>
            )}
          </div>

          <button
            type="submit"
            id="login-submit"
            disabled={isLoading}
            className="mt-1 flex h-11 w-full items-center justify-center gap-2 rounded-lg bg-primary text-sm font-semibold text-primary-foreground transition-all duration-150 hover:opacity-90 active:scale-[0.98] disabled:cursor-not-allowed disabled:opacity-60"
          >
            {isLoading && <Loader2 className="h-4 w-4 animate-spin" />}
            {isLoading ? "Signing in…" : "Sign in"}
          </button>
        </form>
      </AuthFormWrapper>
    </div>
  );
}
