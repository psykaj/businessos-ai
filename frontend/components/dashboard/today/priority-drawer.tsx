import { useState } from "react";
import { format } from "date-fns";
import { 
  CheckCircle2, 
  X, 
  Clock, 
  ArrowRight, 
  Target, 
  TrendingUp, 
  Zap,
  Activity,
  AlertTriangle,
  Info
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { 
  Sheet, 
  SheetContent, 
  SheetHeader, 
  SheetTitle,
  SheetDescription,
  SheetFooter
} from "@/components/ui/sheet";
import { DailyPriorityDto } from "@/lib/daily-operating-loop-service";

interface PriorityDrawerProps {
  priority: DailyPriorityDto;
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onComplete: () => void;
  onDismiss: () => void;
  onSnooze: () => void;
  isActionLoading?: boolean;
}

export function PriorityDrawer({ 
  priority, 
  open, 
  onOpenChange, 
  onComplete, 
  onDismiss, 
  onSnooze,
  isActionLoading
}: PriorityDrawerProps) {
  
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
    <Sheet open={open} onOpenChange={onOpenChange}>
      <SheetContent className="w-full sm:max-w-xl overflow-y-auto">
        <SheetHeader className="mb-6">
          <div className="flex items-center gap-2 mb-2">
            <Badge variant="outline" className="uppercase text-[10px] tracking-wider font-semibold">
              {priority.priorityType.replace(/([A-Z])/g, ' $1').trim()}
            </Badge>
            <Badge variant="secondary" className="bg-primary/10 text-primary hover:bg-primary/20">
              Score: {priority.priorityScore}
            </Badge>
          </div>
          <SheetTitle className="text-2xl font-semibold leading-tight">{priority.title}</SheetTitle>
          <SheetDescription className="text-base text-foreground mt-2">
            {priority.reason}
          </SheetDescription>
        </SheetHeader>

        <div className="flex flex-col gap-6 py-4">
          
          {/* Why it matters / Evidence */}
          <div className="space-y-3">
            <h4 className="text-sm font-semibold uppercase tracking-wider text-muted-foreground flex items-center gap-2">
              <Info className="w-4 h-4" /> Evidence & Context
            </h4>
            <div className="bg-muted/40 rounded-xl p-4 border border-border/50 text-sm leading-relaxed">
              {priority.description}
            </div>
          </div>

          {/* Expected Impact */}
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-3">
              <h4 className="text-sm font-semibold uppercase tracking-wider text-muted-foreground flex items-center gap-2">
                <TrendingUp className="w-4 h-4" /> Expected Impact
              </h4>
              <div className="bg-emerald-500/10 border border-emerald-500/20 rounded-xl p-4">
                <p className="text-sm font-medium text-emerald-700 dark:text-emerald-400">
                  {priority.expectedImpact}
                </p>
                <p className="text-xs text-emerald-600/70 dark:text-emerald-500/70 mt-1">
                  Type: {priority.impactType}
                </p>
              </div>
            </div>
            
            <div className="space-y-3">
              <h4 className="text-sm font-semibold uppercase tracking-wider text-muted-foreground flex items-center gap-2">
                <Target className="w-4 h-4" /> Confidence
              </h4>
              <div className="bg-muted/40 border border-border/50 rounded-xl p-4 flex flex-col justify-center h-[76px]">
                <div className="flex items-center justify-between mb-1.5">
                  <span className="text-sm font-medium">{priority.confidence}%</span>
                  <span className="text-xs text-muted-foreground">AI Certainty</span>
                </div>
                <Progress value={priority.confidence} className="h-1.5" />
              </div>
            </div>
          </div>

          {/* Recommended Action */}
          <div className="space-y-3">
            <h4 className="text-sm font-semibold uppercase tracking-wider text-muted-foreground flex items-center gap-2">
              <Zap className="w-4 h-4" /> Recommended Action
            </h4>
            <div className="bg-primary/5 border border-primary/20 rounded-xl p-4">
              <p className="text-sm font-medium text-foreground">
                {priority.suggestedAction}
              </p>
              <Button className="mt-4 w-full sm:w-auto" variant="default">
                {getActionLabel(priority.priorityType)} <ArrowRight className="w-4 h-4 ml-2" />
              </Button>
            </div>
          </div>

        </div>

        <SheetFooter className="mt-8 flex-col sm:flex-row gap-3 sm:gap-2">
          <Button variant="outline" onClick={onSnooze} disabled={isActionLoading} className="w-full sm:w-auto">
            <Clock className="w-4 h-4 mr-2" /> Snooze
          </Button>
          <Button variant="outline" onClick={onDismiss} disabled={isActionLoading} className="w-full sm:w-auto text-destructive hover:text-destructive hover:bg-destructive/10">
            <X className="w-4 h-4 mr-2" /> Dismiss
          </Button>
          <Button variant="default" onClick={onComplete} disabled={isActionLoading} className="w-full sm:w-auto bg-emerald-600 hover:bg-emerald-700 text-white">
            <CheckCircle2 className="w-4 h-4 mr-2" /> Mark Complete
          </Button>
        </SheetFooter>
      </SheetContent>
    </Sheet>
  );
}
