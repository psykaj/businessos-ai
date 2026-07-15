import type { Metadata } from "next";

export const metadata: Metadata = {
  title: "BusinessOS AI — Authentication",
  description: "Sign in or create your BusinessOS AI account.",
};

export default function AuthLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="relative flex min-h-screen flex-col items-center justify-center overflow-hidden bg-background px-4 py-12">
      {/* Decorative gradient blobs */}
      <div
        aria-hidden="true"
        className="pointer-events-none absolute -top-40 left-1/2 h-[500px] w-[700px] -translate-x-1/2 rounded-full bg-primary/5 blur-3xl"
      />
      <div
        aria-hidden="true"
        className="pointer-events-none absolute bottom-0 right-0 h-[300px] w-[400px] rounded-full bg-primary/5 blur-3xl"
      />

      {/* Content */}
      <main className="relative z-10 w-full">{children}</main>

      {/* Bottom brand strip */}
      <footer className="relative z-10 mt-8 text-center text-xs text-muted-foreground">
        © {new Date().getFullYear()} BusinessOS AI. All rights reserved.
      </footer>
    </div>
  );
}
