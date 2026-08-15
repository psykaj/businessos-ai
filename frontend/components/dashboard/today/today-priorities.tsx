import { DailyPriorityDto } from "@/lib/daily-operating-loop-service";
import { PriorityCard } from "./priority-card";
import { Skeleton } from "@/components/ui/skeleton";
import { CheckCircle2 } from "lucide-react";

interface TodayPrioritiesProps {
  priorities?: DailyPriorityDto[];
  isLoading?: boolean;
}

export function TodayPriorities({ priorities, isLoading }: TodayPrioritiesProps) {
  if (isLoading) {
    return (
      <div className="space-y-4">
        <Skeleton className="h-28 w-full rounded-xl" />
        <Skeleton className="h-28 w-full rounded-xl" />
        <Skeleton className="h-28 w-full rounded-xl" />
      </div>
    );
  }

  if (!priorities || priorities.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-12 px-4 text-center bg-muted/20 border border-dashed rounded-xl">
        <div className="w-12 h-12 rounded-full bg-emerald-500/10 text-emerald-500 flex items-center justify-center mb-4">
          <CheckCircle2 className="w-6 h-6" />
        </div>
        <h3 className="text-lg font-semibold text-foreground">You're all caught up</h3>
        <p className="text-sm text-muted-foreground mt-1 max-w-sm">
          BusinessOS AI found no critical issues requiring immediate attention today. Great job keeping on top of things!
        </p>
      </div>
    );
  }

  return (
    <div className="space-y-4">
      {priorities.map((priority) => (
        <PriorityCard key={priority.id} priority={priority} />
      ))}
    </div>
  );
}
