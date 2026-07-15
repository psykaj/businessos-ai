"use client";

import { useState } from "react";
import { CheckCircle, Loader2, Mail } from "lucide-react";
import { AuthFormWrapper } from "@/components/auth/auth-form-wrapper";
import { FormField } from "@/components/auth/form-field";

export default function ForgotPasswordPage() {
  const [email, setEmail] = useState("");
  const [emailError, setEmailError] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [submitted, setSubmitted] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!email) {
      setEmailError("Email is required.");
      return;
    }
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      setEmailError("Enter a valid email address.");
      return;
    }
    setEmailError("");
    setIsLoading(true);

    // Simulate API call — forgot password backend is Day 4+
    await new Promise((resolve) => setTimeout(resolve, 1200));
    setIsLoading(false);
    setSubmitted(true);
  };

  return (
    <div className="flex justify-center">
      <AuthFormWrapper
        title="Reset your password"
        subtitle="We'll send a reset link to your email"
        footer={{
          text: "Remember your password?",
          linkText: "Back to sign in",
          href: "/login",
        }}
      >
        {submitted ? (
          <div className="flex flex-col items-center gap-4 py-4 text-center">
            <div className="flex h-14 w-14 items-center justify-center rounded-full bg-primary/10">
              <CheckCircle className="h-7 w-7 text-primary" />
            </div>
            <div>
              <p className="font-semibold text-foreground">Check your inbox</p>
              <p className="mt-1 text-sm text-muted-foreground">
                If an account exists for{" "}
                <span className="font-medium text-foreground">{email}</span>,
                you&apos;ll receive a reset link shortly.
              </p>
            </div>
          </div>
        ) : (
          <form onSubmit={handleSubmit} noValidate className="flex flex-col gap-5">
            <div className="flex items-center gap-3 rounded-lg bg-muted px-4 py-3 text-sm text-muted-foreground">
              <Mail className="h-4 w-4 shrink-0 text-primary" />
              Enter the email associated with your account and we&apos;ll send
              you a link to reset your password.
            </div>

            <FormField
              id="forgot-email"
              label="Email address"
              type="email"
              name="email"
              placeholder="you@company.com"
              autoComplete="email"
              value={email}
              onChange={(e) => {
                setEmail(e.target.value);
                setEmailError("");
              }}
              error={emailError}
              disabled={isLoading}
            />

            <button
              type="submit"
              id="forgot-password-submit"
              disabled={isLoading}
              className="flex h-11 w-full items-center justify-center gap-2 rounded-lg bg-primary text-sm font-semibold text-primary-foreground transition-all duration-150 hover:opacity-90 active:scale-[0.98] disabled:cursor-not-allowed disabled:opacity-60"
            >
              {isLoading && <Loader2 className="h-4 w-4 animate-spin" />}
              {isLoading ? "Sending link…" : "Send reset link"}
            </button>
          </form>
        )}
      </AuthFormWrapper>
    </div>
  );
}
