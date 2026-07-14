import { Settings } from "lucide-react";

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

      <div className="flex h-96 flex-col items-center justify-center rounded-xl border border-dashed border-border bg-card">
        <Settings className="h-12 w-12 text-muted-foreground/40" />
        <p className="mt-4 text-sm font-medium text-muted-foreground">
          Settings panel coming soon
        </p>
        <p className="mt-1 text-xs text-muted-foreground/70">
          Account, billing, team, and integration settings
        </p>
      </div>
    </div>
  );
}
