"use client";

import React, { useState } from "react";
import { useFeedbackSearch, useCsatDashboard, useSentimentAnalytics, useSubmitFeedback } from "@/hooks/use-customer-feedback";
import { FeedbackKpiCards } from "@/components/customer-feedback/feedback-kpi-cards";
import { FeedbackManagementTable } from "@/components/customer-feedback/feedback-management-table";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from "@/components/ui/dialog";
import { MessageSquare, Plus, Sparkles, Star, Zap, Building2, ExternalLink } from "lucide-react";

export default function CustomerFeedbackCenterPage() {
  const [searchTerm, setSearchTerm] = useState("");
  const [statusFilter, setStatusFilter] = useState("All");
  const [typeFilter, setTypeFilter] = useState("All");

  const { data: feedbacksData, isLoading: feedbacksLoading } = useFeedbackSearch(searchTerm, statusFilter, typeFilter);
  const { data: csatData, isLoading: csatLoading } = useCsatDashboard();
  const { data: sentimentData, isLoading: sentimentLoading } = useSentimentAnalytics();
  const submitMutation = useSubmitFeedback();

  // Modal states for manual feedback capture / widget simulator
  const [isRecordOpen, setIsRecordOpen] = useState(false);
  const [customerName, setCustomerName] = useState("");
  const [customerCompany, setCustomerCompany] = useState("");
  const [feedbackType, setFeedbackType] = useState<"Complaint" | "Inquiry" | "Praise" | "FeatureRequest">("Complaint");
  const [ratingValue, setRatingValue] = useState(3);
  const [content, setContent] = useState("");

  const handleRecordSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!customerName.trim() || !content.trim()) return;
    submitMutation.mutate(
      {
        customerName,
        customerCompany: customerCompany || "Independent Account",
        type: feedbackType,
        ratingValue,
        content,
        channel: "Web Widget",
        isUrgent: feedbackType === "Complaint" || ratingValue <= 2,
      },
      {
        onSuccess: () => {
          setIsRecordOpen(false);
          setCustomerName("");
          setCustomerCompany("");
          setContent("");
        },
      }
    );
  };

  return (
    <div className="space-y-8 p-6 max-w-7xl mx-auto">
      {/* Header Banner */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b border-slate-200 dark:border-slate-800 pb-6">
        <div>
          <h1 className="text-3xl font-black text-slate-900 dark:text-white tracking-tight flex items-center gap-3">
            <MessageSquare className="h-8 w-8 text-indigo-500" />
            Customer Feedback Center
          </h1>
          <p className="text-slate-500 dark:text-slate-400 mt-1 text-sm font-medium">
            Centralized intake and triage workspace. Identify dissatisfied accounts before cancellation to protect Annual Recurring Revenue.
          </p>
        </div>
        <div className="flex items-center gap-3">
          <Button onClick={() => setIsRecordOpen(true)} className="bg-indigo-600 hover:bg-indigo-700 text-white font-bold px-4 h-10 shadow-lg flex items-center gap-2">
            <Plus className="h-4 w-4" /> Record Customer Feedback
          </Button>
          <Dialog open={isRecordOpen} onOpenChange={setIsRecordOpen}>
            <DialogContent className="max-w-md p-6 bg-white dark:bg-slate-900 rounded-xl border border-slate-200 dark:border-slate-800 shadow-2xl">
              <DialogHeader>
                <DialogTitle className="text-lg font-bold text-slate-900 dark:text-white flex items-center gap-2">
                  <Sparkles className="h-5 w-5 text-indigo-500" /> Capture Live Customer Feedback
                </DialogTitle>
                <CardDescription className="text-xs text-slate-500">
                  Submissions trigger real-time Provider-Independent AI sentiment classification in milliseconds.
                </CardDescription>
              </DialogHeader>
              <form onSubmit={handleRecordSubmit} className="space-y-4 py-3">
                <div>
                  <Label className="text-xs font-semibold text-slate-700 dark:text-slate-300">Customer Name & Role</Label>
                  <Input placeholder="E.g. Elena Vance (COO)" value={customerName} onChange={(e) => setCustomerName(e.target.value)} className="mt-1 h-10 text-sm" required />
                </div>
                <div>
                  <Label className="text-xs font-semibold text-slate-700 dark:text-slate-300">Company Name</Label>
                  <Input placeholder="E.g. VentureScale AI" value={customerCompany} onChange={(e) => setCustomerCompany(e.target.value)} className="mt-1 h-10 text-sm" />
                </div>
                <div className="grid grid-cols-2 gap-3">
                  <div>
                    <Label className="text-xs font-semibold text-slate-700 dark:text-slate-300">Feedback Type</Label>
                    <select value={feedbackType} onChange={(e) => setFeedbackType(e.target.value as any)} className="w-full mt-1 h-10 px-3 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-lg text-sm font-medium">
                      <option value="Complaint">Complaint</option>
                      <option value="Inquiry">Inquiry</option>
                      <option value="Praise">Praise (Promoter)</option>
                      <option value="FeatureRequest">Feature Request</option>
                    </select>
                  </div>
                  <div>
                    <Label className="text-xs font-semibold text-slate-700 dark:text-slate-300">Star Rating ({ratingValue}/5)</Label>
                    <div className="flex items-center gap-1.5 mt-2">
                      {[1, 2, 3, 4, 5].map((s) => (
                        <button key={s} type="button" onClick={() => setRatingValue(s)} className={`p-1.5 rounded ${s <= ratingValue ? "text-amber-500 font-bold" : "text-slate-300 dark:text-slate-700"}`}>
                          <Star className="h-5 w-5 fill-current" />
                        </button>
                      ))}
                    </div>
                  </div>
                </div>
                <div>
                  <Label className="text-xs font-semibold text-slate-700 dark:text-slate-300">Feedback Content / Customer Voice</Label>
                  <textarea placeholder="Describe customer interaction, latency issues, or positive remarks..." value={content} onChange={(e) => setContent(e.target.value)} className="w-full mt-1 h-24 p-3 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-lg text-sm text-slate-800 dark:text-slate-200" required />
                </div>
                <DialogFooter>
                  <Button type="button" variant="outline" size="sm" onClick={() => setIsRecordOpen(false)}>Cancel</Button>
                  <Button type="submit" size="sm" disabled={submitMutation.isPending || !customerName || !content} className="bg-indigo-600 hover:bg-indigo-700 text-white font-semibold">
                    Submit to AI Engine
                  </Button>
                </DialogFooter>
              </form>
            </DialogContent>
          </Dialog>
        </div>
      </div>

      {/* KPI Summary Dashboard */}
      <FeedbackKpiCards csatData={csatData} sentimentData={sentimentData} isLoading={csatLoading || sentimentLoading} />

      {/* Feedback Management Workspace */}
      <FeedbackManagementTable
        feedbacks={feedbacksData?.items}
        total={feedbacksData?.total}
        isLoading={feedbacksLoading}
        searchTerm={searchTerm}
        onSearchChange={setSearchTerm}
        statusFilter={statusFilter}
        onStatusChange={setStatusFilter}
        typeFilter={typeFilter}
        onTypeChange={setTypeFilter}
      />
    </div>
  );
}
