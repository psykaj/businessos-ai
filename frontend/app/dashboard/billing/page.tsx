'use client';

import { useQuery } from '@tanstack/react-query';
import { billingService } from '@/lib/billing-service';
import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from '@/components/ui/card';
import { Progress } from '@/components/ui/progress';
import { Button } from '@/components/ui/button';
import { Skeleton } from '@/components/ui/skeleton';
import { format } from 'date-fns';
import { useRouter } from 'next/navigation';
import { CheckCircle2, AlertTriangle, ArrowRight, Zap, Database, Cpu, Users, BarChart3, Lock } from 'lucide-react';
import { PremiumFeatureLock } from '@/components/billing/premium-feature-lock';
import { cn } from '@/lib/utils';

export default function BillingDashboard() {
  const router = useRouter();

  const { data: subscription, isLoading: subLoading } = useQuery({
    queryKey: ['subscription'],
    queryFn: billingService.getSubscription,
  });

  const { data: usage, isLoading: usageLoading } = useQuery({
    queryKey: ['usageMetrics'],
    queryFn: billingService.getUsageMetrics,
  });

  const getProgressColor = (current: number, limit: number) => {
    const percent = (current / limit) * 100;
    if (percent >= 90) return 'bg-red-500';
    if (percent >= 75) return 'bg-orange-500';
    return 'bg-primary';
  };

  if (subLoading || usageLoading) {
    return (
      <div className="p-8 max-w-7xl mx-auto space-y-8 animate-pulse">
        <div className="h-20 w-1/3 bg-muted rounded-xl" />
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          <div className="md:col-span-2 h-64 bg-muted rounded-2xl" />
          <div className="h-64 bg-muted rounded-2xl" />
        </div>
      </div>
    );
  }

  return (
    <div className="relative min-h-full p-6 sm:p-10 max-w-7xl mx-auto space-y-10 overflow-hidden">
      {/* Background Decor */}
      <div className="absolute top-0 right-0 -translate-y-1/2 translate-x-1/3 w-[600px] h-[600px] bg-primary/5 rounded-full blur-[100px] -z-10" />

      {/* Header */}
      <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <div>
          <h1 className="text-4xl font-extrabold tracking-tight bg-clip-text text-transparent bg-gradient-to-r from-foreground to-foreground/70">
            Billing & Usage
          </h1>
          <p className="text-muted-foreground mt-2 text-lg">Manage your plan, monitor limits, and access billing settings.</p>
        </div>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
        {/* Current Plan Overview */}
        <Card className="md:col-span-2 border-border/50 shadow-xl bg-background/60 backdrop-blur-xl relative overflow-hidden group rounded-3xl">
          <div className="absolute top-0 right-0 p-8 opacity-5 transition-transform duration-500 group-hover:scale-110 group-hover:rotate-12 group-hover:opacity-10">
            <Zap className="w-48 h-48 text-primary" />
          </div>
          <div className="absolute top-0 inset-x-0 h-1 bg-gradient-to-r from-primary/50 via-primary to-primary/50"></div>
          
          <CardHeader className="p-8 pb-4">
            <CardTitle className="flex items-center gap-2 text-2xl">
              Current Plan
              <span className="text-xs font-bold uppercase tracking-wider bg-primary/10 text-primary px-3 py-1 rounded-full border border-primary/20">
                {subscription?.status === 'active' ? 'Active' : 'Inactive'}
              </span>
            </CardTitle>
            <CardDescription className="text-base mt-2">
              You are currently on the <strong className="text-foreground">{subscription?.plan.name}</strong> plan.
            </CardDescription>
          </CardHeader>
          
          <CardContent className="p-8 pt-4 space-y-6">
            <div className="flex items-end gap-3">
              <div className="text-6xl font-black tracking-tighter text-foreground">${subscription?.plan.monthlyPrice}</div>
              <div className="text-muted-foreground flex flex-col pb-2">
                <span className="font-medium">per month</span>
                <span className="text-sm">
                  Renews on {subscription?.currentPeriodEnd ? format(new Date(subscription.currentPeriodEnd), 'MMM dd, yyyy') : ''}
                </span>
              </div>
            </div>
            
            <div className="flex gap-4 pt-4 border-t border-border/50">
              <Button onClick={() => router.push('/dashboard/subscription')} className="rounded-xl shadow-lg shadow-primary/20 transition-all hover:-translate-y-0.5">
                Manage Subscription
              </Button>
              <Button variant="outline" onClick={() => router.push('/pricing')} className="rounded-xl hover:bg-muted transition-all hover:-translate-y-0.5">
                Upgrade Plan
              </Button>
            </div>
          </CardContent>
        </Card>

        {/* Quick Links */}
        <Card className="border-border/50 shadow-lg bg-background/60 backdrop-blur-xl rounded-3xl overflow-hidden flex flex-col">
          <CardHeader className="bg-muted/30 p-6 border-b border-border/50">
            <CardTitle className="text-xl">Quick Links</CardTitle>
          </CardHeader>
          <CardContent className="p-4 space-y-2 flex-grow">
            {[
              { label: 'Invoices', href: '/dashboard/invoices' },
              { label: 'Payment History', href: '/dashboard/payments' },
              { label: 'Billing Settings', href: '/dashboard/settings' }
            ].map((link, i) => (
              <Button key={i} variant="ghost" className="w-full justify-between hover:bg-primary/5 hover:text-primary rounded-xl h-12" onClick={() => router.push(link.href)}>
                <span className="font-medium">{link.label}</span> 
                <ArrowRight className="w-4 h-4 opacity-50 transition-transform group-hover:translate-x-1" />
              </Button>
            ))}
          </CardContent>
        </Card>
      </div>

      <div className="space-y-6 pt-6">
        <h2 className="text-2xl font-bold tracking-tight flex items-center gap-2">
          <BarChart3 className="w-6 h-6 text-primary" />
          Usage Limits
        </h2>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {usage && [
            { label: 'Dynamic QR Codes', metric: usage.qrs, suffix: '', icon: <Zap className="w-4 h-4 text-primary" /> },
            { label: 'Monthly Scans', metric: usage.scans, suffix: '', icon: <BarChart3 className="w-4 h-4 text-primary" /> },
            { label: 'Storage', metric: usage.storageMB, suffix: 'MB', icon: <Database className="w-4 h-4 text-primary" /> },
            { label: 'AI Credits', metric: usage.aiCredits, suffix: '', icon: <Cpu className="w-4 h-4 text-primary" /> },
            { label: 'API Requests', metric: usage.apiRequests, suffix: '', icon: <Lock className="w-4 h-4 text-primary" /> },
            { label: 'Team Members', metric: usage.teamMembers, suffix: '', icon: <Users className="w-4 h-4 text-primary" /> },
          ].map((item, idx) => {
            const percent = Math.min((item.metric.current / item.metric.limit) * 100, 100);
            const isWarning = percent >= 80;
            return (
              <Card key={idx} className={cn(
                "border-border/50 shadow-md bg-background/50 backdrop-blur-md rounded-2xl transition-all hover:shadow-lg",
                isWarning ? "border-orange-500/30 bg-orange-500/5 shadow-orange-500/10" : ""
              )}>
                <CardHeader className="pb-3 p-5">
                  <CardTitle className="text-sm font-semibold text-muted-foreground flex justify-between items-center">
                    <span className="flex items-center gap-2">{item.icon} {item.label}</span>
                    {isWarning && <AlertTriangle className="w-4 h-4 text-orange-500 animate-pulse" />}
                  </CardTitle>
                </CardHeader>
                <CardContent className="p-5 pt-0">
                  <div className="flex items-baseline gap-1 mb-3">
                    <span className="text-2xl font-bold text-foreground">{item.metric.current.toLocaleString()}</span>
                    <span className="text-sm font-medium text-muted-foreground">/ {item.metric.limit.toLocaleString()} {item.suffix}</span>
                  </div>
                  <Progress 
                    value={percent} 
                    className="h-2 rounded-full overflow-hidden bg-muted"
                  />
                  {isWarning && (
                    <p className="text-xs font-medium text-orange-500 mt-3">
                      Approaching limit. Consider upgrading.
                    </p>
                  )}
                </CardContent>
              </Card>
            );
          })}
        </div>
      </div>

      <div className="pt-8">
        <PremiumFeatureLock requiredPlanLevel={2} featureName="Advanced Analytics Export API">
          <Card className="rounded-3xl border-border/50 shadow-lg bg-background/50 backdrop-blur-xl">
            <CardHeader className="p-8 pb-4">
              <CardTitle className="text-xl flex items-center gap-2">
                <Lock className="w-5 h-5 text-muted-foreground" />
                Advanced Developer APIs
              </CardTitle>
              <CardDescription>Export and stream all analytics data to your external data warehouse.</CardDescription>
            </CardHeader>
            <CardContent className="p-8 pt-4">
              <div className="h-32 bg-muted/30 rounded-2xl flex items-center justify-center border-2 border-dashed border-border/60">
                <span className="text-sm font-medium text-muted-foreground flex items-center gap-2">
                  <Cpu className="w-4 h-4" /> API Keys and Webhooks Dashboard
                </span>
              </div>
            </CardContent>
          </Card>
        </PremiumFeatureLock>
      </div>

    </div>
  );
}
