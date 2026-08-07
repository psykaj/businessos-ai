"use client";

import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
} from "@/components/ui/sheet";
import { AiActionDto } from "@/types/action-center";
import { Badge } from "@/components/ui/badge";
import { Separator } from "@/components/ui/separator";
import { format } from "date-fns";

interface ActionDetailsDrawerProps {
  action: AiActionDto | null;
  isOpen: boolean;
  onClose: () => void;
}

export function ActionDetailsDrawer({
  action,
  isOpen,
  onClose,
}: ActionDetailsDrawerProps) {
  if (!action) return null;

  return (
    <Sheet open={isOpen} onOpenChange={onClose}>
      <SheetContent className="w-[400px] sm:w-[540px] overflow-y-auto">
        <SheetHeader className="mb-6">
          <div className="flex items-center justify-between">
            <Badge variant="outline">{action.category}</Badge>
            <Badge
              variant={
                action.status === "Approved"
                  ? "default"
                  : action.status === "Executed"
                  ? "secondary"
                  : action.status === "Failed"
                  ? "destructive"
                  : "outline"
              }
            >
              {action.status}
            </Badge>
          </div>
          <SheetTitle className="text-2xl mt-4">{action.title}</SheetTitle>
          <SheetDescription>
            Generated on {format(new Date(action.createdAt), "PPP")}
          </SheetDescription>
        </SheetHeader>

        <div className="space-y-6">
          {/* Recommendation & Reason */}
          <div>
            <h3 className="font-semibold mb-2">Recommendation & Reason</h3>
            <p className="text-sm text-muted-foreground leading-relaxed">
              {action.description}
            </p>
          </div>

          <Separator />

          {/* Expected Impact */}
          <div>
            <h3 className="font-semibold mb-2">Expected Impact</h3>
            <div className="grid grid-cols-2 gap-4 text-sm">
              <div className="bg-muted/50 p-3 rounded-md">
                <span className="block text-muted-foreground mb-1">
                  Business Impact
                </span>
                <span className="font-medium">{action.businessImpact}</span>
              </div>
              <div className="bg-muted/50 p-3 rounded-md">
                <span className="block text-muted-foreground mb-1">
                  Risk Level
                </span>
                <span className="font-medium">{action.riskLevel}</span>
              </div>
              <div className="bg-muted/50 p-3 rounded-md">
                <span className="block text-muted-foreground mb-1">
                  Est. Revenue
                </span>
                <span className="font-medium text-emerald-600 dark:text-emerald-400">
                  +${action.estimatedRevenueIncrease.toLocaleString()}
                </span>
              </div>
              <div className="bg-muted/50 p-3 rounded-md">
                <span className="block text-muted-foreground mb-1">
                  Est. Savings
                </span>
                <span className="font-medium text-blue-600 dark:text-blue-400">
                  +${action.estimatedCostSaving.toLocaleString()}
                </span>
              </div>
            </div>
          </div>

          <Separator />

          {/* Execution Log */}
          <div>
            <h3 className="font-semibold mb-2">Activity Log</h3>
            <div className="space-y-3 text-sm">
              <div className="flex gap-2">
                <div className="min-w-[100px] text-muted-foreground">
                  {format(new Date(action.createdAt), "MMM d, h:mm a")}
                </div>
                <div>Action Identified</div>
              </div>
              {action.approvedBy && (
                <div className="flex gap-2">
                  <div className="min-w-[100px] text-muted-foreground">
                    {/* Mock approval time for demo purposes since we don't have ApprovedAt */}
                    {format(new Date(action.createdAt), "MMM d, h:mm a")}
                  </div>
                  <div>Approved by User</div>
                </div>
              )}
              {action.executedDate && (
                <div className="flex gap-2">
                  <div className="min-w-[100px] text-muted-foreground">
                    {format(new Date(action.executedDate), "MMM d, h:mm a")}
                  </div>
                  <div>
                    Executed with result:{" "}
                    <span className="font-medium">
                      {action.executionResult || "Success"}
                    </span>
                  </div>
                </div>
              )}
            </div>
          </div>
        </div>
      </SheetContent>
    </Sheet>
  );
}
