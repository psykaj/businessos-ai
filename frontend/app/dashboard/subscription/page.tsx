'use client';

import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { billingService } from '@/lib/billing-service';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Alert, AlertDescription, AlertTitle } from '@/components/ui/alert';
import { AlertDialog, AlertDialogAction, AlertDialogCancel, AlertDialogContent, AlertDialogDescription, AlertDialogFooter, AlertDialogHeader, AlertDialogTitle, AlertDialogTrigger } from '@/components/ui/alert-dialog';
import { format } from 'date-fns';
import { AlertTriangle, CheckCircle2, ShieldAlert, Zap } from 'lucide-react';
import { toast } from 'sonner';
import { cn } from '@/lib/utils';

export default function SubscriptionManagement() {
  const queryClient = useQueryClient();
  const [isCancelling, setIsCancelling] = useState(false);

  const { data: subscription, isLoading } = useQuery({
    queryKey: ['subscription'],
    queryFn: billingService.getSubscription,
  });

  const cancelMutation = useMutation({
    mutationFn: billingService.cancelSubscription,
    onSuccess: () => {
      toast.success('Subscription cancelled. Access remains until the billing cycle ends.');
      queryClient.invalidateQueries({ queryKey: ['subscription'] });
    },
    onError: () => toast.error('Failed to cancel subscription.')
  });

  const resumeMutation = useMutation({
    mutationFn: billingService.resumeSubscription,
    onSuccess: () => {
      toast.success('Subscription resumed successfully!');
      queryClient.invalidateQueries({ queryKey: ['subscription'] });
    },
    onError: () => toast.error('Failed to resume subscription.')
  });

  if (isLoading) {
    return (
      <div className="p-8 max-w-4xl mx-auto space-y-8 animate-pulse">
        <div className="h-16 w-1/2 bg-muted rounded-xl" />
        <div className="h-[400px] w-full bg-muted rounded-3xl" />
      </div>
    );
  }

  return (
    <div className="relative min-h-full p-6 sm:p-10 max-w-5xl mx-auto space-y-10">
      {/* Background Decor */}
      <div className="absolute top-0 left-0 -translate-y-1/4 -translate-x-1/4 w-[500px] h-[500px] bg-primary/10 rounded-full blur-[100px] -z-10" />

      <div>
        <h1 className="text-4xl font-extrabold tracking-tight bg-clip-text text-transparent bg-gradient-to-r from-foreground to-foreground/70">
          Manage Subscription
        </h1>
        <p className="text-muted-foreground mt-2 text-lg">View your subscription status, change plans, or cancel your account.</p>
      </div>

      <Card className="border-border/50 shadow-xl bg-background/60 backdrop-blur-xl rounded-3xl overflow-hidden relative">
        <div className="absolute top-0 inset-x-0 h-1 bg-gradient-to-r from-transparent via-primary/30 to-transparent"></div>
        
        <CardHeader className="p-8 pb-6 border-b border-border/30">
          <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
            <div>
              <CardTitle className="text-2xl flex items-center gap-3">
                Current Status
                <Badge variant={subscription?.status === 'active' ? 'default' : 'destructive'} className={cn(
                  "uppercase text-[10px] tracking-wider px-2 py-0.5",
                  subscription?.status === 'active' ? "bg-primary/20 text-primary hover:bg-primary/30" : ""
                )}>
                  {subscription?.status}
                </Badge>
              </CardTitle>
              <CardDescription className="mt-3 text-base">
                Plan: <strong className="text-foreground font-bold">{subscription?.plan.name}</strong> 
                <span className="text-muted-foreground ml-1">(${subscription?.plan.monthlyPrice}/mo)</span>
              </CardDescription>
            </div>
            {subscription?.cancelAtPeriodEnd && (
              <Badge variant="secondary" className="bg-orange-500/10 text-orange-600 border-orange-500/20 px-3 py-1.5 rounded-xl shadow-sm text-sm font-medium flex items-center gap-2">
                <AlertTriangle className="w-4 h-4" />
                Cancels on {format(new Date(subscription.currentPeriodEnd), 'MMM dd, yyyy')}
              </Badge>
            )}
          </div>
        </CardHeader>
        
        <CardContent className="p-8 space-y-6">
          {subscription?.cancelAtPeriodEnd ? (
            <Alert className="border-orange-500/30 bg-orange-500/5 p-6 rounded-2xl">
              <ShieldAlert className="h-5 w-5 text-orange-500 mt-0.5" />
              <div className="pl-3">
                <AlertTitle className="text-orange-600 font-bold text-lg">Cancellation Pending</AlertTitle>
                <AlertDescription className="text-orange-600/80 mt-2 text-sm leading-relaxed">
                  Your subscription is scheduled to be cancelled at the end of your billing cycle on <strong className="font-bold">{format(new Date(subscription.currentPeriodEnd), 'MMM dd, yyyy')}</strong>. 
                  You will lose access to premium features after this date. You can resume your subscription at any time before then.
                </AlertDescription>
              </div>
            </Alert>
          ) : (
            <Alert className="border-primary/20 bg-primary/5 p-6 rounded-2xl shadow-sm">
              <CheckCircle2 className="h-5 w-5 text-primary mt-0.5" />
              <div className="pl-3">
                <AlertTitle className="text-primary font-bold text-lg">Active Subscription</AlertTitle>
                <AlertDescription className="text-primary/80 mt-2 text-sm leading-relaxed">
                  Your subscription is active and will automatically renew on <strong className="font-bold">{format(new Date(subscription?.currentPeriodEnd || ''), 'MMM dd, yyyy')}</strong>.
                  Enjoy uninterrupted access to all your premium features.
                </AlertDescription>
              </div>
            </Alert>
          )}
        </CardContent>
        
        <CardFooter className="flex flex-col sm:flex-row justify-between items-center border-t border-border/30 p-8 bg-muted/10 gap-4">
          <Button variant="outline" className="w-full sm:w-auto rounded-xl hover:bg-muted h-11 px-6 shadow-sm font-medium" onClick={() => window.location.href = '/pricing'}>
            <Zap className="w-4 h-4 mr-2 text-primary" /> Change Plan
          </Button>
          
          <div className="w-full sm:w-auto">
            {subscription?.cancelAtPeriodEnd ? (
              <Button onClick={() => resumeMutation.mutate()} disabled={resumeMutation.isPending} className="w-full rounded-xl shadow-lg shadow-primary/20 h-11 px-6">
                {resumeMutation.isPending ? 'Resuming...' : 'Resume Subscription'}
              </Button>
            ) : (
              <AlertDialog>
                <AlertDialogTrigger className="w-full bg-destructive/10 text-destructive hover:bg-destructive/20 h-11 px-6 inline-flex items-center justify-center rounded-xl text-sm font-semibold transition-colors shadow-sm">
                  Cancel Subscription
                </AlertDialogTrigger>
                <AlertDialogContent className="rounded-3xl border-border/50">
                  <AlertDialogHeader>
                    <AlertDialogTitle className="text-2xl">Are you absolutely sure?</AlertDialogTitle>
                    <AlertDialogDescription className="text-base mt-2 leading-relaxed">
                      This action will mark your subscription for cancellation. You will lose access to premium features at the end of your current billing cycle on <strong className="text-foreground">{format(new Date(subscription?.currentPeriodEnd || ''), 'MMM dd, yyyy')}</strong>.
                    </AlertDialogDescription>
                  </AlertDialogHeader>
                  <AlertDialogFooter className="mt-6">
                    <AlertDialogCancel className="rounded-xl h-11">Keep Subscription</AlertDialogCancel>
                    <AlertDialogAction 
                      className="bg-destructive hover:bg-destructive/90 text-destructive-foreground rounded-xl h-11 shadow-lg shadow-destructive/20"
                      onClick={(e) => { e.preventDefault(); cancelMutation.mutate(); }}
                    >
                      {cancelMutation.isPending ? 'Cancelling...' : 'Yes, cancel subscription'}
                    </AlertDialogAction>
                  </AlertDialogFooter>
                </AlertDialogContent>
              </AlertDialog>
            )}
          </div>
        </CardFooter>
      </Card>
    </div>
  );
}
