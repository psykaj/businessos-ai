"use client";

import React, { useState } from "react";
import { useOutcomes, useRoiSummary } from "@/hooks/use-outcomes";
import { CommandCenterBusinessValue } from "@/components/dashboard/command-center/command-center-business-value";
import { Button } from "@/components/ui/button";
import { Plus, Filter, AlertCircle } from "lucide-react";
import { format } from "date-fns";
import { RecordOutcomeDialog } from "@/components/dashboard/outcomes/record-outcome-dialog";
import { OutcomeDetailsSheet } from "@/components/dashboard/outcomes/outcome-details-sheet";
import { OutcomeBadge } from "@/components/dashboard/outcomes/outcome-badge";
import { BusinessOutcome } from "@/lib/outcomes-service";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Skeleton } from "@/components/ui/skeleton";
import { Alert, AlertTitle, AlertDescription } from "@/components/ui/alert";

export default function BusinessOutcomesPage() {
  const [isRecordOpen, setIsRecordOpen] = useState(false);
  const [selectedOutcome, setSelectedOutcome] = useState<BusinessOutcome | null>(null);
  
  // Filters
  const [sourceType, setSourceType] = useState<string>("all");
  
  const { data: outcomes, isLoading: isLoadingOutcomes, isError: isErrorOutcomes } = useOutcomes({
    sourceType: sourceType !== "all" ? sourceType : undefined,
    pageSize: 50,
  });

  return (
    <div className="flex flex-col gap-8 w-full max-w-7xl mx-auto pb-10">
      {/* Header */}
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight text-foreground">
            Business Outcomes
          </h1>
          <p className="mt-1 text-sm text-muted-foreground">
            See what happened after BusinessOS AI recommendations and automations.
          </p>
        </div>
        <div className="flex items-center gap-2">
          <Button onClick={() => setIsRecordOpen(true)} className="h-9">
            <Plus className="w-4 h-4 mr-2" />
            Record Outcome
          </Button>
        </div>
      </div>

      {isErrorOutcomes && (
        <Alert variant="destructive">
          <AlertCircle className="h-4 w-4" />
          <AlertTitle>Data Unavailable</AlertTitle>
          <AlertDescription>
            Business outcome data is temporarily unavailable. Please try again later.
          </AlertDescription>
        </Alert>
      )}

      {/* Top Summary using the Command Center component for consistency */}
      <CommandCenterBusinessValue isLoading={isLoadingOutcomes} />

      {/* Main Outcomes List / Timeline */}
      <div className="space-y-4">
        <div className="flex justify-between items-center">
          <h2 className="text-lg font-medium">Outcome Timeline</h2>
          <div className="flex items-center gap-2">
            <Select value={sourceType} onValueChange={(val) => setSourceType(val || 'all')}>
              <SelectTrigger className="w-[180px] h-9">
                <Filter className="w-4 h-4 mr-2 text-muted-foreground" />
                <SelectValue placeholder="All Sources" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="all">All Sources</SelectItem>
                <SelectItem value="ActionCenter">Action Center</SelectItem>
                <SelectItem value="Automation">Automations</SelectItem>
                <SelectItem value="AiRecommendation">AI Recommendations</SelectItem>
                <SelectItem value="UserReported">User Reported</SelectItem>
              </SelectContent>
            </Select>
          </div>
        </div>

        <div className="bg-background rounded-xl border shadow-sm overflow-hidden">
          {isLoadingOutcomes ? (
            <div className="p-8 space-y-4">
              {[1, 2, 3].map((i) => (
                <div key={i} className="flex gap-4 items-center">
                  <Skeleton className="h-12 w-12 rounded-full" />
                  <div className="space-y-2">
                    <Skeleton className="h-4 w-64" />
                    <Skeleton className="h-3 w-32" />
                  </div>
                </div>
              ))}
            </div>
          ) : !outcomes || outcomes.length === 0 ? (
            <div className="py-20 text-center px-4 flex flex-col items-center justify-center">
              <div className="w-16 h-16 bg-muted/50 rounded-full flex items-center justify-center mb-4">
                <AlertCircle className="w-8 h-8 text-muted-foreground" />
              </div>
              <h3 className="text-lg font-medium">No Measurable Outcomes Yet</h3>
              <p className="text-sm text-muted-foreground max-w-sm mt-1">
                Your BusinessOS AI impact report will appear here as actions and automations produce measurable outcomes.
              </p>
            </div>
          ) : (
            <div className="divide-y">
              {outcomes.map((outcome) => (
                <div 
                  key={outcome.id} 
                  className="p-4 flex flex-col sm:flex-row gap-4 items-start sm:items-center hover:bg-muted/30 transition-colors cursor-pointer"
                  onClick={() => setSelectedOutcome(outcome)}
                >
                  <div className="flex-1">
                    <div className="flex items-center gap-2 mb-1">
                      <span className="font-medium text-foreground">
                        {outcome.outcomeType.replace(/([A-Z])/g, ' $1').trim()}
                      </span>
                      <OutcomeBadge 
                        confidence={outcome.confidence} 
                        attributionLevel={outcome.attributionLevel} 
                        sourceType={outcome.sourceType}
                      />
                    </div>
                    <div className="flex items-center gap-2 text-sm text-muted-foreground">
                      <span>Source: {outcome.sourceType.replace(/([A-Z])/g, ' $1').trim()}</span>
                      <span>•</span>
                      <span>{format(new Date(outcome.occurredAt), "MMM d, yyyy")}</span>
                    </div>
                  </div>
                  
                  <div className="flex flex-col items-end min-w-[120px]">
                    {outcome.revenueImpact && (
                      <span className="font-semibold text-green-600">
                        {new Intl.NumberFormat('en-IN', { style: 'currency', currency: 'INR', maximumFractionDigits: 0 }).format(outcome.revenueImpact)}
                      </span>
                    )}
                    {outcome.costImpact && (
                      <span className="font-semibold text-emerald-600">
                        {new Intl.NumberFormat('en-IN', { style: 'currency', currency: 'INR', maximumFractionDigits: 0 }).format(outcome.costImpact)}
                      </span>
                    )}
                    {outcome.timeSavedMinutes && (
                      <span className="font-medium">
                        {(outcome.timeSavedMinutes / 60).toFixed(1)} hrs saved
                      </span>
                    )}
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>

      <RecordOutcomeDialog 
        open={isRecordOpen} 
        onOpenChange={setIsRecordOpen} 
      />

      <OutcomeDetailsSheet 
        outcome={selectedOutcome}
        open={!!selectedOutcome}
        onOpenChange={(open) => {
          if (!open) setSelectedOutcome(null);
        }}
      />
    </div>
  );
}
