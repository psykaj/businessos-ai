import React from "react";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { HealthScoreBadge } from "./health-score-badge";
import { Mail, Phone, Building2, Calendar, DollarSign, Award, Gift, ArrowLeft } from "lucide-react";
import Link from "next/link";

interface Customer360HeaderProps {
  id: string;
  name: string;
  email: string;
  phone?: string;
  company?: string;
  status?: string;
  healthScore?: number;
  riskLevel?: string;
  lifetimeValue: number;
  totalOrders: number;
  outstandingBalance: number;
  lastPurchaseDate?: string;
  loyaltyPoints?: number;
}

export function Customer360Header({
  id,
  name,
  email,
  phone = "+1 (555) 019-2834",
  company = "Acme Corp",
  status = "Active",
  healthScore = 85,
  riskLevel = "Healthy",
  lifetimeValue,
  totalOrders,
  outstandingBalance,
  lastPurchaseDate,
  loyaltyPoints = 450,
}: Customer360HeaderProps) {
  const initials = name
    .split(" ")
    .map((n) => n[0])
    .join("")
    .substring(0, 2)
    .toUpperCase();

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <Link href="/dashboard/customer-health">
          <Button variant="ghost" size="sm" className="gap-2 text-slate-500 hover:text-slate-900 dark:hover:text-white">
            <ArrowLeft className="h-4 w-4" /> Back to Customers
          </Button>
        </Link>
        <div className="flex items-center gap-2">
          <Button variant="outline" size="sm" className="gap-2">
            <Gift className="h-4 w-4 text-purple-500" /> Award Points
          </Button>
          <Button size="sm" className="gap-2 bg-gradient-to-r from-purple-600 to-indigo-600 text-white">
            <Mail className="h-4 w-4" /> Send Engagement
          </Button>
        </div>
      </div>

      <Card className="border-slate-200 dark:border-slate-800 bg-gradient-to-br from-slate-900/5 via-background to-purple-500/5">
        <CardContent className="p-6">
          <div className="flex flex-col lg:flex-row lg:items-center justify-between gap-6">
            <div className="flex items-start gap-4">
              <Avatar className="h-16 w-16 border-2 border-purple-500/20 shadow-md">
                <AvatarFallback className="bg-gradient-to-br from-purple-600 to-indigo-600 text-white font-bold text-lg">
                  {initials}
                </AvatarFallback>
              </Avatar>
              <div className="space-y-1.5">
                <div className="flex items-center gap-3 flex-wrap">
                  <h1 className="text-2xl font-bold text-slate-900 dark:text-white tracking-tight">{name}</h1>
                  <HealthScoreBadge score={healthScore} riskLevel={riskLevel} />
                </div>
                <div className="flex flex-wrap items-center gap-y-1 gap-x-4 text-sm text-slate-500 dark:text-slate-400">
                  <span className="flex items-center gap-1">
                    <Mail className="h-3.5 w-3.5 text-slate-400" /> {email}
                  </span>
                  <span className="flex items-center gap-1">
                    <Phone className="h-3.5 w-3.5 text-slate-400" /> {phone}
                  </span>
                  <span className="flex items-center gap-1">
                    <Building2 className="h-3.5 w-3.5 text-slate-400" /> {company}
                  </span>
                </div>
              </div>
            </div>

            <div className="grid grid-cols-2 sm:grid-cols-4 gap-4 border-t lg:border-t-0 lg:border-l border-slate-200 dark:border-slate-800 lg:pl-6 pt-4 lg:pt-0">
              <div className="space-y-1">
                <p className="text-xs font-medium text-slate-500 dark:text-slate-400 uppercase tracking-wider">Lifetime Value</p>
                <p className="text-lg font-bold text-emerald-600 dark:text-emerald-400">${lifetimeValue.toLocaleString()}</p>
              </div>
              <div className="space-y-1">
                <p className="text-xs font-medium text-slate-500 dark:text-slate-400 uppercase tracking-wider">Total Orders</p>
                <p className="text-lg font-bold text-slate-900 dark:text-white">{totalOrders}</p>
              </div>
              <div className="space-y-1">
                <p className="text-xs font-medium text-slate-500 dark:text-slate-400 uppercase tracking-wider">Outstanding</p>
                <p className={`text-lg font-bold ${outstandingBalance > 0 ? "text-rose-600 dark:text-rose-400" : "text-slate-900 dark:text-white"}`}>
                  ${outstandingBalance.toLocaleString()}
                </p>
              </div>
              <div className="space-y-1">
                <p className="text-xs font-medium text-slate-500 dark:text-slate-400 uppercase tracking-wider">Loyalty Points</p>
                <p className="text-lg font-bold text-purple-600 dark:text-purple-400 flex items-center gap-1">
                  <Award className="h-4 w-4" /> {loyaltyPoints}
                </p>
              </div>
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
