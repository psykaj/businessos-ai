"use client";

import { useState } from "react";
import Link from "next/link";
import {
  CheckSquare,
  Clock,
  CheckCircle2,
  XCircle,
  AlertCircle,
  FileText,
  User,
  MessageSquare,
  ChevronRight,
  ShieldAlert,
  X,
  Send
} from "lucide-react";
import {
  usePendingApprovals,
  useApproveStep,
  useRejectStep
} from "@/hooks/use-documents";
import { ApprovalRequestDto } from "@/types/document";

export default function ApprovalsWorkspacePage() {
  const [activeFilter, setActiveFilter] = useState<"Pending" | "Approved" | "Rejected" | "All">("Pending");
  const [selectedApproval, setSelectedApproval] = useState<ApprovalRequestDto | null>(null);
  const [commentText, setCommentText] = useState("");

  const { data: approvals, isLoading } = usePendingApprovals();
  const approveMutation = useApproveStep();
  const rejectMutation = useRejectStep();

  const handleApprove = async () => {
    if (!selectedApproval) return;
    await approveMutation.mutateAsync({ id: selectedApproval.id, comments: commentText });
    setSelectedApproval(null);
    setCommentText("");
  };

  const handleReject = async () => {
    if (!selectedApproval) return;
    await rejectMutation.mutateAsync({ id: selectedApproval.id, comments: commentText });
    setSelectedApproval(null);
    setCommentText("");
  };

  const approvalList = approvals || [];
  const pendingCount = approvalList.filter((a) => a.status === "Pending").length;
  const approvedCount = approvalList.filter((a) => a.status === "Approved").length;
  const rejectedCount = approvalList.filter((a) => a.status === "Rejected").length;

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 bg-gradient-to-r from-slate-900 via-indigo-950 to-slate-900 p-6 rounded-2xl text-white shadow-xl">
        <div>
          <h1 className="text-2xl font-bold tracking-tight flex items-center gap-2">
            <CheckSquare className="w-7 h-7 text-indigo-400" />
            Document Approval Workspace
          </h1>
          <p className="text-slate-300 text-sm mt-1">
            Review, approve, or reject multi-level sequential and parallel document approval requests.
          </p>
        </div>
      </div>

      {/* Analytics Summary */}
      <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
        <div className="bg-white dark:bg-slate-900 p-4 rounded-xl border border-slate-200 dark:border-slate-800 shadow-sm flex items-center gap-4">
          <div className="p-3 bg-amber-50 dark:bg-amber-950/50 rounded-xl text-amber-600 dark:text-amber-400">
            <Clock className="w-6 h-6" />
          </div>
          <div>
            <div className="text-2xl font-bold text-slate-900 dark:text-white">{pendingCount}</div>
            <div className="text-xs text-slate-500 font-medium">Pending Review</div>
          </div>
        </div>

        <div className="bg-white dark:bg-slate-900 p-4 rounded-xl border border-slate-200 dark:border-slate-800 shadow-sm flex items-center gap-4">
          <div className="p-3 bg-emerald-50 dark:bg-emerald-950/50 rounded-xl text-emerald-600 dark:text-emerald-400">
            <CheckCircle2 className="w-6 h-6" />
          </div>
          <div>
            <div className="text-2xl font-bold text-slate-900 dark:text-white">{approvedCount}</div>
            <div className="text-xs text-slate-500 font-medium">Approved</div>
          </div>
        </div>

        <div className="bg-white dark:bg-slate-900 p-4 rounded-xl border border-slate-200 dark:border-slate-800 shadow-sm flex items-center gap-4">
          <div className="p-3 bg-red-50 dark:bg-red-950/50 rounded-xl text-red-600 dark:text-red-400">
            <XCircle className="w-6 h-6" />
          </div>
          <div>
            <div className="text-2xl font-bold text-slate-900 dark:text-white">{rejectedCount}</div>
            <div className="text-xs text-slate-500 font-medium">Rejected</div>
          </div>
        </div>
      </div>

      {/* Filter Tabs */}
      <div className="flex items-center gap-2 border-b border-slate-200 dark:border-slate-800 pb-2">
        {(["Pending", "Approved", "Rejected", "All"] as const).map((tab) => (
          <button
            key={tab}
            onClick={() => setActiveFilter(tab)}
            className={`px-4 py-2 text-xs font-semibold rounded-xl transition-all ${
              activeFilter === tab
                ? "bg-indigo-600 text-white shadow-sm"
                : "bg-white dark:bg-slate-900 text-slate-600 dark:text-slate-400 border border-slate-200 dark:border-slate-800 hover:bg-slate-50"
            }`}
          >
            {tab}
          </button>
        ))}
      </div>

      {/* Approval Requests List */}
      <div className="space-y-4">
        {isLoading ? (
          <div className="p-12 text-center text-slate-500">Loading approval requests...</div>
        ) : approvalList.length === 0 ? (
          <div className="p-12 text-center bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 space-y-2">
            <CheckCircle2 className="w-10 h-10 text-emerald-500 mx-auto" />
            <div className="text-lg font-bold text-slate-900 dark:text-white">All Caught Up!</div>
            <p className="text-xs text-slate-500">No pending approval requests require your review right now.</p>
          </div>
        ) : (
          approvalList.map((app) => (
            <div
              key={app.id}
              className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-5 shadow-sm space-y-4 hover:border-indigo-500/50 transition-all"
            >
              <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b border-slate-100 dark:border-slate-800 pb-3">
                <div className="flex items-start gap-3">
                  <div className="p-2.5 bg-indigo-50 dark:bg-indigo-950/50 text-indigo-600 rounded-xl">
                    <FileText className="w-5 h-5" />
                  </div>
                  <div>
                    <h3 className="font-bold text-slate-900 dark:text-white text-base">{app.title}</h3>
                    <div className="text-xs text-slate-400 mt-0.5">
                      Document: <span className="font-medium text-slate-700 dark:text-slate-300">{app.documentName}</span> · Requested by {app.requestedByName || "Team Member"}
                    </div>
                  </div>
                </div>

                <div className="flex items-center gap-3">
                  <span
                    className={`text-xs px-3 py-1 rounded-full font-semibold ${
                      app.status === "Pending"
                        ? "bg-amber-50 text-amber-600 dark:bg-amber-950/50 dark:text-amber-400"
                        : app.status === "Approved"
                        ? "bg-emerald-50 text-emerald-600 dark:bg-emerald-950/50 dark:text-emerald-400"
                        : "bg-red-50 text-red-600 dark:bg-red-950/50 dark:text-red-400"
                    }`}
                  >
                    {app.status}
                  </span>

                  {app.status === "Pending" && (
                    <button
                      onClick={() => setSelectedApproval(app)}
                      className="bg-indigo-600 hover:bg-indigo-500 text-white text-xs font-semibold px-4 py-2 rounded-xl transition-all shadow-sm"
                    >
                      Review Request
                    </button>
                  )}
                </div>
              </div>

              {/* Steps Progress Pipeline */}
              <div className="space-y-2">
                <div className="text-xs font-bold text-slate-400 uppercase tracking-wider">Approval Sequence Steps</div>
                <div className="flex flex-wrap items-center gap-2">
                  {app.steps.map((step, idx) => (
                    <div key={step.id} className="flex items-center gap-2">
                      {idx > 0 && <ChevronRight className="w-4 h-4 text-slate-300" />}
                      <div
                        className={`p-2.5 rounded-xl border text-xs flex items-center gap-2 ${
                          step.status === "Approved"
                            ? "bg-emerald-50 border-emerald-200 text-emerald-700 dark:bg-emerald-950/40 dark:border-emerald-800"
                            : step.status === "Rejected"
                            ? "bg-red-50 border-red-200 text-red-700 dark:bg-red-950/40 dark:border-red-800"
                            : "bg-slate-50 border-slate-200 text-slate-700 dark:bg-slate-800 dark:border-slate-700"
                        }`}
                      >
                        <span className="font-bold">Step {step.sequence}:</span>
                        <span>{step.approverName || step.approverEmail}</span>
                        <span className="text-[10px] uppercase font-bold">({step.status})</span>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          ))
        )}
      </div>

      {/* Review Modal */}
      {selectedApproval && (
        <div className="fixed inset-0 bg-slate-900/60 backdrop-blur-sm z-50 flex items-center justify-center p-4">
          <div className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-6 w-full max-w-md shadow-2xl space-y-4">
            <div className="flex items-center justify-between">
              <h3 className="text-lg font-bold text-slate-900 dark:text-white">Review Approval Request</h3>
              <button onClick={() => setSelectedApproval(null)} className="text-slate-400 hover:text-slate-600">
                <X className="w-5 h-5" />
              </button>
            </div>

            <div className="bg-slate-50 dark:bg-slate-800 p-3.5 rounded-xl text-xs space-y-1">
              <div className="font-bold text-slate-900 dark:text-white">{selectedApproval.title}</div>
              <div className="text-slate-500">Document: {selectedApproval.documentName}</div>
            </div>

            <div>
              <label className="text-xs font-semibold text-slate-500 block mb-1">Approval Comments / Notes</label>
              <textarea
                rows={3}
                placeholder="Optional feedback or rationale..."
                value={commentText}
                onChange={(e) => setCommentText(e.target.value)}
                className="w-full p-3 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-xs dark:text-white"
              />
            </div>

            <div className="flex items-center justify-end gap-3 pt-2">
              <button
                onClick={handleReject}
                disabled={rejectMutation.isPending}
                className="px-4 py-2 bg-red-50 hover:bg-red-100 text-red-600 rounded-xl text-xs font-semibold transition-colors"
              >
                Reject Request
              </button>
              <button
                onClick={handleApprove}
                disabled={approveMutation.isPending}
                className="px-4 py-2 bg-emerald-600 hover:bg-emerald-500 text-white rounded-xl text-xs font-semibold transition-colors"
              >
                Approve Request
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
