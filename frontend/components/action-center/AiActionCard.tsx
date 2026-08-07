"use client";

import { AiActionDto } from "@/types/action-center";
import { Card, CardContent, CardFooter, CardHeader } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { motion } from "framer-motion";
import { Check, X, Zap, ChevronRight, AlertTriangle } from "lucide-react";
import { Tooltip, TooltipContent, TooltipProvider, TooltipTrigger } from "@/components/ui/tooltip";

interface AiActionCardProps {
  action: AiActionDto;
  onApprove: (id: string) => void;
  onReject: (id: string) => void;
  onExecute: (id: string) => void;
  onViewDetails: (action: AiActionDto) => void;
  isProcessing: boolean;
}

export function AiActionCard({
  action,
  onApprove,
  onReject,
  onExecute,
  onViewDetails,
  isProcessing,
}: AiActionCardProps) {
  const getPriorityColor = (priority: string) => {
    switch (priority) {
      case "Critical":
        return "bg-rose-500 hover:bg-rose-600";
      case "High":
        return "bg-orange-500 hover:bg-orange-600";
      case "Medium":
        return "bg-blue-500 hover:bg-blue-600";
      case "Low":
        return "bg-slate-500 hover:bg-slate-600";
      default:
        return "bg-slate-500";
    }
  };

  const getStatusBadge = (status: string) => {
    switch (status) {
      case "Approved":
        return <Badge variant="default">Approved</Badge>;
      case "Executed":
        return <Badge variant="secondary" className="bg-emerald-500/15 text-emerald-600 hover:bg-emerald-500/25">Executed</Badge>;
      case "Failed":
      case "Rejected":
        return <Badge variant="destructive">{status}</Badge>;
      default:
        return <Badge variant="outline">Pending</Badge>;
    }
  };

  const canApproveOrReject = action.status === "Pending";
  const canExecute = 
    action.status === "Approved" || 
    (action.status === "Pending" && action.riskLevel === "Low");

  return (
    <Card className="flex flex-col h-full hover:shadow-md transition-shadow group overflow-hidden relative">
      <div className={`absolute top-0 left-0 w-1 h-full ${getPriorityColor(action.priority)}`} />
      
      <CardHeader className="pb-3 px-5 pt-5">
        <div className="flex justify-between items-start mb-2">
          <Badge className={`${getPriorityColor(action.priority)} text-white border-0`}>
            {action.priority} Priority
          </Badge>
          {getStatusBadge(action.status)}
        </div>
        <h3 className="font-semibold text-lg leading-tight line-clamp-2">
          {action.title}
        </h3>
      </CardHeader>
      
      <CardContent className="px-5 py-0 flex-grow">
        <p className="text-sm text-muted-foreground line-clamp-2 mb-4">
          {action.description}
        </p>
        
        <div className="flex flex-wrap gap-2 text-xs mb-2">
          {action.estimatedRevenueIncrease > 0 && (
            <div className="bg-emerald-50 dark:bg-emerald-950/30 text-emerald-600 dark:text-emerald-400 px-2 py-1 rounded-md font-medium">
              +${action.estimatedRevenueIncrease.toLocaleString()} Rev
            </div>
          )}
          {action.estimatedCostSaving > 0 && (
            <div className="bg-blue-50 dark:bg-blue-950/30 text-blue-600 dark:text-blue-400 px-2 py-1 rounded-md font-medium">
              +${action.estimatedCostSaving.toLocaleString()} Saved
            </div>
          )}
          {action.riskLevel === "High" && (
            <div className="bg-amber-50 dark:bg-amber-950/30 text-amber-600 dark:text-amber-400 px-2 py-1 rounded-md font-medium flex items-center gap-1">
              <AlertTriangle className="w-3 h-3" /> High Risk
            </div>
          )}
        </div>
      </CardContent>
      
      <CardFooter className="px-5 pt-4 pb-5 flex gap-2 border-t mt-4 bg-muted/20">
        {canApproveOrReject && (
          <>
            <TooltipProvider>
              <Tooltip>
                <TooltipTrigger render={
                  <Button
                    variant="outline"
                    size="icon"
                    className="h-9 w-9 text-emerald-600 hover:text-emerald-700 hover:bg-emerald-50 dark:hover:bg-emerald-950/50 border-emerald-200 dark:border-emerald-900"
                    onClick={() => onApprove(action.id)}
                    disabled={isProcessing}
                  >
                    <Check className="h-4 w-4" />
                  </Button>
                } />
                <TooltipContent>Approve Action</TooltipContent>
              </Tooltip>
            </TooltipProvider>

            <TooltipProvider>
              <Tooltip>
                <TooltipTrigger render={
                  <Button
                    variant="outline"
                    size="icon"
                    className="h-9 w-9 text-rose-600 hover:text-rose-700 hover:bg-rose-50 dark:hover:bg-rose-950/50 border-rose-200 dark:border-rose-900"
                    onClick={() => onReject(action.id)}
                    disabled={isProcessing}
                  >
                    <X className="h-4 w-4" />
                  </Button>
                } />
                <TooltipContent>Reject Action</TooltipContent>
              </Tooltip>
            </TooltipProvider>
          </>
        )}

        {canExecute && (
          <Button 
            className="flex-1 h-9 bg-indigo-600 hover:bg-indigo-700 text-white"
            onClick={() => onExecute(action.id)}
            disabled={isProcessing}
          >
            <Zap className="h-4 w-4 mr-2" />
            Execute Now
          </Button>
        )}

        {action.status === "Executed" && (
          <Button 
            variant="secondary"
            className="flex-1 h-9"
            disabled
          >
            <Check className="h-4 w-4 mr-2" />
            Completed
          </Button>
        )}
        
        <Button 
          variant="ghost" 
          size="icon" 
          className="h-9 w-9 ml-auto"
          onClick={() => onViewDetails(action)}
        >
          <ChevronRight className="h-4 w-4" />
        </Button>
      </CardFooter>
    </Card>
  );
}
