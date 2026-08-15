import { TrendingUp, Clock, CheckSquare, ArrowRight } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import Link from "next/link";

interface TodayBusinessValueProps {
  isLoading?: boolean;
}

export function TodayBusinessValue({ isLoading }: TodayBusinessValueProps) {
  if (isLoading) {
    return <Skeleton className="h-[120px] w-full rounded-xl" />;
  }

  return (
    <div className="bg-card border border-border/60 rounded-xl p-5 relative overflow-hidden">
      <div className="absolute right-0 top-0 bottom-0 w-32 bg-gradient-to-l from-primary/5 to-transparent pointer-events-none"></div>
      
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 mb-4">
        <h3 className="text-sm font-semibold text-foreground">Recent Business Impact</h3>
        <Button variant="link" size="sm" className="h-auto p-0 text-muted-foreground hover:text-primary">
          <Link href="/dashboard/outcomes" className="flex items-center">
            View Outcomes <ArrowRight className="w-3 h-3 ml-1" />
          </Link>
        </Button>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
        
        <div className="flex items-center gap-3">
          <div className="w-8 h-8 rounded-full bg-emerald-500/10 text-emerald-600 flex items-center justify-center">
            <TrendingUp className="w-4 h-4" />
          </div>
          <div>
            <p className="text-sm font-semibold text-foreground">₹50,000</p>
            <p className="text-xs text-muted-foreground">revenue recovered</p>
          </div>
        </div>
        
        <div className="flex items-center gap-3">
          <div className="w-8 h-8 rounded-full bg-blue-500/10 text-blue-600 flex items-center justify-center">
            <Clock className="w-4 h-4" />
          </div>
          <div>
            <p className="text-sm font-semibold text-foreground">14.5 hours</p>
            <p className="text-xs text-muted-foreground">estimated time saved</p>
          </div>
        </div>

        <div className="flex items-center gap-3">
          <div className="w-8 h-8 rounded-full bg-purple-500/10 text-purple-600 flex items-center justify-center">
            <CheckSquare className="w-4 h-4" />
          </div>
          <div>
            <p className="text-sm font-semibold text-foreground">23 actions</p>
            <p className="text-xs text-muted-foreground">successfully completed</p>
          </div>
        </div>

      </div>
    </div>
  );
}
