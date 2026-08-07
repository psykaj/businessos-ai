"use client";

import { AiActionDto } from "@/types/action-center";
import { AiActionCard } from "./AiActionCard";
import { motion, AnimatePresence } from "framer-motion";
import { Skeleton } from "@/components/ui/skeleton";

interface AiActionListProps {
  actions: AiActionDto[];
  isLoading: boolean;
  onApprove: (id: string) => void;
  onReject: (id: string) => void;
  onExecute: (id: string) => void;
  onViewDetails: (action: AiActionDto) => void;
  processingIds: Set<string>;
}

export function AiActionList({
  actions,
  isLoading,
  onApprove,
  onReject,
  onExecute,
  onViewDetails,
  processingIds,
}: AiActionListProps) {
  if (isLoading) {
    return (
      <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
        {[1, 2, 3, 4, 5, 6].map((i) => (
          <div key={i} className="h-[220px] rounded-xl border bg-card text-card-foreground shadow">
            <div className="p-5">
              <Skeleton className="h-5 w-24 mb-3" />
              <Skeleton className="h-6 w-3/4 mb-4" />
              <Skeleton className="h-4 w-full mb-2" />
              <Skeleton className="h-4 w-2/3" />
            </div>
          </div>
        ))}
      </div>
    );
  }

  if (actions.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-16 px-4 text-center bg-muted/20 border border-dashed rounded-xl">
        <div className="bg-muted p-4 rounded-full mb-4">
          <svg className="w-8 h-8 text-muted-foreground" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
          </svg>
        </div>
        <h3 className="text-xl font-semibold mb-2">No actions found</h3>
        <p className="text-muted-foreground max-w-md mx-auto">
          You have no pending AI actions at the moment. Check back later when the AI engine identifies new opportunities.
        </p>
      </div>
    );
  }

  return (
    <motion.div 
      className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4"
      layout
    >
      <AnimatePresence mode="popLayout">
        {actions.map((action) => (
          <motion.div
            key={action.id}
            layout
            initial={{ opacity: 0, scale: 0.9 }}
            animate={{ opacity: 1, scale: 1 }}
            exit={{ opacity: 0, scale: 0.9 }}
            transition={{ duration: 0.2 }}
          >
            <AiActionCard
              action={action}
              onApprove={onApprove}
              onReject={onReject}
              onExecute={onExecute}
              onViewDetails={onViewDetails}
              isProcessing={processingIds.has(action.id)}
            />
          </motion.div>
        ))}
      </AnimatePresence>
    </motion.div>
  );
}
