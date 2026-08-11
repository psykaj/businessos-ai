import { CommandOpportunityDto } from "@/lib/command-center-service";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button, buttonVariants } from "@/components/ui/button";
import { Lightbulb, ArrowRight, TrendingUp } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";
import Link from "next/link";

interface CommandCenterOpportunitiesProps {
  opportunities?: CommandOpportunityDto[];
  isLoading: boolean;
}

export function CommandCenterOpportunities({ opportunities = [], isLoading }: CommandCenterOpportunitiesProps) {
  if (isLoading) {
    return (
      <Card className="h-full">
        <CardHeader>
          <CardTitle className="flex items-center"><Lightbulb className="w-5 h-5 mr-2 text-amber-500"/> Opportunities</CardTitle>
          <CardDescription>Potential growth areas discovered by AI.</CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          {[1, 2].map((i) => (
            <div key={i} className="flex flex-col gap-2 p-3 border rounded-lg bg-muted/20">
              <Skeleton className="h-4 w-2/3" />
              <Skeleton className="h-3 w-full" />
              <div className="flex justify-between mt-2">
                <Skeleton className="h-5 w-20" />
                <Skeleton className="h-8 w-24" />
              </div>
            </div>
          ))}
        </CardContent>
      </Card>
    );
  }

  if (opportunities.length === 0) {
    return (
      <Card className="h-full">
        <CardHeader>
          <CardTitle className="flex items-center"><Lightbulb className="w-5 h-5 mr-2 text-muted-foreground"/> Opportunities</CardTitle>
          <CardDescription>Potential growth areas discovered by AI.</CardDescription>
        </CardHeader>
        <CardContent className="flex flex-col items-center justify-center py-8 text-center">
          <TrendingUp className="h-8 w-8 text-muted-foreground/50 mb-3" />
          <p className="text-sm font-medium">No new opportunities identified.</p>
          <p className="text-xs text-muted-foreground mt-1">Keep using BusinessOS; we'll surface insights here.</p>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card className="h-full border-amber-200 dark:border-amber-900/50">
      <CardHeader className="bg-amber-50 dark:bg-amber-950/20 border-b pb-4">
        <CardTitle className="flex items-center text-amber-700 dark:text-amber-400">
          <Lightbulb className="w-5 h-5 mr-2" />
          Opportunities
        </CardTitle>
        <CardDescription className="text-amber-700/80 dark:text-amber-400/80">
          Potential revenue and growth areas.
        </CardDescription>
      </CardHeader>
      <CardContent className="p-0">
        <div className="divide-y">
          {opportunities.slice(0, 4).map((opp) => (
            <div key={opp.id} className="p-4 hover:bg-muted/50 transition-colors group">
              <div className="flex items-start justify-between gap-4">
                <div className="space-y-1">
                  <h4 className="text-sm font-semibold group-hover:text-amber-600 dark:group-hover:text-amber-400 transition-colors">
                    {opp.title}
                  </h4>
                  <p className="text-sm text-muted-foreground line-clamp-2">{opp.description}</p>
                  
                  {opp.potentialValue > 0 && (
                    <div className="inline-flex items-center px-2 py-0.5 rounded text-xs font-medium bg-green-100 text-green-800 dark:bg-green-900/30 dark:text-green-400 mt-2">
                      +₹{opp.potentialValue.toLocaleString()} potential
                    </div>
                  )}
                </div>
                <Button 
                  size="sm" 
                  variant="ghost" 
                  className="shrink-0 mt-1 hover:bg-amber-100 dark:hover:bg-amber-900/30 hover:text-amber-700 dark:hover:text-amber-400"
                  render={<Link href="/dashboard/growth-center" />}
                >
                  Review
                  <ArrowRight className="w-3 h-3 ml-1.5" />
                </Button>
              </div>
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  );
}
