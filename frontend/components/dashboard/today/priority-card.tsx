import { useState } from "react";
import { 
  CheckCircle2, 
  Clock, 
  ArrowRight,
  MoreVertical,
  X,
  Zap,
  Target
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { 
  DailyPriorityDto, 
  useCompletePriority, 
  useDismissPriority, 
  useSnoozePriority 
} from "@/lib/daily-operating-loop-service";
import { PriorityDrawer } from "./priority-drawer";
import { cn } from "@/lib/utils";

interface PriorityCardProps {
  priority: DailyPriorityDto;
}

export function PriorityCard({ priority }: PriorityCardProps) {
  const [drawerOpen, setDrawerOpen] = useState(false);
  
  const completeMutation = useCompletePriority();
  const dismissMutation = useDismissPriority();
  const snoozeMutation = useSnoozePriority();

  const isActionLoading = completeMutation.isPending || dismissMutation.isPending || snoozeMutation.isPending;

  const handleComplete = (e?: React.MouseEvent) => {
    e?.stopPropagation();
    completeMutation.mutate(priority.id);
  };

  const handleDismiss = (e?: React.MouseEvent) => {
    e?.stopPropagation();
    dismissMutation.mutate(priority.id);
  };

  const handleSnooze = (e?: React.MouseEvent) => {
    e?.stopPropagation();
    snoozeMutation.mutate({ id: priority.id });
  };

  const getActionLabel = (type: string) => {
    switch (type) {
      case "CustomerFollowUp": return "View Customer";
      case "Collections": return "View Invoice";
      case "UrgentRisk": return "Review Risk";
      case "GoalAtRisk": return "View Goal";
      default: return "Take Action";
    }
  };

  return (
    <>
      <div 
        className={cn(
          "group relative bg-card border border-border/60 hover:border-border rounded-xl p-5 transition-all duration-200 shadow-sm hover:shadow-md cursor-pointer overflow-hidden",
          isActionLoading && "opacity-50 pointer-events-none"
        )}
        onClick={() => setDrawerOpen(true)}
      >
        {/* Priority Indicator Line */}
        <div className="absolute left-0 top-0 bottom-0 w-1 bg-gradient-to-b from-primary/80 to-primary/20"></div>

        <div className="flex flex-col sm:flex-row sm:items-start justify-between gap-4">
          
          <div className="flex-1 space-y-3">
            <div className="flex items-center gap-2">
              <Badge variant="outline" className="uppercase text-[10px] tracking-wider font-semibold text-muted-foreground border-border/80 bg-background">
                {priority.priorityType.replace(/([A-Z])/g, ' $1').trim()}
              </Badge>
              {priority.severity === "Critical" && (
                <Badge variant="destructive" className="uppercase text-[10px] tracking-wider font-semibold">
                  Critical
                </Badge>
              )}
            </div>
            
            <div>
              <h3 className="text-base sm:text-lg font-semibold text-foreground group-hover:text-primary transition-colors line-clamp-1">
                {priority.title}
              </h3>
              <p className="text-sm text-muted-foreground mt-1 line-clamp-2">
                {priority.reason}
              </p>
            </div>
            
            {/* Quick Context Footer */}
            <div className="flex items-center gap-4 text-xs font-medium text-muted-foreground pt-1">
              <div className="flex items-center gap-1.5 text-emerald-600 dark:text-emerald-400 bg-emerald-500/10 px-2 py-0.5 rounded-sm">
                <Target className="w-3.5 h-3.5" />
                {priority.expectedImpact}
              </div>
              <div className="flex items-center gap-1.5">
                <Zap className="w-3.5 h-3.5" />
                {priority.confidence}% Confidence
              </div>
            </div>
          </div>

          {/* Quick Actions (Desktop mainly, folds on mobile) */}
          <div className="flex items-center gap-2 sm:self-start mt-2 sm:mt-0" onClick={e => e.stopPropagation()}>
            <Button size="sm" variant="outline" className="hidden sm:flex h-8 text-xs bg-background hover:bg-emerald-500/10 hover:text-emerald-600 hover:border-emerald-500/30 transition-colors" onClick={handleComplete}>
              <CheckCircle2 className="w-3.5 h-3.5 mr-1.5" /> Complete
            </Button>
            
            <Button size="sm" className="h-8 text-xs w-full sm:w-auto px-4" onClick={(e) => { e.stopPropagation(); /* TODO route to entity */ }}>
              {getActionLabel(priority.priorityType)} <ArrowRight className="w-3.5 h-3.5 ml-1.5" />
            </Button>
            
            <DropdownMenu>
              <DropdownMenuTrigger className="inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 hover:bg-accent hover:text-accent-foreground h-8 w-8 text-muted-foreground">
                  <MoreVertical className="w-4 h-4" />
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DropdownMenuItem onClick={handleComplete} className="text-emerald-600">
                  <CheckCircle2 className="w-4 h-4 mr-2" /> Mark Complete
                </DropdownMenuItem>
                <DropdownMenuItem onClick={handleSnooze}>
                  <Clock className="w-4 h-4 mr-2" /> Snooze
                </DropdownMenuItem>
                <DropdownMenuItem onClick={handleDismiss} className="text-destructive">
                  <X className="w-4 h-4 mr-2" /> Dismiss
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          </div>
        </div>
      </div>

      <PriorityDrawer 
        priority={priority} 
        open={drawerOpen} 
        onOpenChange={setDrawerOpen}
        onComplete={() => { setDrawerOpen(false); handleComplete(); }}
        onDismiss={() => { setDrawerOpen(false); handleDismiss(); }}
        onSnooze={() => { setDrawerOpen(false); handleSnooze(); }}
        isActionLoading={isActionLoading}
      />
    </>
  );
}
