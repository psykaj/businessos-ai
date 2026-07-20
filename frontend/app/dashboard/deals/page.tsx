"use client";

import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { crmService } from "@/lib/crm-service";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { Plus, GripVertical } from "lucide-react";
import Link from "next/link";
import { formatCurrency, cn } from "@/lib/utils";
import { Deal, PipelineStage } from "@/types/crm";

const COLUMNS = [
  { id: PipelineStage.NewLead, title: "New Lead", color: "border-blue-200 bg-blue-50/50" },
  { id: PipelineStage.Contacted, title: "Contacted", color: "border-yellow-200 bg-yellow-50/50" },
  { id: PipelineStage.Qualified, title: "Qualified", color: "border-purple-200 bg-purple-50/50" },
  { id: PipelineStage.ProposalSent, title: "Proposal Sent", color: "border-orange-200 bg-orange-50/50" },
  { id: PipelineStage.Negotiation, title: "Negotiation", color: "border-pink-200 bg-pink-50/50" },
  { id: PipelineStage.Won, title: "Won", color: "border-green-200 bg-green-50/50" },
  { id: PipelineStage.Lost, title: "Lost", color: "border-red-200 bg-red-50/50" },
];

export default function DealsKanbanPage() {
  const queryClient = useQueryClient();
  const [draggedDealId, setDraggedDealId] = useState<string | null>(null);
  
  const { data: deals, isLoading } = useQuery({
    queryKey: ['crm-deals'],
    queryFn: () => crmService.getDeals()
  });

  const updateStageMutation = useMutation({
    mutationFn: ({ id, stage }: { id: string, stage: PipelineStage }) => crmService.updateDealStage(id, stage),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['crm-deals'] });
    }
  });

  const handleDragStart = (e: React.DragEvent, dealId: string) => {
    setDraggedDealId(dealId);
    e.dataTransfer.setData("dealId", dealId);
    e.dataTransfer.effectAllowed = "move";
  };

  const handleDragOver = (e: React.DragEvent) => {
    e.preventDefault();
    e.dataTransfer.dropEffect = "move";
  };

  const handleDrop = (e: React.DragEvent, targetStage: PipelineStage) => {
    e.preventDefault();
    const dealId = e.dataTransfer.getData("dealId");
    
    if (dealId) {
      // Optimistic update
      queryClient.setQueryData(['crm-deals'], (oldDeals: Deal[] | undefined) => {
        if (!oldDeals) return [];
        return oldDeals.map(d => d.id === dealId ? { ...d, pipelineStage: targetStage } : d);
      });
      
      updateStageMutation.mutate({ id: dealId, stage: targetStage });
    }
    setDraggedDealId(null);
  };

  return (
    <div className="p-8 h-full flex flex-col space-y-6">
      <div className="flex justify-between items-center shrink-0">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Sales Pipeline</h1>
          <p className="text-muted-foreground">Manage deals through their lifecycle stages.</p>
        </div>
        <Button>
          <Plus className="mr-2 h-4 w-4" /> Add Deal
        </Button>
      </div>

      <div className="flex-1 overflow-x-auto pb-4">
        <div className="flex gap-4 h-full min-w-max items-start">
          {COLUMNS.map(column => {
            const columnDeals = deals?.filter(d => d.pipelineStage === column.id) || [];
            
            return (
              <div 
                key={column.id} 
                className={cn("flex flex-col w-[320px] rounded-xl border p-3 h-[calc(100vh-220px)]", column.color)}
                onDragOver={handleDragOver}
                onDrop={(e) => handleDrop(e, column.id)}
              >
                <div className="flex justify-between items-center mb-4 px-1">
                  <h3 className="font-semibold">{column.title}</h3>
                  <span className="text-xs bg-background rounded-full px-2 py-0.5 border shadow-sm">
                    {columnDeals.length}
                  </span>
                </div>
                
                <div className="flex-1 overflow-y-auto space-y-3 pr-2 scrollbar-hide">
                  {isLoading ? (
                    Array.from({ length: 3 }).map((_, i) => (
                      <Skeleton key={i} className="h-28 w-full rounded-lg" />
                    ))
                  ) : columnDeals.length > 0 ? (
                    columnDeals.map(deal => (
                      <Card 
                        key={deal.id}
                        draggable
                        onDragStart={(e) => handleDragStart(e, deal.id)}
                        className={cn(
                          "cursor-grab active:cursor-grabbing hover:border-primary/50 transition-colors",
                          draggedDealId === deal.id ? "opacity-50" : "opacity-100"
                        )}
                      >
                        <CardContent className="p-4 flex flex-col gap-3">
                          <div className="flex justify-between items-start">
                            <Link href={`/dashboard/deals/${deal.id}`} className="font-semibold hover:underline">
                              {deal.title}
                            </Link>
                            <GripVertical className="h-4 w-4 text-muted-foreground/50 shrink-0" />
                          </div>
                          
                          <div className="text-sm font-medium text-green-600 bg-green-50 w-fit px-2 py-0.5 rounded-md">
                            {formatCurrency(deal.amount)}
                          </div>
                          
                          <div className="flex justify-between items-center text-xs text-muted-foreground mt-2">
                            <span>{deal.probability}% Prob.</span>
                            {deal.expectedCloseDate && (
                              <span>Close: {new Date(deal.expectedCloseDate).toLocaleDateString()}</span>
                            )}
                          </div>
                        </CardContent>
                      </Card>
                    ))
                  ) : (
                    <div className="h-24 border-2 border-dashed border-background rounded-lg flex items-center justify-center text-muted-foreground/50 text-sm">
                      Drop deals here
                    </div>
                  )}
                </div>
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
}
