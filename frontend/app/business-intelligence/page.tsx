import { DashboardShell } from "@/components/layout/dashboard-shell";
import { ProtectedRoute } from "@/components/auth/protected-route";
import { BiDashboardContent } from "@/components/business-intelligence/bi-dashboard-content";
import { Metadata } from "next";

export const metadata: Metadata = {
  title: "AI Business Intelligence Engine | BusinessOS AI",
  description: "Executive 0-100 Business Health Score, predictive churn warning triggers, revenue velocity trends, and interactive AI recommendation execution.",
};

export default function BusinessIntelligencePage() {
  return (
    <ProtectedRoute>
      <DashboardShell>
        <BiDashboardContent />
      </DashboardShell>
    </ProtectedRoute>
  );
}
