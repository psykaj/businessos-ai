import { ProactiveAlertDto, AiActionDto } from "@/lib/command-center-service";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button, buttonVariants } from "@/components/ui/button";
import { AlertCircle, ArrowRight, Zap } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";
import Link from "next/link";

interface CommandCenterNeedsAttentionProps {
  alerts?: ProactiveAlertDto[];
  actions?: AiActionDto[];
  isLoading: boolean;
}

export function CommandCenterNeedsAttention({ alerts = [], actions = [], isLoading }: CommandCenterNeedsAttentionProps) {
  if (isLoading) {
    return (
      <Card className="h-full">
        <CardHeader>
          <CardTitle className="flex items-center"><AlertCircle className="w-5 h-5 mr-2 text-red-500"/> Needs Attention</CardTitle>
          <CardDescription>Top priorities that require your action.</CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          {[1, 2, 3].map((i) => (
            <div key={i} className="flex flex-col gap-2 p-3 border rounded-lg">
              <Skeleton className="h-4 w-3/4" />
              <Skeleton className="h-3 w-full" />
              <div className="flex justify-between mt-2">
                <Skeleton className="h-5 w-16" />
                <Skeleton className="h-8 w-24" />
              </div>
            </div>
          ))}
        </CardContent>
      </Card>
    );
  }

  const hasItems = alerts.length > 0 || actions.length > 0;

  if (!hasItems) {
    return (
      <Card className="h-full">
        <CardHeader>
          <CardTitle className="flex items-center"><AlertCircle className="w-5 h-5 mr-2 text-muted-foreground"/> Needs Attention</CardTitle>
          <CardDescription>Top priorities that require your action.</CardDescription>
        </CardHeader>
        <CardContent className="flex flex-col items-center justify-center py-8 text-center">
          <div className="w-12 h-12 rounded-full bg-green-100 dark:bg-green-900/20 flex items-center justify-center mb-4">
            <Zap className="h-6 w-6 text-green-600 dark:text-green-400" />
          </div>
          <p className="text-sm font-medium">You're all caught up!</p>
          <p className="text-xs text-muted-foreground mt-1">No critical issues require immediate attention.</p>
        </CardContent>
      </Card>
    );
  }

  // Combine and sort
  const combined: Array<{type: 'alert' | 'action', id: string, title: string, desc: string, priority: string, cta: string, link: string}> = [];
  
  alerts.forEach(a => {
    let link = "/dashboard/alerts";
    if (a.Category === "Finance") link = "/dashboard/finance";
    else if (a.Category === "Inventory") link = "/dashboard/inventory";
    else if (a.Category === "CRM") link = "/dashboard/crm";
    
    combined.push({
      type: 'alert',
      id: a.Id,
      title: a.Title,
      desc: a.Description,
      priority: a.Priority,
      cta: "View Details",
      link
    });
  });

  actions.forEach(a => {
    combined.push({
      type: 'action',
      id: a.Id,
      title: a.Title,
      desc: a.Description,
      priority: a.Priority,
      cta: "Take Action",
      link: "/dashboard/action-center"
    });
  });

  // Sort by High -> Medium -> Low
  const priorityScore = (p: string) => p === "High" ? 3 : p === "Medium" ? 2 : 1;
  combined.sort((a, b) => priorityScore(b.priority) - priorityScore(a.priority));

  // Take top 4
  const topItems = combined.slice(0, 4);

  return (
    <Card className="h-full border-red-200 dark:border-red-900/50">
      <CardHeader className="bg-red-50 dark:bg-red-950/20 border-b pb-4">
        <CardTitle className="flex items-center text-red-700 dark:text-red-400">
          <AlertCircle className="w-5 h-5 mr-2" />
          Needs Attention
        </CardTitle>
        <CardDescription className="text-red-600/80 dark:text-red-400/80">
          Top {topItems.length} issues requiring immediate action.
        </CardDescription>
      </CardHeader>
      <CardContent className="p-0">
        <div className="divide-y">
          {topItems.map((item) => (
            <div key={item.id} className="p-4 hover:bg-muted/50 transition-colors">
              <div className="flex items-start justify-between gap-4">
                <div className="space-y-1 flex-1">
                  <div className="flex items-center gap-2">
                    <h4 className="text-sm font-semibold">{item.title}</h4>
                    {item.priority === "High" && (
                      <Badge variant="destructive" className="text-[10px] px-1.5 py-0 h-4">High</Badge>
                    )}
                  </div>
                  <p className="text-sm text-muted-foreground line-clamp-2">{item.desc}</p>
                </div>
                <Link 
                  href={item.link} 
                  className={buttonVariants({ size: "sm", variant: item.type === 'action' ? "default" : "outline", className: "shrink-0 mt-1" })}
                >
                  {item.cta}
                  {item.type === 'action' && <ArrowRight className="w-3 h-3 ml-1.5" />}
                </Link>
              </div>
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  );
}
