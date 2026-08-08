import { DashboardShell } from "@/components/layout/dashboard-shell";
import { ProtectedRoute } from "@/components/auth/protected-route";
import { Metadata } from "next";

export const metadata: Metadata = {
  title: "AI Automation Engine | BusinessOS AI",
  description: "Automate repetitive tasks with BusinessOS AI workflows.",
};

export default function AutomationLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <ProtectedRoute>
      <DashboardShell>{children}</DashboardShell>
    </ProtectedRoute>
  );
}
