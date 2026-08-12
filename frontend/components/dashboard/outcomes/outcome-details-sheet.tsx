import React from "react";
import { Sheet, SheetContent, SheetHeader, SheetTitle, SheetDescription } from "@/components/ui/sheet";
import { BusinessOutcome } from "@/lib/outcomes-service";
import { OutcomeBadge } from "./outcome-badge";
import { format } from "date-fns";
import { Separator } from "@/components/ui/separator";

interface OutcomeDetailsSheetProps {
  outcome: BusinessOutcome | null;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function OutcomeDetailsSheet({ outcome, open, onOpenChange }: OutcomeDetailsSheetProps) {
  if (!outcome) return null;

  return (
    <Sheet open={open} onOpenChange={onOpenChange}>
      <SheetContent className="sm:max-w-md overflow-y-auto">
        <SheetHeader className="mb-6">
          <SheetTitle>Outcome Details</SheetTitle>
          <SheetDescription>
            Recorded on {format(new Date(outcome.recordedAt), "MMM d, yyyy 'at' h:mm a")}
          </SheetDescription>
        </SheetHeader>

        <div className="space-y-6">
          {/* Top Info */}
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-muted-foreground">Type</p>
              <p className="text-base font-semibold">{outcome.outcomeType.replace(/([A-Z])/g, ' $1').trim()}</p>
            </div>
            <div className="text-right">
              <OutcomeBadge 
                confidence={outcome.confidence} 
                attributionLevel={outcome.attributionLevel} 
                sourceType={outcome.sourceType}
              />
            </div>
          </div>

          <Separator />

          {/* Impact section */}
          <div className="grid grid-cols-2 gap-4">
            {outcome.revenueImpact ? (
              <div>
                <p className="text-sm font-medium text-muted-foreground">Revenue Impact</p>
                <p className="text-lg font-bold text-green-600">
                  {new Intl.NumberFormat('en-IN', { style: 'currency', currency: 'INR', maximumFractionDigits: 0 }).format(outcome.revenueImpact)}
                </p>
              </div>
            ) : null}

            {outcome.costImpact ? (
              <div>
                <p className="text-sm font-medium text-muted-foreground">Cost Saved</p>
                <p className="text-lg font-bold text-emerald-600">
                  {new Intl.NumberFormat('en-IN', { style: 'currency', currency: 'INR', maximumFractionDigits: 0 }).format(outcome.costImpact)}
                </p>
              </div>
            ) : null}

            {outcome.timeSavedMinutes ? (
              <div>
                <p className="text-sm font-medium text-muted-foreground">Time Saved</p>
                <p className="text-lg font-bold">
                  {(outcome.timeSavedMinutes / 60).toFixed(1)} hours
                </p>
              </div>
            ) : null}
          </div>

          {outcome.revenueImpact || outcome.costImpact || outcome.timeSavedMinutes ? <Separator /> : null}

          {/* Explanation */}
          <div>
            <h4 className="text-sm font-medium text-muted-foreground mb-2">What happened?</h4>
            <div className="bg-muted/50 p-4 rounded-lg text-sm">
              {outcome.explanation || "No detailed explanation provided."}
            </div>
          </div>

          {/* Source & Timeline */}
          <div>
            <h4 className="text-sm font-medium text-muted-foreground mb-4">Timeline & Attribution</h4>
            
            <div className="relative border-l-2 border-muted ml-3 space-y-6">
              <div className="relative pl-6">
                <div className="absolute w-3 h-3 bg-muted-foreground rounded-full -left-[7px] top-1.5 ring-4 ring-background" />
                <p className="text-sm font-medium">Source Event</p>
                <p className="text-xs text-muted-foreground">
                  {outcome.sourceType} ({outcome.sourceId})
                </p>
              </div>

              <div className="relative pl-6">
                <div className="absolute w-3 h-3 bg-primary rounded-full -left-[7px] top-1.5 ring-4 ring-background" />
                <p className="text-sm font-medium">Outcome Measured</p>
                <p className="text-xs text-muted-foreground">
                  {format(new Date(outcome.occurredAt), "MMM d, yyyy")}
                </p>
              </div>
            </div>
          </div>
          
          {/* AI Attribution Note */}
          {(outcome.attributionLevel && outcome.attributionLevel !== "Unknown") && (
            <div className="bg-blue-50 text-blue-900 p-4 rounded-lg text-sm flex gap-3 items-start mt-6">
              <div className="mt-0.5">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><circle cx="12" cy="12" r="10"/><path d="M12 16v-4"/><path d="M12 8h.01"/></svg>
              </div>
              <p>
                BusinessOS AI classifies this attribution as <strong>{outcome.attributionLevel}</strong> because evidence links the source event directly to the measurable outcome.
              </p>
            </div>
          )}

        </div>
      </SheetContent>
    </Sheet>
  );
}
