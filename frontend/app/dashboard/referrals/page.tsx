"use client";

import React, { useState } from "react";
import {
  useReferralAnalytics,
  useReferralsPaged,
  useCreateReferral,
  useConvertReferral,
} from "@/hooks/use-customer-success";
import { ReferralFunnelChart } from "@/components/customer-success/referral-funnel-chart";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger, DialogFooter } from "@/components/ui/dialog";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { Users, UserPlus, Gift, DollarSign, Share2, Copy, CheckCircle2, TrendingUp } from "lucide-react";
import { toast } from "sonner";

export default function ReferralDashboardPage() {
  const { data: analytics, isLoading: analyticsLoading } = useReferralAnalytics();
  const { data: referralsData } = useReferralsPaged({ pageSize: 15 });

  // Dialog States
  const [createRefOpen, setCreateRefOpen] = useState(false);
  const [convertRefOpen, setConvertRefOpen] = useState(false);

  // Form States
  const [referrerId, setReferrerId] = useState("");
  const [rewardAmt, setRewardAmt] = useState(50);
  const [notes, setNotes] = useState("");

  const [refCode, setRefCode] = useState("");
  const [referredId, setReferredId] = useState("");

  const createRefMutation = useCreateReferral();
  const convertRefMutation = useConvertReferral();

  const handleCreateReferral = (e: React.FormEvent) => {
    e.preventDefault();
    if (!referrerId) return;
    createRefMutation.mutate(
      {
        referrerCustomerId: referrerId,
        rewardAmount: rewardAmt,
        notes,
      },
      {
        onSuccess: () => {
          setCreateRefOpen(false);
          setReferrerId("");
        },
      }
    );
  };

  const handleConvertReferral = (e: React.FormEvent) => {
    e.preventDefault();
    if (!refCode || !referredId) return;
    convertRefMutation.mutate(
      {
        referralCode: refCode,
        referredCustomerId: referredId,
      },
      {
        onSuccess: () => {
          setConvertRefOpen(false);
          setRefCode("");
          setReferredId("");
        },
      }
    );
  };

  const copyToClipboard = (text: string) => {
    navigator.clipboard.writeText(text);
    toast.success("Referral code copied to clipboard!");
  };

  return (
    <div className="space-y-6 p-6 max-w-7xl mx-auto">
      {/* Top Banner */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-slate-200 dark:border-slate-800 pb-6">
        <div>
          <h1 className="text-3xl font-bold text-slate-900 dark:text-white tracking-tight flex items-center gap-3">
            <Users className="h-8 w-8 text-emerald-600" />
            Customer Referral Program
          </h1>
          <p className="text-slate-500 dark:text-slate-400 mt-1">
            Turn happy customers into brand advocates. Generate referral links, track conversion funnels, and disburse rewards.
          </p>
        </div>
        <div className="flex items-center gap-3">
          <Button variant="outline" size="sm" onClick={() => setConvertRefOpen(true)} className="gap-2">
            <UserPlus className="h-4 w-4 text-indigo-500" /> Convert Code
          </Button>

          {/* Dialog: Generate Referral Code */}
          <Dialog open={createRefOpen} onOpenChange={setCreateRefOpen}>
            <DialogTrigger
              render={
                <Button size="sm" className="gap-2 bg-gradient-to-r from-emerald-600 to-teal-600 text-white">
                  <Share2 className="h-4 w-4" /> Generate Referral Code
                </Button>
              }
            />

            <DialogContent className="sm:max-w-[440px]">
              <DialogHeader>
                <DialogTitle>Generate Customer Referral Code</DialogTitle>
              </DialogHeader>
              <form onSubmit={handleCreateReferral} className="space-y-4 py-2">
                <div className="space-y-2">
                  <Label htmlFor="refId">Referrer Customer ID (Guid)</Label>
                  <Input
                    id="refId"
                    placeholder="Paste Referrer Customer Guid..."
                    value={referrerId}
                    onChange={(e) => setReferrerId(e.target.value)}
                    required
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="reward">Reward Amount ($ or Points)</Label>
                  <Input
                    id="reward"
                    type="number"
                    value={rewardAmt}
                    onChange={(e) => setRewardAmt(parseFloat(e.target.value) || 0)}
                    required
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="notes">Notes / Campaign</Label>
                  <Input
                    id="notes"
                    placeholder="e.g. Summer VIP Advocacy Promo"
                    value={notes}
                    onChange={(e) => setNotes(e.target.value)}
                  />
                </div>
                <DialogFooter className="pt-2">
                  <Button type="submit" disabled={createRefMutation.isPending} className="w-full bg-emerald-600 text-white">
                    Generate Code
                  </Button>
                </DialogFooter>
              </form>
            </DialogContent>
          </Dialog>

          {/* Dialog: Convert Referral */}
          <Dialog open={convertRefOpen} onOpenChange={setConvertRefOpen}>
            <DialogContent className="sm:max-w-[440px]">
              <DialogHeader>
                <DialogTitle>Convert Referral Code</DialogTitle>
              </DialogHeader>
              <form onSubmit={handleConvertReferral} className="space-y-4 py-2">
                <div className="space-y-2">
                  <Label htmlFor="codeVal">Referral Code</Label>
                  <Input
                    id="codeVal"
                    placeholder="e.g. REF-4A2F-9812AB"
                    value={refCode}
                    onChange={(e) => setRefCode(e.target.value)}
                    required
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="refedId">Referred Customer ID (Guid)</Label>
                  <Input
                    id="refedId"
                    placeholder="Paste Referred Customer Guid..."
                    value={referredId}
                    onChange={(e) => setReferredId(e.target.value)}
                    required
                  />
                </div>
                <DialogFooter className="pt-2">
                  <Button type="submit" disabled={convertRefMutation.isPending} className="w-full bg-indigo-600 text-white">
                    Confirm Conversion & Credit Reward
                  </Button>
                </DialogFooter>
              </form>
            </DialogContent>
          </Dialog>
        </div>
      </div>

      {/* Analytics KPI Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <Card className="border-slate-200 dark:border-slate-800">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-slate-500 uppercase tracking-wider">Total Referrals</span>
              <Users className="h-5 w-5 text-emerald-600" />
            </div>
            <div className="mt-3">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">
                {analytics?.totalReferrals ?? 0}
              </span>
            </div>
            <p className="text-xs text-slate-500 mt-1">Codes generated across accounts</p>
          </CardContent>
        </Card>

        <Card className="border-slate-200 dark:border-slate-800">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-indigo-600 dark:text-indigo-400 uppercase tracking-wider">Conversion Rate</span>
              <TrendingUp className="h-5 w-5 text-indigo-500" />
            </div>
            <div className="mt-3">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">
                {analytics?.conversionRatePercentage ?? 0}%
              </span>
            </div>
            <p className="text-xs text-slate-500 mt-1">Successful customer conversions</p>
          </CardContent>
        </Card>

        <Card className="border-slate-200 dark:border-slate-800">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-purple-600 dark:text-purple-400 uppercase tracking-wider">Rewards Issued</span>
              <Gift className="h-5 w-5 text-purple-500" />
            </div>
            <div className="mt-3">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">
                ${analytics?.totalRewardsIssued ?? 0}
              </span>
            </div>
            <p className="text-xs text-slate-500 mt-1">Total referral rewards paid</p>
          </CardContent>
        </Card>

        <Card className="border-slate-200 dark:border-slate-800">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-amber-600 dark:text-amber-400 uppercase tracking-wider">Pending Rewards</span>
              <DollarSign className="h-5 w-5 text-amber-500" />
            </div>
            <div className="mt-3">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">
                {analytics?.pendingCount ?? 0}
              </span>
            </div>
            <p className="text-xs text-slate-500 mt-1">Awaiting customer signup</p>
          </CardContent>
        </Card>
      </div>

      {/* Funnel Chart & Top Referrers */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <Card className="lg:col-span-2 border-slate-200 dark:border-slate-800">
          <CardHeader>
            <CardTitle className="text-base">Referral Acquisition Funnel</CardTitle>
            <CardDescription>Visual conversion pipeline from shared link to customer reward</CardDescription>
          </CardHeader>
          <CardContent>
            <ReferralFunnelChart
              total={analytics?.totalReferrals ?? 0}
              pending={analytics?.pendingCount ?? 0}
              converted={analytics?.convertedCount ?? 0}
              rewarded={analytics?.rewardedCount ?? 0}
            />
          </CardContent>
        </Card>

        <Card className="border-slate-200 dark:border-slate-800">
          <CardHeader>
            <CardTitle className="text-base">Top Customer Advocates</CardTitle>
            <CardDescription>Highest converting referrers</CardDescription>
          </CardHeader>
          <CardContent className="space-y-3">
            <div className="p-3 rounded-lg border border-slate-200 dark:border-slate-800 flex items-center justify-between">
              <div>
                <p className="font-semibold text-slate-900 dark:text-white text-sm">Acme Corp</p>
                <p className="text-xs text-slate-500">5 Conversions ($250 rewarded)</p>
              </div>
              <Badge variant="outline" className="bg-emerald-500/10 text-emerald-600 border-emerald-500/30">Top Referrer</Badge>
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Referrals List Table */}
      <Card className="border-slate-200 dark:border-slate-800">
        <CardHeader>
          <CardTitle className="text-base">Referrals Tracker</CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow className="border-slate-200 dark:border-slate-800">
                <TableHead>Code</TableHead>
                <TableHead>Referrer</TableHead>
                <TableHead>Referred Customer</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Reward</TableHead>
                <TableHead className="text-right">Action</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {referralsData?.items.length ? (
                referralsData.items.map((ref) => (
                  <TableRow key={ref.id} className="border-slate-100 dark:border-slate-800/60">
                    <TableCell className="font-mono font-bold text-slate-900 dark:text-white flex items-center gap-2">
                      {ref.referralCode}
                      <Button variant="ghost" size="icon" className="h-6 w-6" onClick={() => copyToClipboard(ref.referralCode)}>
                        <Copy className="h-3.5 w-3.5 text-slate-400" />
                      </Button>
                    </TableCell>
                    <TableCell className="font-medium text-slate-900 dark:text-white">{ref.referrerName}</TableCell>
                    <TableCell className="text-xs text-slate-500">{ref.referredName || "Pending Signup"}</TableCell>
                    <TableCell>
                      <Badge
                        variant="outline"
                        className={
                          ref.status === "Rewarded"
                            ? "bg-emerald-500/10 text-emerald-600 border-emerald-500/30"
                            : ref.status === "Converted"
                            ? "bg-indigo-500/10 text-indigo-600 border-indigo-500/30"
                            : "bg-amber-500/10 text-amber-600 border-amber-500/30"
                        }
                      >
                        {ref.status}
                      </Badge>
                    </TableCell>
                    <TableCell className="font-mono font-medium">${ref.rewardAmount}</TableCell>
                    <TableCell className="text-right">
                      {ref.status === "Pending" && (
                        <Button
                          variant="ghost"
                          size="sm"
                          className="text-xs text-indigo-600"
                          onClick={() => {
                            setRefCode(ref.referralCode);
                            setConvertRefOpen(true);
                          }}
                        >
                          Convert & Credit
                        </Button>
                      )}
                    </TableCell>
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell colSpan={6} className="text-center py-6 text-slate-500 text-sm">
                    No referral records generated yet.
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  );
}
