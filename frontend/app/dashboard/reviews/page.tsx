import { Star } from "lucide-react";

export default function ReviewsPage() {
  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight text-foreground">
          Reviews
        </h1>
        <p className="mt-1 text-sm text-muted-foreground">
          Monitor and respond to customer reviews with AI assistance.
        </p>
      </div>

      <div className="flex h-96 flex-col items-center justify-center rounded-xl border border-dashed border-border bg-card">
        <Star className="h-12 w-12 text-muted-foreground/40" />
        <p className="mt-4 text-sm font-medium text-muted-foreground">
          Review management coming soon
        </p>
        <p className="mt-1 text-xs text-muted-foreground/70">
          AI-powered review monitoring and response generation
        </p>
      </div>
    </div>
  );
}
