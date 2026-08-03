"use client";

import React, { useState } from "react";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger, DialogFooter } from "@/components/ui/dialog";
import { Badge } from "@/components/ui/badge";
import { Search, Filter, CheckCircle2, UserPlus, MessageSquare, AlertCircle, FileSpreadsheet, Star, Sparkles, Building2 } from "lucide-react";
import { FeedbackItemDto, FeedbackStatus } from "@/lib/customer-feedback-service";
import { useUpdateFeedbackStatus, useAssignFeedback, useAddFeedbackNote, useExportFeedbackCsv } from "@/hooks/use-customer-feedback";

interface FeedbackManagementTableProps {
  feedbacks?: FeedbackItemDto[];
  total?: number;
  isLoading?: boolean;
  searchTerm: string;
  onSearchChange: (val: string) => void;
  statusFilter: string;
  onStatusChange: (val: string) => void;
  typeFilter: string;
  onTypeChange: (val: string) => void;
}

const CSM_TEAMS = [
  "Chloe Bennett (CSM)",
  "Marcus Sterling (VP Tech)",
  "Alex Rivera (Senior CSM)",
  "Jessica Alroy (Account Director)",
  "Infrastructure Emergency Queue",
];

export function FeedbackManagementTable({
  feedbacks = [],
  total = 0,
  isLoading,
  searchTerm,
  onSearchChange,
  statusFilter,
  onStatusChange,
  typeFilter,
  onTypeChange,
}: FeedbackManagementTableProps) {
  const updateStatusMutation = useUpdateFeedbackStatus();
  const assignMutation = useAssignFeedback();
  const addNoteMutation = useAddFeedbackNote();
  const exportCsvMutation = useExportFeedbackCsv();

  // Dialog States
  const [selectedItem, setSelectedItem] = useState<FeedbackItemDto | null>(null);
  const [noteText, setNoteText] = useState("");
  const [selectedAssignee, setSelectedAssignee] = useState(CSM_TEAMS[0]);
  const [isNoteOpen, setIsNoteOpen] = useState(false);
  const [isAssignOpen, setIsAssignOpen] = useState(false);

  const handleResolve = (id: string) => {
    updateStatusMutation.mutate({ id, status: "Resolved" });
  };

  const handleSaveNote = () => {
    if (!selectedItem || !noteText.trim()) return;
    addNoteMutation.mutate(
      { id: selectedItem.id, note: noteText },
      {
        onSuccess: () => {
          setNoteText("");
          setIsNoteOpen(false);
        },
      }
    );
  };

  const handleSaveAssignment = () => {
    if (!selectedItem) return;
    assignMutation.mutate(
      { id: selectedItem.id, assigneeName: selectedAssignee },
      {
        onSuccess: () => {
          setIsAssignOpen(false);
        },
      }
    );
  };

  const getSentimentBadge = (sentiment: string, score: number) => {
    if (sentiment === "Positive") {
      return <Badge className="bg-emerald-500 text-white font-medium hover:bg-emerald-600">Positive ({score}%)</Badge>;
    } else if (sentiment === "Negative") {
      return <Badge className="bg-rose-500 text-white font-medium animate-pulse hover:bg-rose-600">Negative ({score}%)</Badge>;
    }
    return <Badge className="bg-slate-500 text-white font-medium">Neutral ({score}%)</Badge>;
  };

  const getStatusBadge = (status: FeedbackStatus) => {
    switch (status) {
      case "Resolved":
        return <Badge className="bg-emerald-100 text-emerald-800 dark:bg-emerald-900/30 dark:text-emerald-300 border border-emerald-300 dark:border-emerald-800">Resolved</Badge>;
      case "Assigned":
        return <Badge className="bg-blue-100 text-blue-800 dark:bg-blue-900/30 dark:text-blue-300 border border-blue-300 dark:border-blue-800">Assigned</Badge>;
      case "InReview":
        return <Badge className="bg-amber-100 text-amber-800 dark:bg-amber-900/30 dark:text-amber-300 border border-amber-300 dark:border-amber-800">In Review</Badge>;
      case "New":
        return <Badge className="bg-purple-100 text-purple-800 dark:bg-purple-900/30 dark:text-purple-300 border border-purple-300 dark:border-purple-800 font-semibold">New</Badge>;
      default:
        return <Badge className="bg-slate-100 text-slate-800 dark:bg-slate-800 dark:text-slate-300">{status}</Badge>;
    }
  };

  return (
    <Card className="bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-md rounded-xl overflow-hidden">
      <CardHeader className="bg-slate-50/50 dark:bg-slate-900/50 border-b border-slate-200 dark:border-slate-800 p-6">
        <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
          <div>
            <CardTitle className="text-xl font-bold text-slate-900 dark:text-white flex items-center gap-2">
              <MessageSquare className="h-5 w-5 text-indigo-500" />
              Customer Feedback & Triage Workspace ({total})
            </CardTitle>
            <CardDescription className="text-slate-500 dark:text-slate-400 text-xs mt-1">
              Automatically sorted by AI sentiment urgency. Action high-risk items immediately to safeguard revenue.
            </CardDescription>
          </div>
          <Button
            variant="outline"
            size="sm"
            onClick={() => exportCsvMutation.mutate()}
            disabled={exportCsvMutation.isPending}
            className="flex items-center gap-2 bg-emerald-50 dark:bg-emerald-950/30 border-emerald-200 dark:border-emerald-800 text-emerald-700 dark:text-emerald-300 hover:bg-emerald-100 dark:hover:bg-emerald-900/50 transition-colors shadow-sm"
          >
            <FileSpreadsheet className="h-4 w-4" />
            {exportCsvMutation.isPending ? "Exporting CSV..." : "Instant CSV Export"}
          </Button>
        </div>

        {/* Filters Bar */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-3 mt-6">
          <div className="relative">
            <Search className="absolute left-3 top-3 h-4 w-4 text-slate-400" />
            <Input
              placeholder="Search customers, companies, or keywords..."
              value={searchTerm}
              onChange={(e) => onSearchChange(e.target.value)}
              className="pl-9 h-10 bg-white dark:bg-slate-800 border-slate-200 dark:border-slate-700 text-sm rounded-lg"
            />
          </div>
          <div className="flex items-center gap-2">
            <Filter className="h-4 w-4 text-slate-400 flex-shrink-0" />
            <select
              value={statusFilter}
              onChange={(e) => onStatusChange(e.target.value)}
              className="w-full h-10 px-3 bg-white dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-lg text-sm font-medium text-slate-700 dark:text-slate-300 focus:outline-none focus:ring-2 focus:ring-indigo-500"
            >
              <option value="All">All Ticket Statuses</option>
              <option value="New">New & Unassigned</option>
              <option value="InReview">In Review</option>
              <option value="Assigned">Assigned to Team</option>
              <option value="Resolved">Resolved & Closed</option>
            </select>
          </div>
          <div>
            <select
              value={typeFilter}
              onChange={(e) => onTypeChange(e.target.value)}
              className="w-full h-10 px-3 bg-white dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-lg text-sm font-medium text-slate-700 dark:text-slate-300 focus:outline-none focus:ring-2 focus:ring-indigo-500"
            >
              <option value="All">All Feedback Types</option>
              <option value="Complaint">Urgent Complaints (High Churn Risk)</option>
              <option value="Praise">Brand Promoter Praise (VIP Leads)</option>
              <option value="Inquiry">General Inquiries</option>
              <option value="FeatureRequest">Feature & Roadmap Requests</option>
            </select>
          </div>
        </div>
      </CardHeader>

      <CardContent className="p-0">
        {isLoading ? (
          <div className="p-12 text-center text-slate-500 dark:text-slate-400 animate-pulse font-medium">
            Loading real-time enterprise customer feedback...
          </div>
        ) : feedbacks.length === 0 ? (
          <div className="p-16 text-center">
            <Sparkles className="h-12 w-12 text-indigo-400 mx-auto mb-3 animate-bounce" />
            <h4 className="text-lg font-bold text-slate-800 dark:text-white">No feedback records found</h4>
            <p className="text-sm text-slate-500 dark:text-slate-400 mt-1">Try relaxing your search keywords or filter dropdowns above.</p>
          </div>
        ) : (
          <div className="overflow-x-auto">
            <Table className="w-full text-left border-collapse">
              <TableHeader className="bg-slate-50 dark:bg-slate-900 text-xs font-semibold text-slate-500 dark:text-slate-400 uppercase tracking-wider">
                <TableRow>
                  <TableHead className="py-3.5 px-6">Customer Account</TableHead>
                  <TableHead className="py-3.5 px-4">Type & Rating</TableHead>
                  <TableHead className="py-3.5 px-4 w-1/3">Feedback Detail & AI Sentiment</TableHead>
                  <TableHead className="py-3.5 px-4">ARR Value at Risk</TableHead>
                  <TableHead className="py-3.5 px-4">Assignee & Status</TableHead>
                  <TableHead className="py-3.5 px-6 text-right">Action Triage</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody className="divide-y divide-slate-100 dark:divide-slate-800">
                {feedbacks.map((fb) => (
                  <TableRow
                    key={fb.id}
                    className={`hover:bg-slate-50/70 dark:hover:bg-slate-800/50 transition-colors ${fb.isUrgent ? "bg-rose-50/20 dark:bg-rose-950/10 border-l-4 border-l-rose-500" : ""}`}
                  >
                    {/* Customer */}
                    <TableCell className="py-4 px-6 align-top">
                      <div className="font-bold text-sm text-slate-900 dark:text-white flex items-center gap-1.5">
                        {fb.customerName}
                        {fb.isUrgent && (
                          <span title="Urgent Executive Attention Required">
                            <AlertCircle className="h-4 w-4 text-rose-500 flex-shrink-0" />
                          </span>
                        )}
                      </div>
                      <div className="text-xs text-slate-500 dark:text-slate-400 flex items-center gap-1 mt-0.5">
                        <Building2 className="h-3 w-3" />
                        {fb.customerCompany}
                      </div>
                      <span className="text-[11px] font-mono text-slate-400 mt-1 block">Via {fb.channel}</span>
                    </TableCell>

                    {/* Type & Rating */}
                    <TableCell className="py-4 px-4 align-top">
                      <div className="inline-block">
                        <span
                          className={`text-xs font-bold px-2 py-0.5 rounded-full ${
                            fb.type === "Complaint"
                              ? "bg-rose-100 text-rose-800 dark:bg-rose-900/40 dark:text-rose-300"
                              : fb.type === "Praise"
                              ? "bg-emerald-100 text-emerald-800 dark:bg-emerald-900/40 dark:text-emerald-300"
                              : "bg-blue-100 text-blue-800 dark:bg-blue-900/40 dark:text-blue-300"
                          }`}
                        >
                          {fb.type}
                        </span>
                      </div>
                      <div className="flex items-center gap-1 mt-2 text-amber-500 font-bold text-xs">
                        <Star className="h-3.5 w-3.5 fill-current" />
                        <span>{fb.ratingValue} / 5</span>
                      </div>
                    </TableCell>

                    {/* Content & Sentiment */}
                    <TableCell className="py-4 px-4 align-top max-w-sm">
                      <p className="text-sm text-slate-800 dark:text-slate-200 leading-snug font-medium">{fb.content}</p>
                      <div className="flex items-center gap-2 mt-3">
                        {getSentimentBadge(fb.sentimentLabel, fb.confidenceScore)}
                        {fb.internalNotes.length > 0 && (
                          <span className="text-xs text-indigo-600 dark:text-indigo-400 font-semibold bg-indigo-50 dark:bg-indigo-950/40 px-2 py-0.5 rounded border border-indigo-200 dark:border-indigo-800">
                            {fb.internalNotes.length} Note(s) Attached
                          </span>
                        )}
                      </div>
                      {fb.internalNotes.length > 0 && (
                        <div className="mt-2 text-[11px] bg-slate-100 dark:bg-slate-800 p-2 rounded text-slate-600 dark:text-slate-300 italic border border-slate-200 dark:border-slate-700">
                          {fb.internalNotes[fb.internalNotes.length - 1]}
                        </div>
                      )}
                    </TableCell>

                    {/* ARR at Risk */}
                    <TableCell className="py-4 px-4 align-top">
                      <div className="text-sm font-black text-slate-900 dark:text-white font-mono">
                        ${fb.estimatedArrImpact.toLocaleString()}
                      </div>
                      <span className="text-[11px] text-slate-400 block">Annual Value</span>
                    </TableCell>

                    {/* Assignee & Status */}
                    <TableCell className="py-4 px-4 align-top">
                      <div className="mb-2">{getStatusBadge(fb.status)}</div>
                      <div className="text-xs font-medium text-slate-700 dark:text-slate-300">
                        {fb.assignedToUserName || "Unassigned"}
                      </div>
                    </TableCell>

                    {/* Actions */}
                    <TableCell className="py-4 px-6 text-right align-top">
                      <div className="flex items-center justify-end gap-1.5">
                        {/* Resolve */}
                        {fb.status !== "Resolved" && (
                          <Button
                            size="sm"
                            variant="outline"
                            onClick={() => handleResolve(fb.id)}
                            title="Mark ticket resolved & protect SLA"
                            className="text-xs h-8 px-2.5 bg-emerald-50/50 hover:bg-emerald-100 text-emerald-700 border-emerald-200 transition-all font-semibold"
                          >
                            <CheckCircle2 className="h-3.5 w-3.5 mr-1" />
                            Resolve
                          </Button>
                        )}

                        {/* Assign Dialog */}
                        <Button
                          size="sm"
                          variant="outline"
                          onClick={() => {
                            setSelectedItem(fb);
                            setSelectedAssignee(fb.assignedToUserName || CSM_TEAMS[0]);
                            setIsAssignOpen(true);
                          }}
                          className="text-xs h-8 px-2 hover:bg-slate-100 dark:hover:bg-slate-800"
                          title="Assign CSM or Support Lead"
                        >
                          <UserPlus className="h-3.5 w-3.5" />
                        </Button>

                        {/* Add Note Dialog */}
                        <Button
                          size="sm"
                          variant="outline"
                          onClick={() => {
                            setSelectedItem(fb);
                            setNoteText("");
                            setIsNoteOpen(true);
                          }}
                          className="text-xs h-8 px-2 hover:bg-slate-100 dark:hover:bg-slate-800"
                          title="Attach Internal Triage Note"
                        >
                          <MessageSquare className="h-3.5 w-3.5" />
                        </Button>
                      </div>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>
        )}
      </CardContent>

      {/* Assign Dialog Modal */}
      <Dialog open={isAssignOpen} onOpenChange={setIsAssignOpen}>
        <DialogContent className="max-w-md p-6 bg-white dark:bg-slate-900 rounded-xl border border-slate-200 dark:border-slate-800 shadow-2xl">
          <DialogHeader>
            <DialogTitle className="text-lg font-bold flex items-center gap-2 text-slate-900 dark:text-white">
              <UserPlus className="h-5 w-5 text-indigo-500" />
              Assign Ticket to Team Member
            </DialogTitle>
            <CardDescription className="text-xs text-slate-500 dark:text-slate-400">
              Assigning a dedicated Account CSM starts the First Response SLA timer and prevents account drop-off.
            </CardDescription>
          </DialogHeader>
          <div className="space-y-4 py-3">
            <div>
              <Label className="text-xs font-semibold text-slate-700 dark:text-slate-300">Customer</Label>
              <p className="text-sm font-bold text-slate-900 dark:text-white mt-1">
                {selectedItem?.customerName} ({selectedItem?.customerCompany}) - ${selectedItem?.estimatedArrImpact.toLocaleString()} ARR
              </p>
            </div>
            <div>
              <Label className="text-xs font-semibold text-slate-700 dark:text-slate-300">Select Assignee</Label>
              <select
                value={selectedAssignee}
                onChange={(e) => setSelectedAssignee(e.target.value)}
                className="w-full mt-1 h-10 px-3 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-lg text-sm font-medium text-slate-800 dark:text-slate-200"
              >
                {CSM_TEAMS.map((team, idx) => (
                  <option key={idx} value={team}>
                    {team}
                  </option>
                ))}
              </select>
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" size="sm" onClick={() => setIsAssignOpen(false)}>
              Cancel
            </Button>
            <Button
              size="sm"
              onClick={handleSaveAssignment}
              disabled={assignMutation.isPending}
              className="bg-indigo-600 hover:bg-indigo-700 text-white font-semibold shadow-md"
            >
              Confirm Assignment
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Internal Note Dialog Modal */}
      <Dialog open={isNoteOpen} onOpenChange={setIsNoteOpen}>
        <DialogContent className="max-w-md p-6 bg-white dark:bg-slate-900 rounded-xl border border-slate-200 dark:border-slate-800 shadow-2xl">
          <DialogHeader>
            <DialogTitle className="text-lg font-bold flex items-center gap-2 text-slate-900 dark:text-white">
              <MessageSquare className="h-5 w-5 text-indigo-500" />
              Attach Internal Team Note
            </DialogTitle>
            <CardDescription className="text-xs text-slate-500 dark:text-slate-400">
              Notes are visible only to internal team staff and AI Copilot for audit timeline preservation.
            </CardDescription>
          </DialogHeader>
          <div className="space-y-4 py-3">
            <div>
              <Label className="text-xs font-semibold text-slate-700 dark:text-slate-300">Triage Notes & Next Steps</Label>
              <textarea
                value={noteText}
                onChange={(e) => setNoteText(e.target.value)}
                placeholder="E.g., Called customer via Zoom. Applied $500 SLA discount and confirmed bug fix..."
                className="w-full mt-1 h-28 p-3 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-lg text-sm text-slate-800 dark:text-slate-200 focus:outline-none focus:ring-2 focus:ring-indigo-500"
              />
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" size="sm" onClick={() => setIsNoteOpen(false)}>
              Cancel
            </Button>
            <Button
              size="sm"
              onClick={handleSaveNote}
              disabled={addNoteMutation.isPending || !noteText.trim()}
              className="bg-indigo-600 hover:bg-indigo-700 text-white font-semibold shadow-md"
            >
              Save Internal Note
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </Card>
  );
}
