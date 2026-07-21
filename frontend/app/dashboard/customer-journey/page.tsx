"use client";

import { useMarketingDashboardMetrics } from "@/hooks/use-marketing-analytics";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Users, UserPlus, FileCheck, Crown, ArrowRight } from "lucide-react";
import { cn } from "@/lib/utils";

const STAGES = [
  { id: "visitor", label: "Visitor", icon: Users, color: "bg-slate-500", text: "text-slate-500", count: 12450 },
  { id: "lead", label: "Lead", icon: UserPlus, color: "bg-blue-500", text: "text-blue-500", count: 3240 },
  { id: "qualified", label: "Qualified Lead", icon: FileCheck, color: "bg-indigo-500", text: "text-indigo-500", count: 850 },
  { id: "customer", label: "Customer", icon: Crown, color: "bg-emerald-500", text: "text-emerald-500", count: 320 },
];

export default function CustomerJourneyPage() {
  const { data: metrics, isLoading } = useMarketingDashboardMetrics();

  return (
    <div className="flex flex-col gap-8 p-8 max-w-6xl mx-auto w-full">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Customer Journey Pipeline</h1>
        <p className="text-muted-foreground mt-1">Visualize how contacts move through your sales and marketing funnel.</p>
      </div>

      {isLoading ? (
        <div className="p-12 text-center text-muted-foreground">Loading funnel data...</div>
      ) : (
        <div className="flex flex-col gap-6">
          <Card>
            <CardHeader>
              <CardTitle>Journey Funnel</CardTitle>
              <CardDescription>Conversion metrics across the entire lifecycle (Last 30 Days)</CardDescription>
            </CardHeader>
            <CardContent>
              <div className="relative flex flex-col gap-2 w-full py-8">
                {STAGES.map((stage, idx) => {
                  const Icon = stage.icon;
                  // Calculate dummy width for visual funnel effect (100% -> 75% -> 50% -> 25%)
                  const width = `${100 - (idx * 20)}%`;
                  
                  let conversion = "";
                  if (idx > 0) {
                    const prevCount = STAGES[idx - 1].count;
                    conversion = ((stage.count / prevCount) * 100).toFixed(1) + "%";
                  }

                  return (
                    <div key={stage.id} className="flex flex-col items-center">
                      {idx > 0 && (
                        <div className="flex flex-col items-center my-2 text-muted-foreground">
                          <span className="text-xs font-semibold bg-muted px-2 py-0.5 rounded-full mb-1">
                            {conversion} conversion
                          </span>
                          <ArrowRight className="h-4 w-4 rotate-90" />
                        </div>
                      )}
                      
                      <div 
                        className={cn(
                          "flex items-center justify-between p-4 rounded-xl text-white shadow-sm transition-all hover:scale-[1.02] cursor-pointer",
                          stage.color
                        )}
                        style={{ width }}
                      >
                        <div className="flex items-center gap-3">
                          <div className="bg-white/20 p-2 rounded-lg">
                            <Icon className="h-5 w-5" />
                          </div>
                          <span className="font-semibold text-lg">{stage.label}</span>
                        </div>
                        <span className="font-bold text-xl">{stage.count.toLocaleString()}</span>
                      </div>
                    </div>
                  );
                })}
              </div>
            </CardContent>
          </Card>

          <div className="grid gap-6 md:grid-cols-3">
            <Card>
              <CardHeader className="pb-2">
                <CardTitle className="text-sm font-medium">Avg Time to Convert</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">14 Days</div>
                <p className="text-xs text-emerald-500 mt-1">Faster by 2 days this month</p>
              </CardContent>
            </Card>
            <Card>
              <CardHeader className="pb-2">
                <CardTitle className="text-sm font-medium">Biggest Drop-off</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold text-destructive">Lead → Qualified</div>
                <p className="text-xs text-muted-foreground mt-1">Only 26.2% conversion rate</p>
              </CardContent>
            </Card>
            <Card>
              <CardHeader className="pb-2">
                <CardTitle className="text-sm font-medium">Top Converting Source</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">{metrics?.topLeadSource || "Google Ads"}</div>
                <p className="text-xs text-emerald-500 mt-1">Drives 42% of Customers</p>
              </CardContent>
            </Card>
          </div>
        </div>
      )}
    </div>
  );
}
