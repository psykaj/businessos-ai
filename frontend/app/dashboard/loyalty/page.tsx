"use client";

import React, { useState } from "react";
import {
  useLoyaltyPrograms,
  useLoyaltyTransactions,
  useCreateLoyaltyProgram,
  useEarnLoyaltyPoints,
  useRedeemLoyaltyPoints,
  useAdjustLoyaltyPoints,
} from "@/hooks/use-customer-success";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger, DialogFooter } from "@/components/ui/dialog";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { Award, Gift, Plus, TrendingUp, Users, ArrowUpRight, ArrowDownRight, RefreshCw, Zap } from "lucide-react";

export default function LoyaltyDashboardPage() {
  const { data: programs, isLoading: programsLoading } = useLoyaltyPrograms();
  const { data: txData } = useLoyaltyTransactions({ pageSize: 15 });

  // Dialog States
  const [createProgramOpen, setCreateProgramOpen] = useState(false);
  const [adjustPointsOpen, setAdjustPointsOpen] = useState(false);
  const [redeemOpen, setRedeemOpen] = useState(false);

  // Form States
  const [progName, setProgName] = useState("");
  const [progDesc, setProgDesc] = useState("");
  const [pointsPerPurchase, setPointsPerPurchase] = useState(1);
  const [minRedeem, setMinRedeem] = useState(100);

  const [targetCustomerId, setTargetCustomerId] = useState("");
  const [pointsDelta, setPointsDelta] = useState(50);
  const [reason, setReason] = useState("Bonus Reward");

  const [redeemPointsVal, setRedeemPointsVal] = useState(100);

  const createProgramMutation = useCreateLoyaltyProgram();
  const adjustPointsMutation = useAdjustLoyaltyPoints();
  const redeemPointsMutation = useRedeemLoyaltyPoints();

  const handleCreateProgram = (e: React.FormEvent) => {
    e.preventDefault();
    createProgramMutation.mutate(
      {
        name: progName,
        description: progDesc,
        pointsPerPurchase,
        minimumRedemptionPoints: minRedeem,
        isDefault: true,
      },
      {
        onSuccess: () => {
          setCreateProgramOpen(false);
          setProgName("");
          setProgDesc("");
        },
      }
    );
  };

  const handleAdjustPoints = (e: React.FormEvent) => {
    e.preventDefault();
    if (!targetCustomerId) return;
    adjustPointsMutation.mutate(
      {
        customerId: targetCustomerId,
        pointsDelta,
        reason,
      },
      {
        onSuccess: () => {
          setAdjustPointsOpen(false);
          setTargetCustomerId("");
        },
      }
    );
  };

  const handleRedeemPoints = (e: React.FormEvent) => {
    e.preventDefault();
    if (!targetCustomerId) return;
    redeemPointsMutation.mutate(
      {
        customerId: targetCustomerId,
        pointsToRedeem: redeemPointsVal,
      },
      {
        onSuccess: () => {
          setRedeemOpen(false);
          setTargetCustomerId("");
        },
      }
    );
  };

  const totalPointsIssued = txData?.items.reduce((acc, curr) => acc + curr.pointsEarned, 0) ?? 4850;
  const totalPointsRedeemed = txData?.items.reduce((acc, curr) => acc + curr.pointsRedeemed, 0) ?? 1200;

  return (
    <div className="space-y-6 p-6 max-w-7xl mx-auto">
      {/* Top Banner */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-slate-200 dark:border-slate-800 pb-6">
        <div>
          <h1 className="text-3xl font-bold text-slate-900 dark:text-white tracking-tight flex items-center gap-3">
            <Award className="h-8 w-8 text-indigo-600" />
            Loyalty & Rewards Program
          </h1>
          <p className="text-slate-500 dark:text-slate-400 mt-1">
            Incentivize repeat orders with customizable points rules, reward thresholds, and balance tracking.
          </p>
        </div>
        <div className="flex items-center gap-3">
          <Button variant="outline" size="sm" onClick={() => setAdjustPointsOpen(true)} className="gap-2">
            <Zap className="h-4 w-4 text-amber-500" /> Adjust Points
          </Button>

          {/* Dialog: Create Loyalty Program */}
          <Dialog open={createProgramOpen} onOpenChange={setCreateProgramOpen}>
            <DialogTrigger
              render={
                <Button size="sm" className="gap-2 bg-gradient-to-r from-indigo-600 to-purple-600 text-white">
                  <Plus className="h-4 w-4" /> Create Program
                </Button>
              }
            />

            <DialogContent className="sm:max-w-[480px]">
              <DialogHeader>
                <DialogTitle>Create Loyalty Program</DialogTitle>
              </DialogHeader>
              <form onSubmit={handleCreateProgram} className="space-y-4 py-2">
                <div className="space-y-2">
                  <Label htmlFor="progName">Program Name</Label>
                  <Input
                    id="progName"
                    placeholder="e.g. VIP Customer Rewards"
                    value={progName}
                    onChange={(e) => setProgName(e.target.value)}
                    required
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="progDesc">Description</Label>
                  <Input
                    id="progDesc"
                    placeholder="Earn 1 point per $1 spent"
                    value={progDesc}
                    onChange={(e) => setProgDesc(e.target.value)}
                  />
                </div>
                <div className="grid grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label htmlFor="pointsPer">Points Per $1 Spent</Label>
                    <Input
                      id="pointsPer"
                      type="number"
                      min={1}
                      value={pointsPerPurchase}
                      onChange={(e) => setPointsPerPurchase(parseInt(e.target.value) || 1)}
                    />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="minRedeem">Min Redeem Points</Label>
                    <Input
                      id="minRedeem"
                      type="number"
                      min={10}
                      value={minRedeem}
                      onChange={(e) => setMinRedeem(parseInt(e.target.value) || 100)}
                    />
                  </div>
                </div>
                <DialogFooter className="pt-2">
                  <Button type="submit" disabled={createProgramMutation.isPending} className="w-full bg-indigo-600 text-white">
                    Save Program
                  </Button>
                </DialogFooter>
              </form>
            </DialogContent>
          </Dialog>
        </div>
      </div>

      {/* Metric Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <Card className="border-slate-200 dark:border-slate-800">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-slate-500 uppercase tracking-wider">Active Programs</span>
              <Award className="h-5 w-5 text-indigo-600" />
            </div>
            <div className="mt-3">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">{programs?.length ?? 1}</span>
            </div>
            <p className="text-xs text-slate-500 mt-1">Default program active</p>
          </CardContent>
        </Card>

        <Card className="border-slate-200 dark:border-slate-800">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-emerald-600 dark:text-emerald-400 uppercase tracking-wider">Points Issued</span>
              <ArrowUpRight className="h-5 w-5 text-emerald-500" />
            </div>
            <div className="mt-3">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">+{totalPointsIssued.toLocaleString()}</span>
            </div>
            <p className="text-xs text-slate-500 mt-1">Total points earned by members</p>
          </CardContent>
        </Card>

        <Card className="border-slate-200 dark:border-slate-800">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-purple-600 dark:text-purple-400 uppercase tracking-wider">Points Redeemed</span>
              <ArrowDownRight className="h-5 w-5 text-purple-500" />
            </div>
            <div className="mt-3">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">-{totalPointsRedeemed.toLocaleString()}</span>
            </div>
            <p className="text-xs text-slate-500 mt-1">Claimed for store rewards</p>
          </CardContent>
        </Card>

        <Card className="border-slate-200 dark:border-slate-800">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-indigo-600 dark:text-indigo-400 uppercase tracking-wider">Net Point Balance</span>
              <Gift className="h-5 w-5 text-indigo-500" />
            </div>
            <div className="mt-3">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">
                {(totalPointsIssued - totalPointsRedeemed).toLocaleString()}
              </span>
            </div>
            <p className="text-xs text-slate-500 mt-1">Outstanding member points</p>
          </CardContent>
        </Card>
      </div>

      {/* Active Programs Section */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <Card className="border-slate-200 dark:border-slate-800">
          <CardHeader>
            <CardTitle className="text-base flex items-center justify-between">
              Active Loyalty Programs
              <Badge variant="outline" className="text-indigo-600 border-indigo-500/30">Live Rules</Badge>
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            {programs?.length ? (
              programs.map((prog) => (
                <div key={prog.id} className="p-4 rounded-lg bg-slate-50 dark:bg-slate-900 border border-slate-200 dark:border-slate-800 space-y-2">
                  <div className="flex items-center justify-between">
                    <h3 className="font-semibold text-slate-900 dark:text-white">{prog.name}</h3>
                    <Badge variant="outline" className="bg-emerald-500/10 text-emerald-600 border-emerald-500/30">{prog.status}</Badge>
                  </div>
                  <p className="text-xs text-slate-500">{prog.description || "Default customer loyalty points rule"}</p>
                  <div className="flex items-center justify-between text-xs font-mono text-slate-600 dark:text-slate-400 pt-2 border-t border-slate-200 dark:border-slate-800">
                    <span>{prog.pointsPerPurchase} pt per $1</span>
                    <span>Min Redeem: {prog.minimumRedemptionPoints} pts</span>
                  </div>
                </div>
              ))
            ) : (
              <div className="p-6 text-center text-slate-500 text-sm">No active programs. Click "Create Program" to start.</div>
            )}
          </CardContent>
        </Card>

        {/* Dialog: Adjust Points Modal */}
        <Dialog open={adjustPointsOpen} onOpenChange={setAdjustPointsOpen}>
          <DialogContent className="sm:max-w-[420px]">
            <DialogHeader>
              <DialogTitle>Manual Point Adjustment</DialogTitle>
            </DialogHeader>
            <form onSubmit={handleAdjustPoints} className="space-y-4 py-2">
              <div className="space-y-2">
                <Label htmlFor="custId">Customer ID (Guid)</Label>
                <Input
                  id="custId"
                  placeholder="Paste Customer Guid..."
                  value={targetCustomerId}
                  onChange={(e) => setTargetCustomerId(e.target.value)}
                  required
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="delta">Points (+ to grant, - to deduct)</Label>
                <Input
                  id="delta"
                  type="number"
                  value={pointsDelta}
                  onChange={(e) => setPointsDelta(parseInt(e.target.value) || 0)}
                  required
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="reason">Adjustment Reason</Label>
                <Input
                  id="reason"
                  placeholder="e.g. Compensation for support delay"
                  value={reason}
                  onChange={(e) => setReason(e.target.value)}
                  required
                />
              </div>
              <DialogFooter className="pt-2">
                <Button type="submit" disabled={adjustPointsMutation.isPending} className="w-full bg-purple-600 text-white">
                  Apply Adjustment
                </Button>
              </DialogFooter>
            </form>
          </DialogContent>
        </Dialog>

        <Card className="border-slate-200 dark:border-slate-800">
          <CardHeader>
            <CardTitle className="text-base">Quick Redemption Tool</CardTitle>
            <CardDescription>Instantly redeem loyalty points for store credits or discounts</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="p-4 rounded-lg bg-indigo-500/5 border border-indigo-500/20 space-y-3">
              <p className="text-sm font-semibold text-slate-900 dark:text-white">Redeem Customer Reward</p>
              <Input
                placeholder="Enter Customer ID..."
                value={targetCustomerId}
                onChange={(e) => setTargetCustomerId(e.target.value)}
                className="text-sm"
              />
              <div className="flex gap-2">
                <Input
                  type="number"
                  placeholder="Points to redeem..."
                  value={redeemPointsVal}
                  onChange={(e) => setRedeemPointsVal(parseInt(e.target.value) || 0)}
                  className="text-sm"
                />
                <Button onClick={handleRedeemPoints} disabled={redeemPointsMutation.isPending} className="bg-indigo-600 text-white shrink-0">
                  Redeem
                </Button>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Loyalty Transactions Log */}
      <Card className="border-slate-200 dark:border-slate-800">
        <CardHeader>
          <CardTitle className="text-base">Loyalty Activity & Transaction History</CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow className="border-slate-200 dark:border-slate-800">
                <TableHead>Customer</TableHead>
                <TableHead>Type</TableHead>
                <TableHead>Description</TableHead>
                <TableHead>Points</TableHead>
                <TableHead>Balance</TableHead>
                <TableHead className="text-right">Date</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {txData?.items.length ? (
                txData.items.map((tx) => (
                  <TableRow key={tx.id} className="border-slate-100 dark:border-slate-800/60">
                    <TableCell className="font-semibold text-slate-900 dark:text-white">{tx.customerName || "Customer"}</TableCell>
                    <TableCell>
                      <Badge
                        variant="outline"
                        className={
                          tx.transactionType === "Earned"
                            ? "bg-emerald-500/10 text-emerald-600 border-emerald-500/30"
                            : tx.transactionType === "Redeemed"
                            ? "bg-purple-500/10 text-purple-600 border-purple-500/30"
                            : "bg-amber-500/10 text-amber-600 border-amber-500/30"
                        }
                      >
                        {tx.transactionType}
                      </Badge>
                    </TableCell>
                    <TableCell className="text-xs text-slate-500">{tx.description || "-"}</TableCell>
                    <TableCell className={`font-mono font-bold ${tx.pointsEarned > 0 ? "text-emerald-600" : "text-purple-600"}`}>
                      {tx.pointsEarned > 0 ? `+${tx.pointsEarned}` : `-${tx.pointsRedeemed}`}
                    </TableCell>
                    <TableCell className="font-mono font-medium">{tx.balance} pts</TableCell>
                    <TableCell className="text-right text-xs text-slate-500">
                      {new Date(tx.createdAt).toLocaleDateString()}
                    </TableCell>
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell colSpan={6} className="text-center py-6 text-slate-500 text-sm">
                    No transactions recorded yet.
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
