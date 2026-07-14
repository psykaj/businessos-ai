import { Bot } from "lucide-react";

export default function AIAssistantPage() {
  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight text-foreground">
          AI Assistant
        </h1>
        <p className="mt-1 text-sm text-muted-foreground">
          Your intelligent business assistant powered by AI.
        </p>
      </div>

      <div className="flex h-96 flex-col items-center justify-center rounded-xl border border-dashed border-border bg-card">
        <Bot className="h-12 w-12 text-muted-foreground/40" />
        <p className="mt-4 text-sm font-medium text-muted-foreground">
          AI Assistant coming soon
        </p>
        <p className="mt-1 text-xs text-muted-foreground/70">
          Chat with AI to manage your business operations
        </p>
      </div>
    </div>
  );
}
