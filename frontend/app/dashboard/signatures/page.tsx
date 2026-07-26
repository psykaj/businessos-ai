"use client";

import { useState } from "react";
import {
  Sparkles,
  Plus,
  FileCheck2,
  Clock,
  CheckCircle2,
  XCircle,
  ShieldCheck,
  UserCheck,
  Mail,
  Lock,
  X,
  ExternalLink,
  Search,
  FileText
} from "lucide-react";
import {
  useCreateSignatureRequest,
  useSignatureAuditTrail,
  useDocuments
} from "@/hooks/use-documents";
import { SignatureRequestDto, SignatureAuditTrailDto } from "@/types/document";

export default function ESignatureWorkspacePage() {
  const [activeTab, setActiveTab] = useState<"Pending" | "Completed" | "Expired" | "All">("Pending");

  // Modals state
  const [isNewRequestOpen, setIsNewRequestOpen] = useState(false);
  const [selectedAuditTrailId, setSelectedAuditTrailId] = useState<string | null>(null);

  // Form State
  const [title, setTitle] = useState("");
  const [documentId, setDocumentId] = useState("");
  const [message, setMessage] = useState("");
  const [expiresAt, setExpiresAt] = useState("");
  const [recipients, setRecipients] = useState<{ signerName: string; signerEmail: string; role: string; accessCode: string }[]>([
    { signerName: "", signerEmail: "", role: "Signer", accessCode: "" }
  ]);

  // Data fetching
  const { data: documentsData } = useDocuments({ pageSize: 50 });
  const createSignatureMutation = useCreateSignatureRequest();
  const { data: auditTrail } = useSignatureAuditTrail(selectedAuditTrailId || "");

  const handleAddRecipient = () => {
    setRecipients([...recipients, { signerName: "", signerEmail: "", role: "Signer", accessCode: "" }]);
  };

  const handleRemoveRecipient = (index: number) => {
    setRecipients(recipients.filter((_, i) => i !== index));
  };

  const handleRecipientChange = (index: number, field: string, value: string) => {
    const updated = [...recipients];
    (updated[index] as any)[field] = value;
    setRecipients(updated);
  };

  const handleCreateRequest = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!documentId || !title.trim()) return;

    await createSignatureMutation.mutateAsync({
      documentId,
      title,
      message,
      expiresAt: expiresAt ? new Date(expiresAt).toISOString() : undefined,
      recipients: recipients.map((r, idx) => ({
        signerName: r.signerName,
        signerEmail: r.signerEmail,
        role: r.role,
        signingOrder: idx + 1,
        accessCode: r.accessCode || undefined,
      })),
    });

    setIsNewRequestOpen(false);
    setTitle("");
    setMessage("");
  };

  const docs = documentsData?.items || [];

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 bg-gradient-to-r from-slate-900 via-indigo-950 to-slate-900 p-6 rounded-2xl text-white shadow-xl">
        <div>
          <h1 className="text-2xl font-bold tracking-tight flex items-center gap-2">
            <Sparkles className="w-7 h-7 text-indigo-400" />
            E-Signature Workspace
          </h1>
          <p className="text-slate-300 text-sm mt-1">
            Collect legally-binding digital signatures with cryptographic SHA-256 hash certification.
          </p>
        </div>
        <button
          onClick={() => setIsNewRequestOpen(true)}
          className="flex items-center gap-2 bg-indigo-600 hover:bg-indigo-500 text-white px-4 py-2.5 rounded-xl font-medium text-sm transition-all shadow-md shadow-indigo-600/30"
        >
          <Plus className="w-4 h-4" /> Request Signatures
        </button>
      </div>

      {/* Analytics Summary */}
      <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
        <div className="bg-white dark:bg-slate-900 p-4 rounded-xl border border-slate-200 dark:border-slate-800 shadow-sm flex items-center gap-4">
          <div className="p-3 bg-purple-50 dark:bg-purple-950/50 rounded-xl text-purple-600 dark:text-purple-400">
            <Clock className="w-6 h-6" />
          </div>
          <div>
            <div className="text-2xl font-bold text-slate-900 dark:text-white">Active Requests</div>
            <div className="text-xs text-slate-500 font-medium">Pending Signer Action</div>
          </div>
        </div>

        <div className="bg-white dark:bg-slate-900 p-4 rounded-xl border border-slate-200 dark:border-slate-800 shadow-sm flex items-center gap-4">
          <div className="p-3 bg-emerald-50 dark:bg-emerald-950/50 rounded-xl text-emerald-600 dark:text-emerald-400">
            <CheckCircle2 className="w-6 h-6" />
          </div>
          <div>
            <div className="text-2xl font-bold text-slate-900 dark:text-white">Completed</div>
            <div className="text-xs text-slate-500 font-medium">Fully Signed Documents</div>
          </div>
        </div>

        <div className="bg-white dark:bg-slate-900 p-4 rounded-xl border border-slate-200 dark:border-slate-800 shadow-sm flex items-center gap-4">
          <div className="p-3 bg-indigo-50 dark:bg-indigo-950/50 rounded-xl text-indigo-600 dark:text-indigo-400">
            <ShieldCheck className="w-6 h-6" />
          </div>
          <div>
            <div className="text-2xl font-bold text-slate-900 dark:text-white">SHA-256</div>
            <div className="text-xs text-slate-500 font-medium">Audit Certified</div>
          </div>
        </div>
      </div>

      {/* Signature Requests List */}
      <div className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-6 space-y-4 shadow-sm">
        <div className="flex items-center justify-between">
          <h2 className="font-bold text-slate-900 dark:text-white text-base">Signature Workflows</h2>
        </div>

        <div className="p-8 text-center border border-dashed border-slate-200 dark:border-slate-800 rounded-xl space-y-3">
          <FileCheck2 className="w-10 h-10 text-indigo-500 mx-auto" />
          <div className="font-bold text-slate-900 dark:text-white text-base">Send Contracts & Agreements for Signature</div>
          <p className="text-xs text-slate-500 max-w-md mx-auto">
            Add multiple signers, track viewing and signing events live, and download immutable audit certificates.
          </p>
          <button
            onClick={() => setIsNewRequestOpen(true)}
            className="inline-flex items-center gap-2 bg-indigo-600 text-white px-4 py-2 rounded-xl text-xs font-semibold hover:bg-indigo-500 transition-all"
          >
            <Plus className="w-4 h-4" /> Start New Signature Request
          </button>
        </div>
      </div>

      {/* New Signature Request Modal */}
      {isNewRequestOpen && (
        <div className="fixed inset-0 bg-slate-900/60 backdrop-blur-sm z-50 flex items-center justify-center p-4">
          <div className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-6 w-full max-w-xl shadow-2xl space-y-4 max-h-[90vh] overflow-y-auto">
            <div className="flex items-center justify-between">
              <h3 className="text-lg font-bold text-slate-900 dark:text-white flex items-center gap-2">
                <Sparkles className="w-5 h-5 text-indigo-500" /> New E-Signature Request
              </h3>
              <button onClick={() => setIsNewRequestOpen(false)} className="text-slate-400 hover:text-slate-600">
                <X className="w-5 h-5" />
              </button>
            </div>

            <form onSubmit={handleCreateRequest} className="space-y-4">
              <div>
                <label className="text-xs font-semibold text-slate-500 block mb-1">Select Document</label>
                <select
                  required
                  value={documentId}
                  onChange={(e) => setDocumentId(e.target.value)}
                  className="w-full px-3.5 py-2.5 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-sm dark:text-white"
                >
                  <option value="">-- Select Document --</option>
                  {docs.map((d) => (
                    <option key={d.id} value={d.id}>
                      {d.name} (v{d.versionCount})
                    </option>
                  ))}
                </select>
              </div>

              <div>
                <label className="text-xs font-semibold text-slate-500 block mb-1">Request Title</label>
                <input
                  type="text"
                  required
                  placeholder="e.g. Master Agreement Signature"
                  value={title}
                  onChange={(e) => setTitle(e.target.value)}
                  className="w-full px-3.5 py-2.5 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-sm dark:text-white"
                />
              </div>

              <div>
                <label className="text-xs font-semibold text-slate-500 block mb-1">Message to Signers (Optional)</label>
                <textarea
                  rows={2}
                  placeholder="Please review and sign the attached agreement."
                  value={message}
                  onChange={(e) => setMessage(e.target.value)}
                  className="w-full p-3 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-xs dark:text-white"
                />
              </div>

              {/* Recipients Section */}
              <div className="space-y-3 pt-2">
                <div className="flex items-center justify-between">
                  <span className="text-xs font-bold text-slate-500 uppercase tracking-wider">Signers & Recipients</span>
                  <button
                    type="button"
                    onClick={handleAddRecipient}
                    className="text-xs text-indigo-600 dark:text-indigo-400 font-semibold hover:underline flex items-center gap-1"
                  >
                    <Plus className="w-3.5 h-3.5" /> Add Signer
                  </button>
                </div>

                {recipients.map((rec, idx) => (
                  <div key={idx} className="p-3 bg-slate-50 dark:bg-slate-800/50 rounded-xl border border-slate-200 dark:border-slate-700 space-y-2 relative">
                    <div className="flex items-center justify-between text-xs font-semibold text-slate-500">
                      <span>Signer #{idx + 1}</span>
                      {recipients.length > 1 && (
                        <button type="button" onClick={() => handleRemoveRecipient(idx)} className="text-red-500 hover:text-red-700">
                          Remove
                        </button>
                      )}
                    </div>
                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-2">
                      <input
                        type="text"
                        required
                        placeholder="Full Name"
                        value={rec.signerName}
                        onChange={(e) => handleRecipientChange(idx, "signerName", e.target.value)}
                        className="px-3 py-2 bg-white dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-xs dark:text-white"
                      />
                      <input
                        type="email"
                        required
                        placeholder="Email Address"
                        value={rec.signerEmail}
                        onChange={(e) => handleRecipientChange(idx, "signerEmail", e.target.value)}
                        className="px-3 py-2 bg-white dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-xs dark:text-white"
                      />
                    </div>
                  </div>
                ))}
              </div>

              <div className="flex justify-end gap-2 pt-2">
                <button type="button" onClick={() => setIsNewRequestOpen(false)} className="px-4 py-2 text-sm text-slate-600">
                  Cancel
                </button>
                <button type="submit" disabled={createSignatureMutation.isPending} className="px-4 py-2 bg-indigo-600 text-white rounded-xl text-sm font-medium">
                  {createSignatureMutation.isPending ? "Sending..." : "Send Signature Request"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
