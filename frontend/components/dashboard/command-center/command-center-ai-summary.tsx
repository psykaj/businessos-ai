import { BusinessBriefingDto } from "@/lib/command-center-service";
import { Card, CardContent, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
import { Button, buttonVariants } from "@/components/ui/button";
import { Bot, FileText, ExternalLink } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";
import Link from "next/link";
import { formatDistanceToNow } from "date-fns";

interface CommandCenterAiSummaryProps {
  summary?: BusinessBriefingDto;
  isLoading: boolean;
}

export function CommandCenterAiSummary({ summary, isLoading }: CommandCenterAiSummaryProps) {
  if (isLoading) {
    return (
      <Card className="h-full bg-primary/5 border-primary/20">
        <CardHeader className="pb-2">
          <CardTitle className="flex items-center text-primary">
            <Bot className="w-5 h-5 mr-2" />
            AI Executive Summary
          </CardTitle>
        </CardHeader>
        <CardContent className="space-y-4">
          <Skeleton className="h-4 w-full" />
          <Skeleton className="h-4 w-[90%]" />
          <Skeleton className="h-4 w-[85%]" />
        </CardContent>
      </Card>
    );
  }

  if (!summary) {
    return (
      <Card className="h-full bg-primary/5 border-primary/20">
        <CardHeader className="pb-2">
          <CardTitle className="flex items-center text-primary">
            <Bot className="w-5 h-5 mr-2" />
            AI Executive Summary
          </CardTitle>
        </CardHeader>
        <CardContent>
          <p className="text-sm text-muted-foreground">AI Summary temporarily unavailable.</p>
        </CardContent>
      </Card>
    );
  }

  const generatedTime = summary.generatedAt ? formatDistanceToNow(new Date(summary.generatedAt), { addSuffix: true }) : "recently";

  return (
    <Card className="h-full bg-gradient-to-br from-primary/10 via-background to-background border-primary/20 shadow-sm relative overflow-hidden">
      <div className="absolute -top-12 -right-12 w-32 h-32 bg-primary/10 rounded-full blur-2xl pointer-events-none" />
      <CardHeader className="pb-2">
        <div className="flex items-center justify-between">
          <CardTitle className="flex items-center text-primary">
            <Bot className="w-5 h-5 mr-2" />
            AI Executive Summary
          </CardTitle>
          <span className="text-[10px] uppercase font-semibold tracking-wider text-primary/60 bg-primary/10 px-2 py-1 rounded-full">
            Auto-Generated
          </span>
        </div>
      </CardHeader>
      <CardContent className="space-y-4 relative z-10">
        <p className="text-sm leading-relaxed text-foreground/90 font-medium">
          {summary.executiveSummary}
        </p>
        
        {summary.keyHighlights?.length > 0 && (
          <div className="pt-2 border-t border-border/50">
            <h4 className="text-xs font-semibold text-muted-foreground uppercase tracking-wider mb-2">Key Highlights</h4>
            <ul className="space-y-1">
              {summary.keyHighlights.slice(0, 2).map((highlight, idx) => (
                <li key={idx} className="text-sm flex items-start">
                  <span className="text-primary mr-2">•</span>
                  {highlight}
                </li>
              ))}
            </ul>
          </div>
        )}
      </CardContent>
      <CardFooter className="pt-2 flex justify-between items-center text-xs text-muted-foreground border-t bg-muted/20">
        <span>Generated {generatedTime}</span>
        <Link href="/dashboard/executive" className={buttonVariants({ variant: "link", size: "sm", className: "h-auto p-0 text-primary" })}>
          <FileText className="w-3 h-3 mr-1" />
          Full Briefing
        </Link>
      </CardFooter>
    </Card>
  );
}
