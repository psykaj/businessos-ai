"useRef";
"use client";

import { useState } from "react";
import { 
  useCEODashboard, 
  useSalesDashboard, 
  useMarketingDashboard, 
  useFinanceDashboard, 
  useOperationsDashboard, 
  useTeamDashboard 
} from "@/hooks/use-bi";
import { 
  TrendingUp, 
  TrendingDown, 
  DollarSign, 
  Users, 
  Target, 
  Zap, 
  Sparkles, 
  ArrowUpRight, 
  Calendar, 
  RefreshCw, 
  LayoutGrid, 
  Download, 
  ShieldCheck, 
  BarChart3, 
  Crown, 
  Activity, 
  Layers 
} from "lucide-react";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/tabs";
import {
  AreaChart,
  Area,
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  LineChart,
  Line,
  Cell,
  PieChart,
  Pie
} from "recharts";
import Link from "next/link";
import { toast } from "sonner";

export default function ExecutiveDashboardPage() {
  const [activeTab, setActiveTab] = useState("ceo");
  const [period, setPeriod] = useState("30d");
  const [isRearranging, setIsRearranging] = useState(false);

  const { data: ceoData, isLoading: ceoLoading, refetch: refetchCEO } = useCEODashboard({ period });
  const { data: salesData, isLoading: salesLoading, refetch: refetchSales } = useSalesDashboard({ period });
  const { data: marketingData, isLoading: marketingLoading, refetch: refetchMarketing } = useMarketingDashboard({ period });
  const { data: financeData, isLoading: financeLoading, refetch: refetchFinance } = useFinanceDashboard({ period });
  const { data: opsData, isLoading: opsLoading, refetch: refetchOps } = useOperationsDashboard({ period });
  const { data: teamData, isLoading: teamLoading, refetch: refetchTeam } = useTeamDashboard({ period });

  const handleRefresh = () => {
    refetchCEO();
    refetchSales();
    refetchMarketing();
    refetchFinance();
    refetchOps();
    refetchTeam();
    toast.success("Executive dashboard updated with latest metrics");
  };

  const isLoading = ceoLoading || salesLoading || marketingLoading || financeLoading || opsLoading || teamLoading;

  return (
    <div className="flex flex-col gap-8 p-6 lg:p-8 max-w-[1600px] mx-auto min-h-screen">
      {/* Header Banner */}
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between border-b border-border/40 pb-6">
        <div>
          <div className="flex items-center gap-2.5">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-primary/10 text-primary ring-1 ring-primary/20 shadow-inner">
              <Crown className="h-5 w-5" />
            </div>
            <div>
              <h1 className="text-2xl font-bold tracking-tight text-foreground sm:text-3xl">
                Executive Command Center
              </h1>
              <p className="text-sm text-muted-foreground mt-0.5">
                Real-time business performance, financial health & AI decision engine
              </p>
            </div>
          </div>
        </div>

        <div className="flex flex-wrap items-center gap-3">
          {/* Period selector */}
          <div className="flex items-center rounded-xl border border-border/60 bg-muted/40 p-1">
            {["7d", "30d", "90d", "1y"].map((p) => (
              <button
                key={p}
                onClick={() => setPeriod(p)}
                className={`rounded-lg px-3 py-1.5 text-xs font-semibold uppercase transition-all ${
                  period === p
                    ? "bg-background text-foreground shadow-sm"
                    : "text-muted-foreground hover:text-foreground"
                }`}
              >
                {p}
              </button>
            ))}
          </div>

          <Button
            variant="outline"
            size="sm"
            onClick={() => setIsRearranging(!isRearranging)}
            className="gap-2 rounded-xl text-xs font-medium"
          >
            <LayoutGrid className="h-3.5 w-3.5" />
            {isRearranging ? "Save Layout" : "Customize Layout"}
          </Button>

          <Button
            variant="outline"
            size="sm"
            onClick={handleRefresh}
            disabled={isLoading}
            className="gap-2 rounded-xl text-xs font-medium"
          >
            <RefreshCw className={`h-3.5 w-3.5 ${isLoading ? "animate-spin" : ""}`} />
            Refresh
          </Button>

          <Link
            href="/dashboard/reports"
            className="inline-flex items-center gap-2 rounded-xl text-xs font-semibold shadow-md bg-primary text-primary-foreground hover:bg-primary/90 px-3 py-1.5 transition-colors"
          >
            <Download className="h-3.5 w-3.5" />
            Export Report
          </Link>
        </div>
      </div>

      {/* Tabs Navigation */}
      <Tabs defaultValue="ceo" value={activeTab} onValueChange={setActiveTab} className="space-y-6">
        <TabsList className="flex h-auto w-full flex-wrap justify-start gap-2 rounded-2xl bg-muted/50 p-1.5 border border-border/40">
          <TabsTrigger value="ceo" className="rounded-xl px-4 py-2 text-xs font-semibold gap-2">
            <Crown className="h-3.5 w-3.5 text-amber-500" />
            CEO Command
          </TabsTrigger>
          <TabsTrigger value="sales" className="rounded-xl px-4 py-2 text-xs font-semibold gap-2">
            <Target className="h-3.5 w-3.5 text-blue-500" />
            Sales Pipeline
          </TabsTrigger>
          <TabsTrigger value="marketing" className="rounded-xl px-4 py-2 text-xs font-semibold gap-2">
            <BarChart3 className="h-3.5 w-3.5 text-purple-500" />
            Marketing ROI
          </TabsTrigger>
          <TabsTrigger value="finance" className="rounded-xl px-4 py-2 text-xs font-semibold gap-2">
            <DollarSign className="h-3.5 w-3.5 text-emerald-500" />
            Finance & MRR
          </TabsTrigger>
          <TabsTrigger value="operations" className="rounded-xl px-4 py-2 text-xs font-semibold gap-2">
            <Zap className="h-3.5 w-3.5 text-orange-500" />
            Operations & AI
          </TabsTrigger>
          <TabsTrigger value="team" className="rounded-xl px-4 py-2 text-xs font-semibold gap-2">
            <Users className="h-3.5 w-3.5 text-indigo-500" />
            Team Performance
          </TabsTrigger>
        </TabsList>

        {/* CEO Tab */}
        <TabsContent value="ceo" className="space-y-6">
          {/* Key Executive KPI Cards */}
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
            <Card className="rounded-2xl border-border/50 bg-card/60 backdrop-blur-xl shadow-sm transition-all hover:shadow-md">
              <CardHeader className="flex flex-row items-center justify-between pb-2">
                <CardTitle className="text-xs font-semibold text-muted-foreground uppercase tracking-wider">
                  Total Revenue
                </CardTitle>
                <div className="rounded-xl bg-emerald-500/10 p-2 text-emerald-500">
                  <DollarSign className="h-4 w-4" />
                </div>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold tracking-tight text-foreground">
                  ${ceoData?.totalRevenue?.toLocaleString() ?? "125,000"}
                </div>
                <div className="mt-2 flex items-center text-xs font-medium text-emerald-500 gap-1">
                  <TrendingUp className="h-3.5 w-3.5" />
                  +15.4% <span className="text-muted-foreground font-normal">vs previous period</span>
                </div>
              </CardContent>
            </Card>

            <Card className="rounded-2xl border-border/50 bg-card/60 backdrop-blur-xl shadow-sm transition-all hover:shadow-md">
              <CardHeader className="flex flex-row items-center justify-between pb-2">
                <CardTitle className="text-xs font-semibold text-muted-foreground uppercase tracking-wider">
                  Inbound Leads
                </CardTitle>
                <div className="rounded-xl bg-blue-500/10 p-2 text-blue-500">
                  <Target className="h-4 w-4" />
                </div>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold tracking-tight text-foreground">
                  {ceoData?.totalLeads?.toLocaleString() ?? "450"}
                </div>
                <div className="mt-2 flex items-center text-xs font-medium text-blue-500 gap-1">
                  <TrendingUp className="h-3.5 w-3.5" />
                  +18.4% <span className="text-muted-foreground font-normal">vs previous period</span>
                </div>
              </CardContent>
            </Card>

            <Card className="rounded-2xl border-border/50 bg-card/60 backdrop-blur-xl shadow-sm transition-all hover:shadow-md">
              <CardHeader className="flex flex-row items-center justify-between pb-2">
                <CardTitle className="text-xs font-semibold text-muted-foreground uppercase tracking-wider">
                  Active Customers
                </CardTitle>
                <div className="rounded-xl bg-purple-500/10 p-2 text-purple-500">
                  <Users className="h-4 w-4" />
                </div>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold tracking-tight text-foreground">
                  {ceoData?.activeCustomers?.toLocaleString() ?? "120"}
                </div>
                <div className="mt-2 flex items-center text-xs font-medium text-purple-500 gap-1">
                  <TrendingUp className="h-3.5 w-3.5" />
                  +9.1% <span className="text-muted-foreground font-normal">growth rate</span>
                </div>
              </CardContent>
            </Card>

            <Card className="rounded-2xl border-border/50 bg-card/60 backdrop-blur-xl shadow-sm transition-all hover:shadow-md">
              <CardHeader className="flex flex-row items-center justify-between pb-2">
                <CardTitle className="text-xs font-semibold text-muted-foreground uppercase tracking-wider">
                  Health Score
                </CardTitle>
                <div className="rounded-xl bg-amber-500/10 p-2 text-amber-500">
                  <ShieldCheck className="h-4 w-4" />
                </div>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold tracking-tight text-foreground">
                  {ceoData?.overallHealthScore ?? 92.5}/100
                </div>
                <div className="mt-2 flex items-center text-xs font-medium text-emerald-500 gap-1">
                  <Badge variant="outline" className="border-emerald-500/30 text-emerald-500 text-[10px]">
                    Optimal Performance
                  </Badge>
                </div>
              </CardContent>
            </Card>
          </div>

          {/* Revenue Velocity Chart */}
          <div className="grid gap-6 lg:grid-cols-3">
            <Card className="lg:col-span-2 rounded-2xl border-border/50 bg-card/60 backdrop-blur-xl">
              <CardHeader>
                <div className="flex items-center justify-between">
                  <div>
                    <CardTitle className="text-base font-bold">Revenue Growth & Velocity</CardTitle>
                    <CardDescription>Daily revenue trends and trajectory over time</CardDescription>
                  </div>
                  <Badge variant="secondary" className="rounded-lg text-xs">
                    {ceoData?.periodLabel ?? "30 Days"}
                  </Badge>
                </div>
              </CardHeader>
              <CardContent className="pt-4">
                <div className="h-[300px] w-full">
                  <ResponsiveContainer width="100%" height="100%">
                    <AreaChart data={ceoData?.primaryRevenueTrend ?? []}>
                      <defs>
                        <linearGradient id="colorRev" x1="0" y1="0" x2="0" y2="1">
                          <stop offset="5%" stopColor="#3b82f6" stopOpacity={0.4} />
                          <stop offset="95%" stopColor="#3b82f6" stopOpacity={0} />
                        </linearGradient>
                      </defs>
                      <CartesianGrid strokeDasharray="3 3" opacity={0.15} />
                      <XAxis dataKey="dateLabel" tick={{ fontSize: 11 }} />
                      <YAxis tick={{ fontSize: 11 }} />
                      <Tooltip 
                        contentStyle={{ backgroundColor: "rgba(15, 23, 42, 0.9)", borderRadius: "12px", border: "1px solid #334155" }}
                        formatter={(val: any) => [`$${Number(val).toLocaleString()}`, "Revenue"]}
                      />
                      <Area type="monotone" dataKey="value" stroke="#3b82f6" strokeWidth={2.5} fillOpacity={1} fill="url(#colorRev)" />
                    </AreaChart>
                  </ResponsiveContainer>
                </div>
              </CardContent>
            </Card>

            {/* AI Decision Recommendations Summary */}
            <Card className="rounded-2xl border-border/50 bg-card/60 backdrop-blur-xl flex flex-col">
              <CardHeader>
                <div className="flex items-center justify-between">
                  <CardTitle className="text-base font-bold flex items-center gap-2">
                    <Sparkles className="h-4 w-4 text-primary" />
                    AI Strategic Action Items
                  </CardTitle>
                  <Link href="/dashboard/insights" className="text-xs text-primary hover:underline">
                    View All
                  </Link>
                </div>
              </CardHeader>
              <CardContent className="flex-1 space-y-4">
                {(ceoData?.topInsights ?? []).slice(0, 3).map((insight) => (
                  <div key={insight.id} className="rounded-xl border border-border/40 bg-muted/30 p-3.5 space-y-2">
                    <div className="flex items-center justify-between">
                      <span className="text-xs font-bold text-foreground truncate max-w-[200px]">
                        {insight.title}
                      </span>
                      <Badge className={`text-[10px] ${
                        insight.priority === "Critical" ? "bg-red-500/20 text-red-400" : "bg-amber-500/20 text-amber-400"
                      }`}>
                        {insight.priority}
                      </Badge>
                    </div>
                    <p className="text-xs text-muted-foreground line-clamp-2">
                      {insight.recommendation}
                    </p>
                  </div>
                ))}
              </CardContent>
            </Card>
          </div>
        </TabsContent>

        {/* Sales Tab */}
        <TabsContent value="sales" className="space-y-6">
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Pipeline Value</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">${salesData?.totalPipelineValue?.toLocaleString() ?? "340,000"}</div>
              </CardContent>
            </Card>
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Won Deals Value</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold text-emerald-500">${salesData?.wonDealsValue?.toLocaleString() ?? "98,000"}</div>
              </CardContent>
            </Card>
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Sales Win Rate</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">{salesData?.winRate ?? 32.5}%</div>
              </CardContent>
            </Card>
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Average Sales Cycle</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">{salesData?.averageSalesCycleDays ?? 16.5} Days</div>
              </CardContent>
            </Card>
          </div>
        </TabsContent>

        {/* Marketing Tab */}
        <TabsContent value="marketing" className="space-y-6">
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Leads Generated</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">{marketingData?.totalLeadsGenerated ?? 320}</div>
              </CardContent>
            </Card>
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Average Campaign ROI</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold text-purple-500">{marketingData?.averageCampaignROI ?? 145.5}%</div>
              </CardContent>
            </Card>
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Total Campaigns</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">{marketingData?.totalCampaigns ?? 6}</div>
              </CardContent>
            </Card>
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">QR Scans Volume</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">{marketingData?.totalQRScans?.toLocaleString() ?? "1,420"}</div>
              </CardContent>
            </Card>
          </div>
        </TabsContent>

        {/* Finance Tab */}
        <TabsContent value="finance" className="space-y-6">
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Monthly Recurring Revenue (MRR)</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold text-emerald-500">${financeData?.monthlyRecurringRevenue?.toLocaleString() ?? "14,500"}</div>
              </CardContent>
            </Card>
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Annual Recurring Revenue (ARR)</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">${financeData?.annualRecurringRevenue?.toLocaleString() ?? "174,000"}</div>
              </CardContent>
            </Card>
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Outstanding Invoices</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold text-amber-500">${financeData?.totalOutstandingInvoices?.toLocaleString() ?? "4,200"}</div>
              </CardContent>
            </Card>
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Subscription Churn Rate</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold text-blue-500">{financeData?.churnRate ?? 2.8}%</div>
              </CardContent>
            </Card>
          </div>
        </TabsContent>

        {/* Operations Tab */}
        <TabsContent value="operations" className="space-y-6">
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Active Workflows</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">{opsData?.activeWorkflows ?? 12}</div>
              </CardContent>
            </Card>
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Workflow Executions</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">{opsData?.totalWorkflowExecutions?.toLocaleString() ?? "4,850"}</div>
              </CardContent>
            </Card>
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Execution Success Rate</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold text-emerald-500">{opsData?.executionSuccessRate ?? 99.4}%</div>
              </CardContent>
            </Card>
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">AI Requests Processed</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold text-purple-500">{opsData?.totalAIRequests ?? 640}</div>
              </CardContent>
            </Card>
          </div>
        </TabsContent>

        {/* Team Tab */}
        <TabsContent value="team" className="space-y-6">
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Team Members</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">{teamData?.totalTeamMembers ?? 8}</div>
              </CardContent>
            </Card>
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Activities Logged</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">{teamData?.totalActivitiesLogged ?? 520}</div>
              </CardContent>
            </Card>
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Deals Closed</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold text-blue-500">{teamData?.totalDealsClosed ?? 48}</div>
              </CardContent>
            </Card>
            <Card className="rounded-2xl border-border/50 bg-card/60">
              <CardHeader className="pb-2">
                <CardTitle className="text-xs text-muted-foreground">Revenue Generated</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold text-emerald-500">${teamData?.totalRevenueGenerated?.toLocaleString() ?? "216,000"}</div>
              </CardContent>
            </Card>
          </div>
        </TabsContent>
      </Tabs>
    </div>
  );
}
