import { CheckCircle2, XCircle, TrendingUp } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";

interface YesterdayReviewProps {
  completedCount?: number;
  dismissedCount?: number;
  isLoading?: boolean;
}

export function YesterdayReview({ completedCount = 0, dismissedCount = 0, isLoading }: YesterdayReviewProps) {
  if (isLoading) {
    return (
      <div className="bg-muted/30 border rounded-xl p-5">
        <Skeleton className="h-5 w-32 mb-4" />
        <div className="space-y-3">
          <Skeleton className="h-4 w-48" />
          <Skeleton className="h-4 w-40" />
        </div>
      </div>
    );
  }

  return (
    <div className="bg-muted/30 border rounded-xl p-5 h-full">
      <h3 className="text-sm font-semibold text-foreground mb-4">Yesterday Review</h3>
      
      <div className="space-y-4">
        <div className="flex items-start gap-3">
          <div className="mt-0.5 text-emerald-500">
            <CheckCircle2 className="w-4 h-4" />
          </div>
          <div>
            <p className="text-sm font-medium text-foreground">
              {completedCount} priorities completed
            </p>
          </div>
        </div>
        
        <div className="flex items-start gap-3">
          <div className="mt-0.5 text-muted-foreground">
            <XCircle className="w-4 h-4" />
          </div>
          <div>
            <p className="text-sm font-medium text-foreground">
              {dismissedCount} dismissed
            </p>
          </div>
        </div>

        {/* Hardcoded sample outcome since backend model didn't return this yet for DTO, 
            but this connects to Day 32 outcomes ideally */}
        <div className="flex items-start gap-3 pt-2 border-t border-border/50">
          <div className="mt-0.5 text-primary">
            <TrendingUp className="w-4 h-4" />
          </div>
          <div>
            <p className="text-sm font-medium text-foreground">
              ₹25,000 recovered
            </p>
            <p className="text-xs text-muted-foreground mt-0.5">
              From collections follow-ups
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
