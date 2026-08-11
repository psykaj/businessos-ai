import { MemoryListTable } from "@/components/settings/MemoryListTable";
import { BrainCircuit } from "lucide-react";

export default function BusinessMemorySettingsPage() {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight text-foreground flex items-center gap-2">
          <BrainCircuit className="h-6 w-6 text-indigo-500" />
          Business Memory
        </h1>
        <p className="mt-1 text-sm text-muted-foreground">
          Manage the business context BusinessOS AI uses to provide more relevant recommendations.
        </p>
      </div>

      <MemoryListTable />
    </div>
  );
}
