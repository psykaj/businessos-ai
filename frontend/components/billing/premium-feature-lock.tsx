'use client';

import React from 'react';
import { useQuery } from '@tanstack/react-query';
import { billingService } from '@/lib/billing-service';
import { Button } from '@/components/ui/button';
import { LockKeyhole } from 'lucide-react';
import { useRouter } from 'next/navigation';
import { Skeleton } from '@/components/ui/skeleton';

interface PremiumFeatureLockProps {
  children: React.ReactNode;
  requiredPlanLevel?: number; // 0 = Free, 1 = Starter, 2 = Pro, 3 = Enterprise
  featureName?: string;
}

const planLevelMap: Record<string, number> = {
  plan_free: 0,
  plan_starter: 1,
  plan_pro: 2,
  plan_enterprise: 3,
};

export function PremiumFeatureLock({ children, requiredPlanLevel = 1, featureName = 'This feature' }: PremiumFeatureLockProps) {
  const router = useRouter();
  
  const { data: subscription, isLoading } = useQuery({
    queryKey: ['subscription'],
    queryFn: billingService.getSubscription,
  });

  if (isLoading) {
    return (
      <div className="relative">
        <div className="blur-sm opacity-50 pointer-events-none select-none transition-all duration-300">
          {children}
        </div>
        <div className="absolute inset-0 flex items-center justify-center">
          <Skeleton className="h-[200px] w-[350px] rounded-xl" />
        </div>
      </div>
    );
  }

  const currentLevel = planLevelMap[subscription?.planId || 'plan_free'] || 0;
  const hasAccess = currentLevel >= requiredPlanLevel;

  if (hasAccess) {
    return <>{children}</>;
  }

  return (
    <div className="relative group">
      {/* Blurred out content */}
      <div className="blur-[4px] opacity-40 pointer-events-none select-none transition-all duration-300">
        {children}
      </div>

      {/* Lock overlay */}
      <div className="absolute inset-0 flex items-center justify-center z-10">
        <div className="bg-background/95 backdrop-blur-md border border-border shadow-2xl rounded-2xl p-8 max-w-sm text-center transform transition-all duration-300 scale-100 group-hover:scale-105">
          <div className="bg-primary/10 w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-4">
            <LockKeyhole className="w-8 h-8 text-primary" />
          </div>
          <h3 className="text-xl font-semibold tracking-tight mb-2">Premium Feature</h3>
          <p className="text-muted-foreground mb-6 text-sm">
            {featureName} is only available on upgraded plans. Upgrade your subscription to unlock this and many other premium features.
          </p>
          <div className="flex flex-col space-y-3">
            <Button onClick={() => router.push('/dashboard/subscription')} className="w-full font-medium">
              Upgrade Plan
            </Button>
            <Button variant="outline" onClick={() => router.push('/pricing')} className="w-full">
              Compare Plans
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
}
