"use client";

import { useState } from "react";
import { Eye, EyeOff, Loader2 } from "lucide-react";
import { useAuth } from "@/contexts/auth-context";
import { AuthFormWrapper } from "@/components/auth/auth-form-wrapper";
import { FormField } from "@/components/auth/form-field";
import type { RegisterRequest } from "@/types/auth";
import axios from "axios";

interface FormErrors {
  fullName?: string;
  email?: string;
  password?: string;
  confirmPassword?: string;
  general?: string;
}

function validate(form: RegisterRequest): FormErrors {
  const errors: FormErrors = {};
  if (!form.fullName.trim()) errors.fullName = "Full name is required.";
  if (!form.email) errors.email = "Email is required.";
  else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email))
    errors.email = "Enter a valid email address.";
  if (!form.password) errors.password = "Password is required.";
  else if (form.password.length < 8)
    errors.password = "Password must be at least 8 characters.";
  else if (!/[A-Z]/.test(form.password))
    errors.password = "Password must contain at least one uppercase letter.";
  else if (!/[0-9]/.test(form.password))
    errors.password = "Password must contain at least one digit.";
  if (!form.confirmPassword)
    errors.confirmPassword = "Please confirm your password.";
  else if (form.password !== form.confirmPassword)
    errors.confirmPassword = "Passwords do not match.";
  return errors;
}

function PasswordInput({
  id,
  name,
  label,
  value,
  onChange,
  disabled,
  show,
  onToggle,
  error,
}: {
  id: string;
  name: string;
  label: string;
  value: string;
  onChange: React.ChangeEventHandler<HTMLInputElement>;
  disabled: boolean;
  show: boolean;
  onToggle: () => void;
  error?: string;
}) {
  return (
    <div className="flex flex-col gap-1.5">
      <label htmlFor={id} className="text-sm font-medium text-foreground">
        {label}
      </label>
      <div className="relative">
        <input
          id={id}
          type={show ? "text" : "password"}
          name={name}
          placeholder="••••••••"
          autoComplete="new-password"
          value={value}
          onChange={onChange}
          disabled={disabled}
          className={`h-11 w-full rounded-lg border bg-background px-3.5 pr-11 text-sm text-foreground placeholder:text-muted-foreground outline-none ring-offset-background transition-all duration-150 focus:ring-2 focus:ring-primary/20 ${
            error
              ? "border-destructive focus:ring-destructive/20"
              : "border-input focus:border-primary"
          }`}
        />
        <button
          type="button"
          onClick={onToggle}
          className="absolute right-3 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground"
          aria-label={show ? "Hide password" : "Show password"}
        >
          {show ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
        </button>
      </div>
      {error && <p className="text-xs font-medium text-destructive">{error}</p>}
    </div>
  );
}


export default function RegisterPage() {
  const { register } = useAuth();
  const [form, setForm] = useState<RegisterRequest>({
    fullName: "",
    email: "",
    password: "",
    confirmPassword: "",
  });
  const [errors, setErrors] = useState<FormErrors>({});
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);
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
      await register(form);
    } catch (err) {
      let message = "An unexpected error occurred. Please try again.";
      if (axios.isAxiosError(err)) {
        const data = err.response?.data;
        const serverErrors: string[] = data?.errors ?? [];
        message = serverErrors.length > 0 ? serverErrors[0] : (data?.message ?? message);
      }
      setErrors({ general: message });
    } finally {
      setIsLoading(false);
    }
  };



  return (
    <div className="flex justify-center">
      <AuthFormWrapper
        title="Create your account"
        subtitle="Start your Simplify journey today"
        footer={{
          text: "Already have an account?",
          linkText: "Sign in",
          href: "/login",
        }}
      >
        <form onSubmit={handleSubmit} noValidate className="flex flex-col gap-4">
          {errors.general && (
            <div
              role="alert"
              className="rounded-lg border border-destructive/30 bg-destructive/10 px-4 py-3 text-sm text-destructive"
            >
              {errors.general}
            </div>
          )}

          <FormField
            id="register-fullname"
            label="Full name"
            type="text"
            name="fullName"
            placeholder="Pankaj Yadav"
            autoComplete="name"
            value={form.fullName}
            onChange={handleChange}
            error={errors.fullName}
            disabled={isLoading}
          />

          <FormField
            id="register-email"
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

          <PasswordInput
            id="register-password"
            name="password"
            label="Password"
            value={form.password}
            onChange={handleChange}
            disabled={isLoading}
            show={showPassword}
            onToggle={() => setShowPassword((v) => !v)}
            error={errors.password}
          />

          <PasswordInput
            id="register-confirm-password"
            name="confirmPassword"
            label="Confirm password"
            value={form.confirmPassword}
            onChange={handleChange}
            disabled={isLoading}
            show={showConfirm}
            onToggle={() => setShowConfirm((v) => !v)}
            error={errors.confirmPassword}
          />

          <p className="text-xs text-muted-foreground">
            Password must be 8+ characters with one uppercase letter and one digit.
          </p>

          <button
            type="submit"
            id="register-submit"
            disabled={isLoading}
            className="mt-1 flex h-11 w-full items-center justify-center gap-2 rounded-lg bg-primary text-sm font-semibold text-primary-foreground transition-all duration-150 hover:opacity-90 active:scale-[0.98] disabled:cursor-not-allowed disabled:opacity-60"
          >
            {isLoading && <Loader2 className="h-4 w-4 animate-spin" />}
            {isLoading ? "Creating account…" : "Create account"}
          </button>
        </form>
      </AuthFormWrapper>
    </div>
  );
}
