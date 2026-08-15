import { Sparkles } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";

interface TodaySummaryProps {
  summary?: string;
  isLoading?: boolean;
}

export function TodaySummary({ summary, isLoading }: TodaySummaryProps) {
  if (isLoading) {
    return (
      <div className="flex flex-col gap-2">
        <Skeleton className="h-6 w-full max-w-3xl" />
        <Skeleton className="h-6 w-4/5 max-w-2xl" />
      </div>
    );
  }

  if (!summary) return null;

  return (
    <div className="flex gap-3">
      <div className="mt-1">
        <div className="flex h-6 w-6 items-center justify-center rounded-full bg-primary/10 text-primary">
          <Sparkles className="h-3.5 w-3.5" />
        </div>
      </div>
      <p className="text-lg text-foreground font-medium leading-relaxed max-w-4xl">
        {summary}
      </p>
    </div>
  );
}
