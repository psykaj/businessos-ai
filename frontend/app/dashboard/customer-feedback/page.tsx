"use client";

import React, { useState } from "react";
import { useSatisfactionSummary, useFeedbackPaged, useSubmitFeedback } from "@/hooks/use-customer-success";
import { CSATDistributionChart } from "@/components/customer-success/csat-distribution-chart";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger, DialogFooter } from "@/components/ui/dialog";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { MessageSquare, Star, Plus, AlertTriangle, ThumbsUp, ThumbsDown, ShieldAlert, Filter } from "lucide-react";

export default function CustomerFeedbackPage() {
  const { data: summary, isLoading: summaryLoading } = useSatisfactionSummary();
  const { data: feedbackData } = useFeedbackPaged({ pageSize: 15 });

  // Dialog States
  const [submitFeedbackOpen, setSubmitFeedbackOpen] = useState(false);

  // Form States
  const [targetCustomerId, setTargetCustomerId] = useState("");
  const [rating, setRating] = useState(5);
  const [comment, setComment] = useState("");
  const [channel, setChannel] = useState("Web");

  const submitFeedbackMutation = useSubmitFeedback();

  const handleSubmitFeedback = (e: React.FormEvent) => {
    e.preventDefault();
    if (!targetCustomerId) return;
    submitFeedbackMutation.mutate(
      {
        customerId: targetCustomerId,
        rating,
        feedback: comment,
        channel,
      },
      {
        onSuccess: () => {
          setSubmitFeedbackOpen(false);
          setTargetCustomerId("");
          setComment("");
        },
      }
    );
  };

  return (
    <div className="space-y-6 p-6 max-w-7xl mx-auto">
      {/* Top Banner */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-slate-200 dark:border-slate-800 pb-6">
        <div>
          <h1 className="text-3xl font-bold text-slate-900 dark:text-white tracking-tight flex items-center gap-3">
            <MessageSquare className="h-8 w-8 text-amber-500" />
            Customer Satisfaction & CSAT
          </h1>
          <p className="text-slate-500 dark:text-slate-400 mt-1">
            Capture satisfaction scores, monitor star distributions, and receive automated alerts on negative feedback.
          </p>
        </div>

        {/* Dialog: Submit Customer CSAT */}
        <Dialog open={submitFeedbackOpen} onOpenChange={setSubmitFeedbackOpen}>
          <DialogTrigger
            render={
              <Button size="sm" className="gap-2 bg-amber-500 hover:bg-amber-600 text-slate-950 font-semibold">
                <Plus className="h-4 w-4" /> Submit Customer Feedback
              </Button>
            }
          />

          <DialogContent className="sm:max-w-[440px]">
            <DialogHeader>
              <DialogTitle>Record Customer CSAT Feedback</DialogTitle>
            </DialogHeader>
            <form onSubmit={handleSubmitFeedback} className="space-y-4 py-2">
              <div className="space-y-2">
                <Label htmlFor="cId">Customer ID (Guid)</Label>
                <Input
                  id="cId"
                  placeholder="Paste Customer Guid..."
                  value={targetCustomerId}
                  onChange={(e) => setTargetCustomerId(e.target.value)}
                  required
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="ratingVal">Satisfaction Rating (1 to 5 Stars)</Label>
                <div className="flex items-center gap-2 pt-1">
                  {[1, 2, 3, 4, 5].map((s) => (
                    <button
                      key={s}
                      type="button"
                      onClick={() => setRating(s)}
                      className={`p-2 rounded-lg border transition-all ${
                        rating >= s
                          ? "bg-amber-500/10 border-amber-500 text-amber-500"
                          : "bg-slate-50 dark:bg-slate-900 border-slate-200 dark:border-slate-800 text-slate-400"
                      }`}
                    >
                      <Star className={`h-5 w-5 ${rating >= s ? "fill-amber-400" : ""}`} />
                    </button>
                  ))}
                </div>
              </div>
              <div className="space-y-2">
                <Label htmlFor="comments">Feedback Comment</Label>
                <Input
                  id="comments"
                  placeholder="e.g. Great product onboarding experience!"
                  value={comment}
                  onChange={(e) => setComment(e.target.value)}
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="channelVal">Submission Channel</Label>
                <Input
                  id="channelVal"
                  placeholder="Web, InApp, Email, SMS"
                  value={channel}
                  onChange={(e) => setChannel(e.target.value)}
                />
              </div>
              <DialogFooter className="pt-2">
                <Button type="submit" disabled={submitFeedbackMutation.isPending} className="w-full bg-amber-500 text-slate-950 font-bold">
                  Save Feedback
                </Button>
              </DialogFooter>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      {/* Overview Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <Card className="border-slate-200 dark:border-slate-800">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-amber-600 dark:text-amber-400 uppercase tracking-wider">Average CSAT Score</span>
              <Star className="h-5 w-5 text-amber-500 fill-amber-500/20" />
            </div>
            <div className="mt-3 flex items-baseline gap-2">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">
                {summary?.averageRating ?? 4.8}
              </span>
              <span className="text-sm text-slate-500">/ 5.0</span>
            </div>
            <p className="text-xs text-slate-500 mt-1">Based on {summary?.totalSubmissions ?? 0} reviews</p>
          </CardContent>
        </Card>

        <Card className="border-slate-200 dark:border-slate-800">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-emerald-600 dark:text-emerald-400 uppercase tracking-wider">Positive Feedback</span>
              <ThumbsUp className="h-5 w-5 text-emerald-500" />
            </div>
            <div className="mt-3">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">
                {summary?.positiveFeedbackPercentage ?? 92}%
              </span>
            </div>
            <p className="text-xs text-slate-500 mt-1">4 & 5 Star Ratings</p>
          </CardContent>
        </Card>

        <Card className="border-slate-200 dark:border-slate-800">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-rose-600 dark:text-rose-400 uppercase tracking-wider">Negative Feedback</span>
              <ThumbsDown className="h-5 w-5 text-rose-500" />
            </div>
            <div className="mt-3">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">
                {summary?.negativeFeedbackPercentage ?? 4}%
              </span>
            </div>
            <p className="text-xs text-slate-500 mt-1">1 & 2 Star Ratings (Triggers Tasks)</p>
          </CardContent>
        </Card>

        <Card className="border-slate-200 dark:border-slate-800">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <span className="text-xs font-semibold text-purple-600 dark:text-purple-400 uppercase tracking-wider">Total Submissions</span>
              <MessageSquare className="h-5 w-5 text-purple-500" />
            </div>
            <div className="mt-3">
              <span className="text-3xl font-bold text-slate-900 dark:text-white font-mono">
                {summary?.totalSubmissions ?? 0}
              </span>
            </div>
            <p className="text-xs text-slate-500 mt-1">Recorded across all channels</p>
          </CardContent>
        </Card>
      </div>

      {/* Distribution Chart & Negative Feedback Alert Box */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <Card className="lg:col-span-2 border-slate-200 dark:border-slate-800">
          <CardHeader>
            <CardTitle className="text-base">Rating Distribution Breakdown</CardTitle>
            <CardDescription>Star rating distribution across customer feedback responses</CardDescription>
          </CardHeader>
          <CardContent>
            <CSATDistributionChart
              distribution={summary?.ratingDistribution ?? { 5: 18, 4: 5, 3: 2, 2: 1, 1: 0 }}
              totalSubmissions={summary?.totalSubmissions ?? 26}
            />
          </CardContent>
        </Card>

        <Card className="border-slate-200 dark:border-slate-800 bg-rose-500/5">
          <CardHeader>
            <CardTitle className="text-base flex items-center gap-2 text-rose-600 dark:text-rose-400">
              <ShieldAlert className="h-5 w-5" /> Negative Feedback Alert System
            </CardTitle>
            <CardDescription>Proactive churn prevention</CardDescription>
          </CardHeader>
          <CardContent className="space-y-3 text-xs text-slate-600 dark:text-slate-300">
            <p>
              When a customer submits a CSAT score of <strong>1 or 2 stars</strong>, BusinessOS AI automatically:
            </p>
            <ul className="list-disc pl-4 space-y-1">
              <li>Triggers an urgent Outreach Success Task.</li>
              <li>Lowers the customer's account Health Score.</li>
              <li>Flags account for manager review.</li>
            </ul>
          </CardContent>
        </Card>
      </div>

      {/* Feedback History Log */}
      <Card className="border-slate-200 dark:border-slate-800">
        <CardHeader>
          <CardTitle className="text-base">Customer Feedback History</CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow className="border-slate-200 dark:border-slate-800">
                <TableHead>Customer</TableHead>
                <TableHead>Rating</TableHead>
                <TableHead>Channel</TableHead>
                <TableHead>Feedback</TableHead>
                <TableHead className="text-right">Submitted At</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {feedbackData?.items.length ? (
                feedbackData.items.map((fb) => (
                  <TableRow key={fb.id} className="border-slate-100 dark:border-slate-800/60">
                    <TableCell className="font-semibold text-slate-900 dark:text-white">{fb.customerName || "Customer"}</TableCell>
                    <TableCell>
                      <span className="font-bold text-amber-500 flex items-center gap-1">
                        <Star className="h-3.5 w-3.5 fill-amber-400" /> {fb.rating} / 5
                      </span>
                    </TableCell>
                    <TableCell>
                      <Badge variant="outline" className="text-xs">{fb.channel}</Badge>
                    </TableCell>
                    <TableCell className="text-xs text-slate-600 dark:text-slate-300">{fb.feedback || "No comment provided."}</TableCell>
                    <TableCell className="text-right text-xs text-slate-500">
                      {new Date(fb.submittedAt).toLocaleDateString()}
                    </TableCell>
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell colSpan={5} className="text-center py-6 text-slate-500 text-sm">
                    No customer feedback records logged yet.
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
