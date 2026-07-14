import { MetricsGrid } from "@/components/dashboard/metrics-grid";

export default function DashboardPage() {
  return (
    <div className="flex flex-col gap-8">
      {/* Page header */}
      <div>
        <h1 className="text-2xl font-semibold tracking-tight text-foreground">
          Dashboard
        </h1>
        <p className="mt-1 text-sm text-muted-foreground">
          Welcome back. Here&apos;s an overview of your business.
        </p>
      </div>

      {/* Metrics */}
      <MetricsGrid />

      {/* Placeholder for future widgets */}
      <div className="grid grid-cols-1 gap-4 lg:grid-cols-2">
        <div className="rounded-xl border border-border bg-card p-6">
          <h2 className="text-sm font-medium text-muted-foreground">
            Recent Activity
          </h2>
          <div className="mt-4 flex h-48 items-center justify-center rounded-lg border border-dashed border-border">
            <p className="text-sm text-muted-foreground">
              Activity feed coming soon
            </p>
          </div>
        </div>
        <div className="rounded-xl border border-border bg-card p-6">
          <h2 className="text-sm font-medium text-muted-foreground">
            Revenue Overview
          </h2>
          <div className="mt-4 flex h-48 items-center justify-center rounded-lg border border-dashed border-border">
            <p className="text-sm text-muted-foreground">
              Charts coming soon
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
