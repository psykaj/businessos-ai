"use client";

import { CopilotContextResponseDto } from "@/types/memory";
import { format } from "date-fns";
import { 
  X, 
  BrainCircuit, 
  Target, 
  Clock, 
  Zap,
  TrendingUp,
  Link as LinkIcon
} from "lucide-react";
import { Badge } from "@/components/ui/badge";

interface BusinessContextDrawerProps {
  isOpen: boolean;
  onClose: () => void;
  contextData: CopilotContextResponseDto | null;
}

export function BusinessContextDrawer({ isOpen, onClose, contextData }: BusinessContextDrawerProps) {
  if (!isOpen) return null;

  return (
    <>
      <div 
        className="fixed inset-0 z-40 bg-background/80 backdrop-blur-sm"
        onClick={onClose}
      />
      <div className="fixed inset-y-0 right-0 z-50 w-full max-w-md border-l border-border bg-card shadow-2xl transition-transform animate-in slide-in-from-right duration-300 flex flex-col">
        <div className="flex items-center justify-between border-b border-border p-4">
          <div className="flex items-center gap-2 text-indigo-600 dark:text-indigo-400">
            <BrainCircuit className="h-5 w-5" />
            <h2 className="text-lg font-semibold text-foreground">AI Business Context</h2>
          </div>
          <button 
            onClick={onClose}
            className="rounded-lg p-2 text-muted-foreground hover:bg-muted transition-colors"
          >
            <X className="h-5 w-5" />
          </button>
        </div>

        <div className="flex-1 overflow-y-auto p-4 space-y-6">
          {!contextData ? (
            <div className="text-center py-10 text-muted-foreground text-sm">
              No context available for this response.
            </div>
          ) : (
            <>
              {/* Internal Analysis */}
              <div>
                <h3 className="text-xs font-bold uppercase tracking-wider text-muted-foreground mb-3 flex items-center gap-2">
                  <Target className="h-4 w-4" /> Why this response?
                </h3>
                <div className="rounded-xl border border-indigo-500/20 bg-indigo-500/5 p-4 text-sm leading-relaxed text-foreground shadow-sm">
                  {contextData.relevantContext || "The AI used real-time business data combined with historical memory to formulate this action."}
                </div>
              </div>

              {/* Memories Used */}
              <div>
                <h3 className="text-xs font-bold uppercase tracking-wider text-muted-foreground mb-3 flex items-center gap-2">
                  <TrendingUp className="h-4 w-4" /> Evidence & Memory Used
                </h3>
                
                {contextData.relevantMemories?.length > 0 ? (
                  <div className="space-y-3">
                    {contextData.relevantMemories.map((memory) => (
                      <div key={memory.id} className="rounded-lg border border-border bg-card p-3 shadow-sm">
                        <div className="flex items-center justify-between mb-1">
                          <span className="font-medium text-sm text-foreground">{memory.title}</span>
                          <Badge variant="outline" className="text-[10px] h-5 px-1.5 font-mono">
                            {memory.confidence} Conf.
                          </Badge>
                        </div>
                        <p className="text-xs text-muted-foreground leading-relaxed mb-2">
                          {memory.content}
                        </p>
                        <div className="flex items-center justify-between text-[10px] text-muted-foreground">
                          <span className="flex items-center gap-1">
                            <Clock className="h-3 w-3" />
                            {format(new Date(memory.createdAt), "MMM d, yyyy")}
                          </span>
                          <span className="flex items-center gap-1">
                            <LinkIcon className="h-3 w-3" />
                            Source: {memory.sourceModule}
                          </span>
                        </div>
                      </div>
                    ))}
                  </div>
                ) : (
                  <div className="rounded-lg border border-dashed border-border p-4 text-center text-xs text-muted-foreground">
                    No historical business memories were used for this specific answer. Only real-time data was queried.
                  </div>
                )}
              </div>
              
              {/* Data Sources */}
              {contextData.suggestedSources?.length > 0 && (
                <div>
                  <h3 className="text-xs font-bold uppercase tracking-wider text-muted-foreground mb-3 flex items-center gap-2">
                    <Zap className="h-4 w-4" /> Active Data Sources
                  </h3>
                  <div className="flex flex-wrap gap-2">
                    {contextData.suggestedSources.map(source => (
                      <Badge key={source} variant="secondary" className="bg-muted">
                        {source}
                      </Badge>
                    ))}
                  </div>
                </div>
              )}
            </>
          )}
        </div>
      </div>
    </>
  );
}
