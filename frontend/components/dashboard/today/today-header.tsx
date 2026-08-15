import { Button } from "@/components/ui/button";
import { RefreshCcw } from "lucide-react";
import { format } from "date-fns";
import { useEffect, useState } from "react";

interface TodayHeaderProps {
  onRefresh: () => void;
  isRefetching: boolean;
  generatedAt?: string;
}

export function TodayHeader({ onRefresh, isRefetching, generatedAt }: TodayHeaderProps) {
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
  }, []);

  const today = mounted ? format(new Date(), "EEEE, MMMM d, yyyy") : "";
  const lastUpdated = generatedAt && mounted ? format(new Date(generatedAt), "h:mm a") : "";

  return (
    <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight text-foreground">
          Today
        </h1>
        <p className="mt-1 text-sm text-muted-foreground">
          {today}. Here is what matters most for your business today.
        </p>
      </div>
      <div className="flex items-center gap-4">
        {lastUpdated && (
          <span className="text-xs text-muted-foreground hidden sm:inline-block">
            Updated {lastUpdated}
          </span>
        )}
        <Button 
          variant="outline" 
          size="sm" 
          onClick={onRefresh} 
          disabled={isRefetching}
          className="h-8"
        >
          <RefreshCcw className={`w-3.5 h-3.5 mr-2 ${isRefetching ? "animate-spin" : ""}`} />
          Refresh
        </Button>
      </div>
    </div>
  );
}
