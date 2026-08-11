"use client";

import React, { use } from "react";
import { Customer360Header } from "@/components/customer-success/customer-360-header";
import { useCustomerHealthByCustomer, useCustomerLoyalty } from "@/hooks/use-customer-success";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import {
  Send,
  Sparkles,
  Bot,
  User,
  Copy,
  RefreshCw,
  Plus,
  Trash2,
  Check,
  AlertCircle,
  Command as CommandIcon,
  ChevronRight,
  MessageSquare,
  Search,
  BrainCircuit,
  Briefcase,
  QrCode,
  Award,
  Clock,
  DollarSign,
  CheckCircle2,
  Star,
} from "lucide-react";
import { CustomerAiContext } from "@/components/customer-success/CustomerAiContext";

interface PageProps {
  params: Promise<{ id: string }>;
}

export default function Customer360Page({ params }: PageProps) {
  const resolvedParams = use(params);
  const customerId = resolvedParams.id;

  const { data: health, isLoading: healthLoading } = useCustomerHealthByCustomer(customerId);
  const { data: loyalty } = useCustomerLoyalty(customerId);

  const mockCustomer = {
    id: customerId,
    name: health?.customerName ?? "Acme Global Solutions",
    email: "contact@acmeglobal.com",
    phone: "+1 (555) 234-5678",
    company: "Acme Global Corp",
    status: "Active",
    healthScore: health?.healthScore ?? 88,
    riskLevel: health?.riskLevel ?? "Healthy",
    lifetimeValue: health?.lifetimeValue ?? 12450,
    totalOrders: health?.purchaseFrequency ?? 14,
    outstandingBalance: health?.outstandingPayments ?? 0,
    loyaltyPoints: loyalty?.totalBalance ?? 850,
  };

  return (
    <div className="space-y-6 p-6 max-w-7xl mx-auto">
      {/* 360 Header Component */}
      <Customer360Header
        id={mockCustomer.id}
        name={mockCustomer.name}
        email={mockCustomer.email}
        phone={mockCustomer.phone}
        company={mockCustomer.company}
        status={mockCustomer.status}
        healthScore={mockCustomer.healthScore}
        riskLevel={mockCustomer.riskLevel}
        lifetimeValue={mockCustomer.lifetimeValue}
        totalOrders={mockCustomer.totalOrders}
        outstandingBalance={mockCustomer.outstandingBalance}
        loyaltyPoints={mockCustomer.loyaltyPoints}
      />

      {/* Main Tabbed Workspaces */}
      <Tabs defaultValue="overview" className="space-y-6">
        <TabsList className="bg-slate-100 dark:bg-slate-900 p-1 border border-slate-200 dark:border-slate-800">
          <TabsTrigger value="overview" className="gap-2">
            <User className="h-4 w-4" /> Overview
          </TabsTrigger>
          <TabsTrigger value="crm" className="gap-2">
            <Briefcase className="h-4 w-4" /> CRM & Deals
          </TabsTrigger>
          <TabsTrigger value="marketing" className="gap-2">
            <QrCode className="h-4 w-4" /> Marketing & QR
          </TabsTrigger>
          <TabsTrigger value="loyalty" className="gap-2">
            <Award className="h-4 w-4" /> Loyalty & Rewards
          </TabsTrigger>
          <TabsTrigger value="timeline" className="gap-2">
            <Clock className="h-4 w-4" /> Activity Timeline
          </TabsTrigger>
          <TabsTrigger value="ai-context" className="gap-2 text-indigo-600 data-[state=active]:text-indigo-600">
            <BrainCircuit className="h-4 w-4" /> AI Context
          </TabsTrigger>
        </TabsList>

        {/* Tab 1: Overview */}
        <TabsContent value="overview" className="space-y-6">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            <Card className="border-slate-200 dark:border-slate-800">
              <CardHeader>
                <CardTitle className="text-base flex items-center gap-2">
                  <User className="h-4 w-4 text-purple-500" /> Customer Information
                </CardTitle>
              </CardHeader>
              <CardContent className="space-y-3 text-sm">
                <div className="flex justify-between border-b border-slate-100 dark:border-slate-800 pb-2">
                  <span className="text-slate-500">Contact Person</span>
                  <span className="font-medium text-slate-900 dark:text-white">Alex Morgan</span>
                </div>
                <div className="flex justify-between border-b border-slate-100 dark:border-slate-800 pb-2">
                  <span className="text-slate-500">Industry</span>
                  <span className="font-medium text-slate-900 dark:text-white">Software / SaaS</span>
                </div>
                <div className="flex justify-between border-b border-slate-100 dark:border-slate-800 pb-2">
                  <span className="text-slate-500">Tier</span>
                  <Badge variant="outline" className="border-purple-500/30 bg-purple-500/10 text-purple-600">VIP Customer</Badge>
                </div>
                <div className="flex justify-between">
                  <span className="text-slate-500">Account Owner</span>
                  <span className="font-medium text-slate-900 dark:text-white">Sarah Jenkins</span>
                </div>
              </CardContent>
            </Card>

            <Card className="border-slate-200 dark:border-slate-800">
              <CardHeader>
                <CardTitle className="text-base flex items-center gap-2">
                  <DollarSign className="h-4 w-4 text-emerald-500" /> Financial Summary
                </CardTitle>
              </CardHeader>
              <CardContent className="space-y-3 text-sm">
                <div className="flex justify-between border-b border-slate-100 dark:border-slate-800 pb-2">
                  <span className="text-slate-500">Total Spent</span>
                  <span className="font-bold text-emerald-600 dark:text-emerald-400">${mockCustomer.lifetimeValue.toLocaleString()}</span>
                </div>
                <div className="flex justify-between border-b border-slate-100 dark:border-slate-800 pb-2">
                  <span className="text-slate-500">Avg Order Value</span>
                  <span className="font-medium text-slate-900 dark:text-white">${Math.round(mockCustomer.lifetimeValue / (mockCustomer.totalOrders || 1))}</span>
                </div>
                <div className="flex justify-between border-b border-slate-100 dark:border-slate-800 pb-2">
                  <span className="text-slate-500">Outstanding Invoices</span>
                  <span className="font-medium text-slate-900 dark:text-white">${mockCustomer.outstandingBalance}</span>
                </div>
                <div className="flex justify-between">
                  <span className="text-slate-500">Payment Status</span>
                  <span className="font-medium text-emerald-500 flex items-center gap-1">
                    <CheckCircle2 className="h-3.5 w-3.5" /> Up to Date
                  </span>
                </div>
              </CardContent>
            </Card>

            <Card className="border-slate-200 dark:border-slate-800">
              <CardHeader>
                <CardTitle className="text-base flex items-center gap-2">
                  <Star className="h-4 w-4 text-amber-500" /> Satisfaction & Support
                </CardTitle>
              </CardHeader>
              <CardContent className="space-y-3 text-sm">
                <div className="flex justify-between border-b border-slate-100 dark:border-slate-800 pb-2">
                  <span className="text-slate-500">CSAT Score</span>
                  <span className="font-bold text-amber-500">5.0 / 5.0</span>
                </div>
                <div className="flex justify-between border-b border-slate-100 dark:border-slate-800 pb-2">
                  <span className="text-slate-500">Support Tickets</span>
                  <span className="font-medium text-slate-900 dark:text-white">{health?.supportTicketCount ?? 0} tickets</span>
                </div>
                <div className="flex justify-between border-b border-slate-100 dark:border-slate-800 pb-2">
                  <span className="text-slate-500">Referrals Provided</span>
                  <span className="font-medium text-purple-600 dark:text-purple-400">{health?.referralCount ?? 2} successful</span>
                </div>
                <div className="flex justify-between">
                  <span className="text-slate-500">Last CSAT Feedback</span>
                  <span className="font-medium text-slate-900 dark:text-white text-xs">"Outstanding service!"</span>
                </div>
              </CardContent>
            </Card>
          </div>
        </TabsContent>

        {/* Tab 2: CRM & Deals */}
        <TabsContent value="crm" className="space-y-4">
          <Card className="border-slate-200 dark:border-slate-800">
            <CardHeader>
              <CardTitle className="text-base">Deals & Opportunities</CardTitle>
              <CardDescription>Active sales pipeline deals for this account</CardDescription>
            </CardHeader>
            <CardContent>
              <div className="space-y-3">
                <div className="p-4 rounded-lg bg-slate-50 dark:bg-slate-900 border border-slate-200 dark:border-slate-800 flex items-center justify-between">
                  <div>
                    <p className="font-semibold text-slate-900 dark:text-white">Annual Enterprise Plan Expansion</p>
                    <p className="text-xs text-slate-500">Stage: Negotiation • Expected Close: Next Month</p>
                  </div>
                  <span className="font-bold font-mono text-emerald-600 dark:text-emerald-400">$18,000</span>
                </div>
                <div className="p-4 rounded-lg bg-slate-50 dark:bg-slate-900 border border-slate-200 dark:border-slate-800 flex items-center justify-between">
                  <div>
                    <p className="font-semibold text-slate-900 dark:text-white">Custom Integration Add-on</p>
                    <p className="text-xs text-slate-500">Stage: Closed Won • Date: 2 months ago</p>
                  </div>
                  <span className="font-bold font-mono text-slate-600 dark:text-slate-400">$4,500</span>
                </div>
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Tab 3: Marketing & QR */}
        <TabsContent value="marketing" className="space-y-4">
          <Card className="border-slate-200 dark:border-slate-800">
            <CardHeader>
              <CardTitle className="text-base">Marketing Campaigns & Interactions</CardTitle>
              <CardDescription>Email marketing engagement and QR campaign scans</CardDescription>
            </CardHeader>
            <CardContent className="space-y-3 text-sm">
              <div className="flex items-center justify-between p-3 rounded-lg bg-slate-50 dark:bg-slate-900">
                <span className="font-medium text-slate-900 dark:text-white">Summer VIP Loyalty Promo Email</span>
                <Badge variant="outline" className="text-emerald-600 border-emerald-500/30">Opened & Clicked</Badge>
              </div>
              <div className="flex items-center justify-between p-3 rounded-lg bg-slate-50 dark:bg-slate-900">
                <span className="font-medium text-slate-900 dark:text-white">Storefront QR Scan — Loyalty Signup</span>
                <Badge variant="outline" className="text-blue-600 border-blue-500/30">Scanned (3 times)</Badge>
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Tab 4: Loyalty & Rewards */}
        <TabsContent value="loyalty" className="space-y-4">
          <Card className="border-slate-200 dark:border-slate-800">
            <CardHeader>
              <CardTitle className="text-base">Loyalty Program Transactions</CardTitle>
            </CardHeader>
            <CardContent className="space-y-3">
              {loyalty?.recentTransactions.length ? (
                loyalty.recentTransactions.map((tx) => (
                  <div key={tx.id} className="flex items-center justify-between p-3 rounded-lg border border-slate-200 dark:border-slate-800 text-sm">
                    <div>
                      <p className="font-semibold text-slate-900 dark:text-white">{tx.description || tx.transactionType}</p>
                      <p className="text-xs text-slate-500">{new Date(tx.createdAt).toLocaleDateString()}</p>
                    </div>
                    <span className={`font-bold font-mono ${tx.pointsEarned > 0 ? "text-emerald-600" : "text-rose-600"}`}>
                      {tx.pointsEarned > 0 ? `+${tx.pointsEarned}` : `-${tx.pointsRedeemed}`} pts
                    </span>
                  </div>
                ))
              ) : (
                <div className="text-center py-6 text-slate-500 text-sm">No loyalty transactions recorded yet.</div>
              )}
            </CardContent>
          </Card>
        </TabsContent>

        {/* Tab 5: Activity Timeline */}
        <TabsContent value="timeline" className="space-y-4">
          <Card className="border-slate-200 dark:border-slate-800">
            <CardHeader>
              <CardTitle className="text-base">Unified Customer Activity Log</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="relative border-l border-slate-200 dark:border-slate-800 ml-4 space-y-6 pl-6 py-2">
                <div className="relative">
                  <div className="absolute -left-[31px] top-1.5 h-3.5 w-3.5 rounded-full bg-emerald-500 ring-4 ring-background" />
                  <p className="text-sm font-semibold text-slate-900 dark:text-white">Purchase Completed ($1,250)</p>
                  <p className="text-xs text-slate-500">2 days ago • Store Checkout</p>
                </div>
                <div className="relative">
                  <div className="absolute -left-[31px] top-1.5 h-3.5 w-3.5 rounded-full bg-purple-500 ring-4 ring-background" />
                  <p className="text-sm font-semibold text-slate-900 dark:text-white">Loyalty Milestone Reached (850 Points)</p>
                  <p className="text-xs text-slate-500">5 days ago • Auto-Triggered</p>
                </div>
                <div className="relative">
                  <div className="absolute -left-[31px] top-1.5 h-3.5 w-3.5 rounded-full bg-blue-500 ring-4 ring-background" />
                  <p className="text-sm font-semibold text-slate-900 dark:text-white">Submitted 5-Star CSAT Rating</p>
                  <p className="text-xs text-slate-500">1 week ago • Web Portal</p>
                </div>
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Tab 6: AI Context */}
        <TabsContent value="ai-context" className="space-y-4 animate-in fade-in slide-in-from-bottom-4 duration-300">
          <CustomerAiContext customerId={customerId} />
        </TabsContent>
      </Tabs>
    </div>
  );
}
