'use client';

import { useState } from 'react';
import { billingService } from '@/lib/billing-service';
import { useQuery } from '@tanstack/react-query';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '@/components/ui/card';
import { CheckCircle2, Zap, Shield, Sparkles } from 'lucide-react';
import { RazorpayCheckout } from '@/components/billing/razorpay-checkout';
import { Switch } from '@/components/ui/switch';
import { Label } from '@/components/ui/label';
import { cn } from '@/lib/utils';

export default function PricingPage() {
  const [isYearly, setIsYearly] = useState(false);
  
  const { data: plans, isLoading } = useQuery({
    queryKey: ['plans'],
    queryFn: billingService.getPlans,
  });

  if (isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-zinc-950 p-8">
        <div className="flex gap-8 flex-wrap justify-center">
          {[1, 2, 3].map(i => (
            <div key={i} className="w-[350px] h-[600px] rounded-3xl bg-zinc-900/50 border border-zinc-800 animate-pulse" />
          ))}
        </div>
      </div>
    );
  }

  return (
    <div className="relative min-h-screen bg-zinc-950 text-zinc-50 overflow-hidden selection:bg-primary/30">
      {/* Background Effects */}
      <div className="absolute inset-0 bg-[linear-gradient(to_right,#4f4f4f2e_1px,transparent_1px),linear-gradient(to_bottom,#4f4f4f2e_1px,transparent_1px)] bg-[size:14px_24px] [mask-image:radial-gradient(ellipse_60%_50%_at_50%_0%,#000_70%,transparent_100%)]"></div>
      <div className="absolute left-1/2 top-0 -translate-x-1/2 -translate-y-1/2 h-[500px] w-[1000px] rounded-full bg-primary/20 opacity-50 blur-[120px]"></div>

      <div className="relative z-10 py-24 px-6 sm:py-32 lg:px-8">
        {/* Header Section */}
        <div className="mx-auto max-w-4xl text-center mb-20 space-y-8">
          <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-primary/10 border border-primary/20 text-primary text-sm font-medium mb-4">
            <Sparkles className="w-4 h-4" />
            <span>Pricing plans for every stage</span>
          </div>
          <h1 className="text-5xl md:text-7xl font-extrabold tracking-tight bg-clip-text text-transparent bg-gradient-to-br from-zinc-100 to-zinc-500">
            Simple, transparent pricing
          </h1>
          <p className="text-xl text-zinc-400 max-w-2xl mx-auto leading-relaxed">
            Choose the perfect plan for your business needs. Upgrade or downgrade at any time with zero hidden fees.
          </p>
          
          {/* Toggle */}
          <div className="flex items-center justify-center gap-4 pt-8">
            <Label htmlFor="billing-cycle" className={cn("text-base cursor-pointer transition-colors", !isYearly ? 'text-zinc-100 font-semibold' : 'text-zinc-500')}>Monthly</Label>
            <Switch 
              id="billing-cycle" 
              checked={isYearly} 
              onCheckedChange={setIsYearly} 
              className="data-[state=checked]:bg-primary"
            />
            <Label htmlFor="billing-cycle" className={cn("flex items-center gap-2 text-base cursor-pointer transition-colors", isYearly ? 'text-zinc-100 font-semibold' : 'text-zinc-500')}>
              Yearly 
              <span className="text-[10px] uppercase font-bold tracking-wider bg-primary/20 text-primary px-2.5 py-1 rounded-full">Save 20%</span>
            </Label>
          </div>
        </div>

        {/* Pricing Cards */}
        <div className="mx-auto max-w-7xl grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 items-center">
          {plans?.map((plan) => (
            <Card 
              key={plan.id} 
              className={cn(
                "relative flex flex-col h-full bg-zinc-900/50 backdrop-blur-xl transition-all duration-500 hover:-translate-y-2 hover:shadow-2xl overflow-hidden rounded-3xl",
                plan.isPopular 
                  ? "border-primary/50 shadow-primary/20 shadow-2xl lg:scale-105 z-10 bg-zinc-900/80" 
                  : "border-zinc-800 hover:border-zinc-700"
              )}
            >
              {plan.isPopular && (
                <div className="absolute top-0 inset-x-0 h-1 bg-gradient-to-r from-primary/50 via-primary to-primary/50"></div>
              )}
              {plan.isPopular && (
                <div className="absolute top-5 right-5 bg-primary/10 border border-primary/30 text-primary px-3 py-1 rounded-full text-xs font-bold tracking-widest uppercase flex items-center gap-1">
                  <Zap className="w-3 h-3 fill-primary" /> Popular
                </div>
              )}
              
              <CardHeader className="p-8 pb-4">
                <CardTitle className="text-2xl font-bold text-zinc-100 mb-2">{plan.name}</CardTitle>
                <CardDescription className="text-zinc-400 min-h-[40px] text-sm leading-relaxed">{plan.description || "Everything you need to get started."}</CardDescription>
                
                <div className="pt-6 flex items-baseline gap-2">
                  <span className="text-5xl font-extrabold text-zinc-100 tracking-tight">
                    ${isYearly ? (plan.yearlyPrice / 12).toFixed(0) : plan.monthlyPrice}
                  </span>
                  <span className="text-zinc-500 font-medium">/mo</span>
                </div>
                
                <div className="min-h-[24px] mt-2">
                  {isYearly && plan.yearlyPrice > 0 && (
                    <span className="text-sm text-primary font-medium bg-primary/10 px-2 py-1 rounded-md">
                      Billed ${plan.yearlyPrice} yearly
                    </span>
                  )}
                </div>
              </CardHeader>
              
              <CardContent className="p-8 pt-4 flex-grow">
                <div className="space-y-6">
                  {/* Features */}
                  <div>
                    <h4 className="text-sm font-semibold text-zinc-100 mb-4 flex items-center gap-2">
                      <Shield className="w-4 h-4 text-zinc-500" /> Core Features
                    </h4>
                    <ul className="space-y-4 text-sm text-zinc-300">
                      {plan.features.map((feature, idx) => (
                        <li key={idx} className="flex items-start gap-3">
                          <CheckCircle2 className="w-5 h-5 text-primary shrink-0 drop-shadow-[0_0_8px_rgba(var(--primary),0.5)]" />
                          <span className="leading-tight">{feature}</span>
                        </li>
                      ))}
                    </ul>
                  </div>

                  <div className="h-px bg-gradient-to-r from-transparent via-zinc-800 to-transparent my-6"></div>

                  {/* Limits */}
                  <div>
                    <h4 className="text-sm font-semibold text-zinc-100 mb-4">Plan Limits</h4>
                    <ul className="space-y-3 text-sm text-zinc-400">
                      <li className="flex justify-between border-b border-zinc-800/50 pb-2">
                        <span>Dynamic QR Codes</span>
                        <span className="font-medium text-zinc-200">{plan.limits.qrs.toLocaleString()}</span>
                      </li>
                      <li className="flex justify-between border-b border-zinc-800/50 pb-2">
                        <span>Monthly Scans</span>
                        <span className="font-medium text-zinc-200">{plan.limits.scans.toLocaleString()}</span>
                      </li>
                      <li className="flex justify-between pb-2">
                        <span>AI Credits</span>
                        <span className="font-medium text-zinc-200">{plan.limits.aiCredits.toLocaleString()}</span>
                      </li>
                    </ul>
                  </div>
                </div>
              </CardContent>
              
              <CardFooter className="p-8 pt-0">
                {plan.monthlyPrice === 0 ? (
                  <Button variant="outline" className="w-full h-12 rounded-xl border-zinc-700 hover:bg-zinc-800 hover:text-zinc-100 transition-all font-semibold">
                    Get Started Free
                  </Button>
                ) : (
                  <RazorpayCheckout 
                    planId={plan.id}
                    planName={plan.name}
                    amount={isYearly ? plan.yearlyPrice : plan.monthlyPrice}
                    billingCycle={isYearly ? 'yearly' : 'monthly'}
                    className={cn(
                      "w-full h-12 rounded-xl font-semibold transition-all shadow-lg",
                      plan.isPopular 
                        ? "bg-primary hover:bg-primary/90 text-primary-foreground shadow-primary/25" 
                        : "bg-zinc-100 text-zinc-900 hover:bg-zinc-200"
                    )}
                  />
                )}
              </CardFooter>
            </Card>
          ))}
        </div>
      </div>
    </div>
  );
}
