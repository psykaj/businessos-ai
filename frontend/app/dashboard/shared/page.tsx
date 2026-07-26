"use client";

import { useState } from "react";
import {
  Globe,
  Share2,
  Lock,
  Plus,
  Copy,
  Check,
  Trash2,
  FileText,
  Clock,
  Eye,
  X,
  UserCheck
} from "lucide-react";
import { useDocuments } from "@/hooks/use-documents";
import { documentsService } from "@/lib/documents-service";

export default function SharedDocumentsPage() {
  const [activeTab, setActiveTab] = useState<"Internal" | "Public">("Public");
  const [isShareModalOpen, setIsShareModalOpen] = useState(false);
  const [copiedToken, setCopiedToken] = useState<string | null>(null);

  // Share form state
  const [documentId, setDocumentId] = useState("");
  const [permissionType, setPermissionType] = useState<"InternalUser" | "PublicLink">("PublicLink");
  const [accessLevel, setAccessLevel] = useState<"Read" | "Comment" | "Edit">("Read");
  const [sharedWithEmail, setSharedWithEmail] = useState("");
  const [passcode, setPasscode] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  const { data: documentsData } = useDocuments({ pageSize: 50 });
  const docs = documentsData?.items || [];

  const handleShareSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!documentId) return;

    setIsSubmitting(true);
    try {
      await documentsService.shareDocument(
        documentId,
        accessLevel,
        permissionType,
        permissionType === "InternalUser" ? sharedWithEmail : undefined,
        passcode || undefined
      );
      setIsShareModalOpen(false);
      setDocumentId("");
      setPasscode("");
      setSharedWithEmail("");
    } catch {
      // Error handling
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleCopyLink = (token: string) => {
    const link = `${window.location.origin}/api/documents/public/${token}`;
    navigator.clipboard.writeText(link);
    setCopiedToken(token);
    setTimeout(() => setCopiedToken(null), 2000);
  };

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 bg-gradient-to-r from-slate-900 via-indigo-950 to-slate-900 p-6 rounded-2xl text-white shadow-xl">
        <div>
          <h1 className="text-2xl font-bold tracking-tight flex items-center gap-2">
            <Globe className="w-7 h-7 text-indigo-400" />
            Shared Files & Public Links
          </h1>
          <p className="text-slate-300 text-sm mt-1">
            Manage team access permissions and external passcode-protected document links.
          </p>
        </div>
        <button
          onClick={() => setIsShareModalOpen(true)}
          className="flex items-center gap-2 bg-indigo-600 hover:bg-indigo-500 text-white px-4 py-2.5 rounded-xl font-medium text-sm transition-all shadow-md shadow-indigo-600/30"
        >
          <Share2 className="w-4 h-4" /> Share Document
        </button>
      </div>

      {/* Analytics Summary */}
      <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
        <div className="bg-white dark:bg-slate-900 p-4 rounded-xl border border-slate-200 dark:border-slate-800 shadow-sm flex items-center gap-4">
          <div className="p-3 bg-indigo-50 dark:bg-indigo-950/50 rounded-xl text-indigo-600 dark:text-indigo-400">
            <Globe className="w-6 h-6" />
          </div>
          <div>
            <div className="text-2xl font-bold text-slate-900 dark:text-white">Public Links</div>
            <div className="text-xs text-slate-500 font-medium">External Share Links</div>
          </div>
        </div>

        <div className="bg-white dark:bg-slate-900 p-4 rounded-xl border border-slate-200 dark:border-slate-800 shadow-sm flex items-center gap-4">
          <div className="p-3 bg-emerald-50 dark:bg-emerald-950/50 rounded-xl text-emerald-600 dark:text-emerald-400">
            <Lock className="w-6 h-6" />
          </div>
          <div>
            <div className="text-2xl font-bold text-slate-900 dark:text-white">Protected</div>
            <div className="text-xs text-slate-500 font-medium">Passcode Encrypted</div>
          </div>
        </div>

        <div className="bg-white dark:bg-slate-900 p-4 rounded-xl border border-slate-200 dark:border-slate-800 shadow-sm flex items-center gap-4">
          <div className="p-3 bg-purple-50 dark:bg-purple-950/50 rounded-xl text-purple-600 dark:text-purple-400">
            <Eye className="w-6 h-6" />
          </div>
          <div>
            <div className="text-2xl font-bold text-slate-900 dark:text-white">Access Count</div>
            <div className="text-xs text-slate-500 font-medium">Total Link Views</div>
          </div>
        </div>
      </div>

      {/* Content Area */}
      <div className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-6 space-y-4 shadow-sm">
        <div className="flex items-center justify-between">
          <h2 className="font-bold text-slate-900 dark:text-white text-base">Active Share Links</h2>
        </div>

        <div className="p-8 text-center border border-dashed border-slate-200 dark:border-slate-800 rounded-xl space-y-3">
          <Globe className="w-10 h-10 text-indigo-500 mx-auto" />
          <div className="font-bold text-slate-900 dark:text-white text-base">Share Files Safely with Clients & Team</div>
          <p className="text-xs text-slate-500 max-w-md mx-auto">
            Create public links with custom passcodes, expiration dates, and view trackers.
          </p>
          <button
            onClick={() => setIsShareModalOpen(true)}
            className="inline-flex items-center gap-2 bg-indigo-600 text-white px-4 py-2 rounded-xl text-xs font-semibold hover:bg-indigo-500 transition-all"
          >
            <Share2 className="w-4 h-4" /> Share Document
          </button>
        </div>
      </div>

      {/* Share Document Modal */}
      {isShareModalOpen && (
        <div className="fixed inset-0 bg-slate-900/60 backdrop-blur-sm z-50 flex items-center justify-center p-4">
          <div className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-6 w-full max-w-md shadow-2xl space-y-4">
            <div className="flex items-center justify-between">
              <h3 className="text-lg font-bold text-slate-900 dark:text-white flex items-center gap-2">
                <Share2 className="w-5 h-5 text-indigo-500" /> Share Document
              </h3>
              <button onClick={() => setIsShareModalOpen(false)} className="text-slate-400 hover:text-slate-600">
                <X className="w-5 h-5" />
              </button>
            </div>

            <form onSubmit={handleShareSubmit} className="space-y-4">
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
                      {d.name}
                    </option>
                  ))}
                </select>
              </div>

              <div>
                <label className="text-xs font-semibold text-slate-500 block mb-1">Share Type</label>
                <div className="grid grid-cols-2 gap-2">
                  <button
                    type="button"
                    onClick={() => setPermissionType("PublicLink")}
                    className={`py-2 px-3 text-xs font-semibold rounded-xl border transition-all ${
                      permissionType === "PublicLink"
                        ? "bg-indigo-600 text-white border-indigo-600"
                        : "bg-slate-50 border-slate-200 text-slate-600 dark:bg-slate-800 dark:border-slate-700 dark:text-slate-300"
                    }`}
                  >
                    Public Link
                  </button>
                  <button
                    type="button"
                    onClick={() => setPermissionType("InternalUser")}
                    className={`py-2 px-3 text-xs font-semibold rounded-xl border transition-all ${
                      permissionType === "InternalUser"
                        ? "bg-indigo-600 text-white border-indigo-600"
                        : "bg-slate-50 border-slate-200 text-slate-600 dark:bg-slate-800 dark:border-slate-700 dark:text-slate-300"
                    }`}
                  >
                    Internal User
                  </button>
                </div>
              </div>

              {permissionType === "InternalUser" && (
                <div>
                  <label className="text-xs font-semibold text-slate-500 block mb-1">User Email Address</label>
                  <input
                    type="email"
                    required
                    placeholder="colleague@company.com"
                    value={sharedWithEmail}
                    onChange={(e) => setSharedWithEmail(e.target.value)}
                    className="w-full px-3.5 py-2.5 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-sm dark:text-white"
                  />
                </div>
              )}

              {permissionType === "PublicLink" && (
                <div>
                  <label className="text-xs font-semibold text-slate-500 block mb-1">Passcode Protection (Optional)</label>
                  <input
                    type="text"
                    placeholder="Enter PIN passcode e.g. 9876"
                    value={passcode}
                    onChange={(e) => setPasscode(e.target.value)}
                    className="w-full px-3.5 py-2.5 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-sm dark:text-white"
                  />
                </div>
              )}

              <div className="flex justify-end gap-2 pt-2">
                <button type="button" onClick={() => setIsShareModalOpen(false)} className="px-4 py-2 text-sm text-slate-600">
                  Cancel
                </button>
                <button type="submit" disabled={isSubmitting} className="px-4 py-2 bg-indigo-600 text-white rounded-xl text-sm font-medium">
                  {isSubmitting ? "Creating Share..." : "Generate Share Link"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
