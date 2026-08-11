import { Settings, BrainCircuit } from "lucide-react";
import Link from "next/link";

export default function SettingsPage() {
  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight text-foreground">
          Settings
        </h1>
        <p className="mt-1 text-sm text-muted-foreground">
          Configure your account, preferences, and integrations.
        </p>
      </div>

      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
        {/* Business Memory Card */}
        <Link href="/dashboard/settings/business-memory" className="block">
          <div className="group rounded-xl border border-border bg-card p-6 shadow-sm transition-all hover:border-indigo-500/50 hover:shadow-md cursor-pointer">
            <div className="flex items-center gap-4">
              <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-indigo-500/10 text-indigo-500">
                <BrainCircuit className="h-5 w-5" />
              </div>
              <div className="space-y-1">
                <h3 className="font-semibold leading-none tracking-tight text-foreground group-hover:text-indigo-600 transition-colors">
                  Business Memory
                </h3>
                <p className="text-sm text-muted-foreground">
                  Manage the context BusinessOS AI uses for recommendations.
                </p>
              </div>
            </div>
          </div>
        </Link>
        
        {/* Empty Placeholder Card */}
        <div className="flex h-32 flex-col items-center justify-center rounded-xl border border-dashed border-border bg-card">
          <Settings className="h-8 w-8 text-muted-foreground/40 mb-2" />
          <p className="text-sm font-medium text-muted-foreground">
            More Settings
          </p>
        </div>
      </div>
    </div>
  );
}
